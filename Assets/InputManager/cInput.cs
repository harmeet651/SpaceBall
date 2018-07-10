using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Represents one custom input
[System.Serializable]
public class cInput {
	public enum InputType {Button, Axis};
	public enum ButtonType {PositiveButton, NegativeButton, AltPositiveButton, AltNegativeButton};
	
	public string Name;					// Name of the action that is being handled
	public InputType Type;				// Defines the input type: Button or Axis (NOTE: If the input is Button, only the PositiveButton will be evaluated)
    public string PositiveButtonName;   // Name of the positive button (this will be shown in the game options menu)
    public string NegativeButtonName;   // Name of the negative button (this will be shown in the game options menu)
	public string PositiveButton;		// This button incrases the value (MAX = 1)
    public string NegativeButton;		// This button decreases the value (MIN = -1), NegativeButton doesn't affect Button input types
	public string AltPositiveButton;	// Alternative button for increasing
    public string AltNegativeButton;	// Alternative button for decreasing, AltNegativeButton doesn't affect Button input types
	public float Gravity = 5;			// How fast should the value return to zero 
    public float Sensitivity = 5;		// How fast should the script react on the pressed button, NOTE: Gravity and Sensitivity doesnt affect Button types only Axis  
    public bool Lock = false;         // If this is true, the input fields can not be changed any more (useful for freezing some inputs)
	
	// Keys are parsed into this array keys for better performance
	private KeyCode[] _positiveButton;
	private KeyCode[] _negativeButton;		
	private KeyCode[] _altPositiveButton;	
	private KeyCode[] _altNegativeButton;
	
	// If the key consists only of one button, these arrays are holding the information if an other
	// key is registered with this button plus a modifier
	private KeyCode[] _otherModifiersPositiveButton = null;
	private KeyCode[] _otherModifiersNegativeButton = null;
	private KeyCode[] _otherModifiersAltPositiveButton = null;
	private KeyCode[] _otherModifiersAltNegativeButton = null;
	
	private float inputValue;					// The current value of the input
	private float velInputValue = 0.0f;			// The current velocity, used for Gravity and Sensitivity
    private bool buttonDown;                    // Works only for Button Inputs, true on button  down
    private bool buttonUp;                      // Works only for Button Inputs, true on button up
	private static float MAX_VALUE = 1.0f;
	private static float MIN_VALUE = -1.0f;
	private static float ZERO_VALUE = 0.0f;		// The default value if the button is not pressed
	
	// Possible modifiers
	private static KeyCode[] modifiers = {
		KeyCode.LeftShift, KeyCode.RightShift, 
		KeyCode.LeftAlt, KeyCode.RightAlt, 
		KeyCode.LeftControl, KeyCode.RightControl,
		KeyCode.LeftCommand, KeyCode.RightCommand
	};

    private const int keyCount = 450;           // Max KeyCode
    
	
	
	// Default constructor
	public cInput() {
	}	
	
	
	public float GetValue () {	return inputValue; } // Returns the current value of this input
    public bool GetButtonDown() { return buttonDown; }
    public bool GetButtonUp() { return buttonUp; }
	
	
	#region ===== KEY PARSING =====
	
	/// <summary>
    /// This function parses all defined keys into an array of KeyCodes for better performance.
	/// </summary> 
	public void ParseKeys() {
		_positiveButton = parseKey(PositiveButton);
		_negativeButton = parseKey(NegativeButton);
		_altPositiveButton = parseKey(AltPositiveButton);
		_altNegativeButton = parseKey(AltNegativeButton);
		
		if (PositiveButton != ""   &&   PositiveButton == NegativeButton)
			throw new System.Exception("Positive and Negative buttons can not have the same keys.");
		if (AltPositiveButton != ""   &&   AltPositiveButton == AltNegativeButton)
			throw new System.Exception("AltPositive and AltNegative buttons can not have the same keys.");
	}
	
	// Check if array contains element
	private static bool containsKey(KeyCode[] array, KeyCode element) {
		foreach (KeyCode code in array) {
			if (code == element) {
				return true;
			}
		}
		return false;
	}
	
	// Parse the specified key and check the syntax
	private KeyCode[] parseKey(string keys) {
		if (keys == "") // If the key is not set
			return null;
		
		try {
			string[] arrayKeys = keys.Split('+'); 		// Split the input keys into an array of keys
			if (arrayKeys.Length > 2) { 				// Only one or two keys can be a virtual key
				throw new System.Exception(keys + " -  Key can consist of maximum two elements: modifier+key");		
			} else if (arrayKeys.Length == 2) { 		// Key with modifier
				KeyCode key1 = (KeyCode)string2KeyCode(arrayKeys[0]); 
				KeyCode key2 = (KeyCode)string2KeyCode(arrayKeys[1]);
				if (!containsKey(modifiers, key1))
					throw new System.Exception(keys + " -  First key must be a modifier.");
				if (containsKey(modifiers, key2))
					throw new System.Exception(keys + " -  Second key can not be a modifier");
				
				KeyCode[] parsed = {key1, key2};
				return parsed;
			} else { 									// Key without modifier
				KeyCode key = (KeyCode)string2KeyCode(arrayKeys[0]);
				if (containsKey(modifiers, key))
					throw new System.Exception(keys + " -  Key can not be a modifier.");
				
				KeyCode[] parsed = {key};
				return parsed;
			}
		}
		catch(KeyNotFoundException e) {
			Debug.Log(e.Message);
			throw new System.Exception(keys + " -  The given key is not present in the dictionary.");
		}
	}

    // Converts a string to the equivalent integer KeyCode. Throws an exeption if the string does not match any KeyCode.
    private int string2KeyCode(string key)
    {
        for (int i = 0; i < keyCount; i++)
        {
            if (((KeyCode)i).ToString() == key)
                return i;
        }
        throw new System.Exception(key + " - The given key is not present in the dictionary.");
    }
	#endregion
	
	
	
	
	#region ===== MODIFIER ANALYZING =====
	
	// Record if there are inputs with same buttons but diferent modifiers
	// NOTE: These methods are called after the input is parsed	
	
	public void CheckModifiers() {
		if (_positiveButton!=null  &&  _positiveButton.Length == 1)
			_otherModifiersPositiveButton = checkOtherInputs(_positiveButton[0]);	
		if (_negativeButton!=null  &&  _negativeButton.Length == 1)
			_otherModifiersNegativeButton = checkOtherInputs(_negativeButton[0]);	
		if (_altPositiveButton!=null  &&  _altPositiveButton.Length == 1)
			_otherModifiersAltPositiveButton = checkOtherInputs(_altPositiveButton[0]);	
		if (_altNegativeButton!=null  &&  _altNegativeButton.Length == 1)
			_otherModifiersAltNegativeButton = checkOtherInputs(_altNegativeButton[0]);			
	}
	
	// Search if other inputs contain this key
	private KeyCode[] checkOtherInputs(KeyCode key) {
		List<KeyCode> otherModifiers = new List<KeyCode>(); 
		
		foreach (cInput input in cInputManager.Instance.Inputs) {
			List<KeyCode> tmpModifiers = input.SearchForModifiers(key);
			otherModifiers.AddRange(tmpModifiers);
		}
		
		// Remove doubles
		for (int i=0; i<otherModifiers.Count; i++) {
			for (int j=i+1; j<otherModifiers.Count; j++) {
				if (otherModifiers[i] == otherModifiers[j])
					otherModifiers.RemoveAt(j);
			}
		}
		
		if (otherModifiers.Count>0)
			return otherModifiers.ToArray();
		else
			return null;
	}
	
	public List<KeyCode> SearchForModifiers(KeyCode key) {
		List<KeyCode> tmpModifiers = new List<KeyCode>();
		if (_positiveButton!=null  &&  _positiveButton.Length == 2  &&  _positiveButton[1] == key)
			tmpModifiers.Add(_positiveButton[0]);		
		if (_negativeButton!=null  &&  _negativeButton.Length == 2  &&  _negativeButton[1] == key)
			tmpModifiers.Add(_negativeButton[0]);
		if (_altPositiveButton!=null  &&  _altPositiveButton.Length == 2  &&  _altPositiveButton[1] == key)
			tmpModifiers.Add(_altPositiveButton[0]);
		if (_altNegativeButton!=null  &&  _altNegativeButton.Length == 2  &&  _altNegativeButton[1] == key)
			tmpModifiers.Add(_altNegativeButton[0]);
		
		return tmpModifiers;		
	}
	
	#endregion
	
	
		
	
	#region ===== INPUT CALCULATION METHODS =====
	
	// Calculates the new value
	// PARAM desiredValue - calculates the value to achieve this value
	private void calculateValue(float desiredValue) {
		inputValue = Mathf.SmoothDamp(inputValue, desiredValue, ref velInputValue, Sensitivity/100.0f);

		// Round the value to 3 DP		
		inputValue = Mathf.Round(inputValue*1000.0f);
		inputValue /= 1000.0f;
	}	
	
	
	// Check if the specified key is pressed
	private bool isPressed(KeyCode[] keys, KeyCode[] modifierArray) {
		if (keys == null) { // If the key is null try to parse it again, the key shoud never be null when this method is called (if it is null then it can not be parsed and the key is not usable)
			ParseKeys();
			return false;
		}
		
		foreach (KeyCode key in keys) {
			if (!Input.GetKey(key))
				return false;
		}	
		
		// Check if any of these modifiers is pressed, they should not be pressed because these modifiers are contained in other virtual keys
		if (modifierArray != null)
			foreach(KeyCode key in modifierArray)
				if (Input.GetKey(key))
					return false;
		
		return true;
	}	
	
	// Checks if the button is pressed and calculates the value
	public void UpdateValue() {
		if (Type == InputType.Button) {				// Check the type of the input
			if (PositiveButton != "") {				// If the value is not set, do nothing
				if (isPressed(_positiveButton, _otherModifiersPositiveButton)) {	// If the button is pressed & no modifier fron the list is pressed -> increase the value, else -> put the value back to zero
                    buttonPressed();
					inputValue = MAX_VALUE;
					return; // Only for buttons (no transition)
				} else {
                    buttonReleased();
					inputValue = ZERO_VALUE;
				}
			}			
			
			if (AltPositiveButton != "") {			// Check the alternative buttons
				if (isPressed(_altPositiveButton, _otherModifiersAltPositiveButton)) {	// If the button is pressed -> increase the value, else -> put the value back to zero
                    buttonPressed();
					inputValue = MAX_VALUE;
                    return; // Only for buttons (no transition)
				} else {
                    buttonReleased();
					inputValue = ZERO_VALUE;
				}
			}
			
		} else if (Type == InputType.Axis) {
            float refValue = ZERO_VALUE;
			if (PositiveButton != ""  &&  NegativeButton != "") {	// If the type is Axis -> both values should be set
				if (isPressed(_positiveButton, _otherModifiersPositiveButton)) {					// If the button is pressed -> increase the value
                    refValue = MAX_VALUE;
				} else if (isPressed(_negativeButton, _otherModifiersNegativeButton)) {			    // If the button is pressed -> decrease the value
                    refValue = MIN_VALUE;
				} else {
                    refValue = ZERO_VALUE;							// If no button is pressed return to the default value
				}				
			}
			
			if (AltPositiveButton != ""   &&   AltNegativeButton != "") {	// Check the alternative buttons
				if (isPressed(_altPositiveButton, _otherModifiersAltPositiveButton)) {					// If the button is pressed -> increase the value
                    refValue = MAX_VALUE;
				} else if (isPressed(_altNegativeButton, _otherModifiersAltNegativeButton)) {			// If the button is pressed -> decrease the value
                    refValue = MIN_VALUE;
				}
			}
            calculateValue(refValue);
		}
		
	}

    private void buttonPressed()
    {
        if (inputValue == ZERO_VALUE)
            buttonDown = true;
        else
            buttonDown = false;
    }

    private void buttonReleased()
    {
        if (inputValue == MAX_VALUE)
            buttonUp = true;
        else
            buttonUp = false;
    }
		
	#endregion
	
		
	
		
	#region ===== SET AND GET KEY FUNCTIONS =====
	
	/// <summary>
	/// Gets the current value of an axis.
	/// </summary>
	/// <param name="axisName">Name of axis</param>
	/// <returns></returns>
	public static float GetAxis (string axisName) {
		foreach (cInput input in cInputManager.Instance.Inputs) {		// Find the axis
			if (input.Name == axisName   &&   input.Type == InputType.Axis)
				return input.GetValue();
		}
		throw new System.Exception(axisName + ": Can not find axis with given name");	// throw exception if the axisName doesn't exist
	}	
	
	/// <summary>
    /// Returns true if button is pressed, else returns false.
	/// </summary>
	/// <param name="buttonName">Target button name</param>
	/// <returns></returns>
	public static bool GetButton (string buttonName) {
		foreach (cInput input in cInputManager.Instance.Inputs) {		// find the button
			if (input.Name == buttonName   &&   input.Type == InputType.Button) {
				if (input.GetValue() == MAX_VALUE)
					return true;
				else
					return false;
			}
		}
		throw new System.Exception(buttonName + ": Can not find button with given name");	// throw exception if the buttonBame doesn't exist
	}

    /// <summary>
    /// Returns true on buttns down.
    /// </summary>
    /// <param name="buttonName">Target button name</param>
    /// <returns></returns>
    public static bool GetButtonDown(string buttonName)
    {
        foreach (cInput input in cInputManager.Instance.Inputs)
        {		// find the button
            if (input.Name == buttonName && input.Type == InputType.Button)
            {
                return input.GetButtonDown();
            }
        }
        throw new System.Exception(buttonName + ": Can not find button with given name");	// throw exception if the buttonBame doesn't exist
    }

    /// <summary>
    /// Returns true on button up.
    /// </summary>
    /// <param name="buttonName">Target button name</param>
    /// <returns></returns>
    public static bool GetButtonUp(string buttonName)
    {        
        foreach (cInput input in cInputManager.Instance.Inputs)
        {		// find the button
            if (input.Name == buttonName && input.Type == InputType.Button)
            {
                return input.GetButtonUp();
            }
        }
        throw new System.Exception(buttonName + ": Can not find button with given name");	// throw exception if the buttonBame doesn't exist
    }

    /// <summary>
    /// Sets a new key for the given virtual key name. Deletes other virtual keys with that same key.
    /// </summary>
    /// <param name="name">Target virtual key name</param>
    /// <param name="button">New button</param>
    /// <param name="type">Type of the new button</param>
    public static void SetKey(string name, string button, ButtonType type)
    {
        bool changed = false;


        // Check for Locked keys, their key binds can not be changed
        foreach (cInput input in cInputManager.Instance.Inputs)
        {
            if (input.Lock && (input.PositiveButton == button || input.NegativeButton == button || input.AltPositiveButton == button || input.AltNegativeButton == button))
                return; 
        }


        // Find the input and change the key
        foreach (cInput input in cInputManager.Instance.Inputs)
        {
            if (input.Name == name && !input.Lock)
            {
                if (type == ButtonType.PositiveButton)
                    input.PositiveButton = button;
                else if (type == ButtonType.NegativeButton)
                    input.NegativeButton = button;
                else if (type == ButtonType.AltPositiveButton)
                    input.AltPositiveButton = button;
                else if (type == ButtonType.AltNegativeButton)
                    input.AltNegativeButton = button;
                changed = true;
            }
        }

        if (changed)  // If the input is found
        {
            // Check if other inputs have this key, if yes then delete that virtual key
            foreach (cInput input in cInputManager.Instance.Inputs)
            {
                if (input.Name != name)
                {
                    if (input.PositiveButton == button)
                        input.PositiveButton = "";
                    if (input.NegativeButton == button)
                        input.NegativeButton = "";
                    if (input.AltPositiveButton == button)
                        input.AltPositiveButton = "";
                    if (input.AltNegativeButton == button)
                        input.AltNegativeButton = "";
                }
            }

            cInputManager.Instance.LoadKeys(); // Re-parse all keys and check new modifiers
        }
        else
            throw new System.Exception(name + ": Can not find virtual key with given name or the virtual key is set to be ignored.");        
    }

    /// <summary>
    /// Sets a new key for the given virtual key name. The new key is the next pressed key. 
    /// </summary>
    /// <param name="name">Target virtual key name</param>
    /// <param name="type">Type of the new button</param>
    public static void SetKey(string name, ButtonType type)
    {
        // Save key name and type
        cInputManager.Instance.IsScanning = true;
        cInputManager.Instance.VirtualKeyName = name;
        cInputManager.Instance.VirtualKeyType = type;

        if (Input.anyKey)
        {
            string modifier = "";
            string key =  "";
            for (int i = 1; i < keyCount; i++ )
            {
                if (Input.GetKey((KeyCode)i))
                {
                    if (containsKey(modifiers, (KeyCode)i))
                        modifier = ((KeyCode)i).ToString();
                    else
                        key = ((KeyCode)i).ToString();
                }
            }

            if (key != "")
            {
                if (modifier != "")
                    key = modifier + "+" + key;

                SetKey(name, key, type);
                cInputManager.Instance.IsScanning = false;
            }
        }
    }
		
	#endregion
	
	
}