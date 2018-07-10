using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// This Input Manager is used only for buttons (Mouse and Keyboard)
// Mouse movements and mouse axis should be obtained from default Input Manager

public class cInputManager : MonoBehaviour  {
    private static cInputManager instance; 
    public static cInputManager Instance
    {
        get
        {
            if (instance == null)
            {
                Instantiate(Resources.Load("cInputManager"));
            }
            return instance;
        }
    }
	
	public cInput[] Inputs;			    // All inputs defined in the Inspector are stored in this variable
    //public bool LockInputs {get; set;}  // Freeze all inputs

    #region ===== SCAN FOR NEW KEY =====
    public bool IsScanning { get; set; } // If the script is scanning for a new key
    public string VirtualKeyName { get; set; }
    public cInput.ButtonType VirtualKeyType { get; set; }
    #endregion


    void Awake () {
		instance = this;
        IsScanning = false;
	}
	
	void Start() {
		checkDuplicateNames();
		LoadKeys();
	}
	
	void Update () {
        if (IsScanning) // If the script is scanning for a new input
            cInput.SetKey(VirtualKeyName, VirtualKeyType);
        else
            foreach (cInput input in cInputManager.Instance.Inputs)
            {			// Update the values of every Input
                input.UpdateValue();
            }
	}
	
	
	
	#region ===== INIT FUNCTIONS =====
	
	// Checks if there are inputs with same name
	private void checkDuplicateNames() {
		for (int i=0; i<Inputs.Length; i++) {
			for (int j=i+1; j<Inputs.Length; j++) {
				if (Inputs[i].Name == Inputs[j].Name)
					throw new System.Exception(Inputs[i].Name + ": Two inputs with same name.");
			}
		}
	}
	
	public void LoadKeys() {
		parseKeys();
		checkDuplicateKeys();
		checkModifiers();
	}
	
	// Parse all keys in the beginning for better performance
	private void parseKeys() {
		foreach (cInput input in cInputManager.Instance.Inputs) {			
			input.ParseKeys();
		}
	}
	
	// Check for duplicate input keys
	private void checkDuplicateKeys() {
		for (int i=0; i<Inputs.Length; i++) {
			for (int j=i+1; j<Inputs.Length; j++) {
				if (((Inputs[i].PositiveButton == Inputs[j].PositiveButton   ||   Inputs[i].PositiveButton == Inputs[j].NegativeButton   ||   Inputs[i].PositiveButton == Inputs[j].AltPositiveButton   ||   Inputs[i].PositiveButton == Inputs[j].AltNegativeButton)  &&  Inputs[i].PositiveButton!="")   ||
					((Inputs[i].NegativeButton == Inputs[j].PositiveButton   ||   Inputs[i].NegativeButton == Inputs[j].NegativeButton   ||   Inputs[i].NegativeButton == Inputs[j].AltPositiveButton   ||   Inputs[i].NegativeButton == Inputs[j].AltNegativeButton)  &&  Inputs[i].NegativeButton!="")   ||
					((Inputs[i].AltPositiveButton == Inputs[j].PositiveButton   ||   Inputs[i].AltPositiveButton == Inputs[j].NegativeButton   ||   Inputs[i].AltPositiveButton == Inputs[j].AltPositiveButton   ||   Inputs[i].AltPositiveButton == Inputs[j].AltNegativeButton)  &&  Inputs[i].AltPositiveButton!="")   ||
					((Inputs[i].AltNegativeButton == Inputs[j].PositiveButton   ||   Inputs[i].AltNegativeButton == Inputs[j].NegativeButton   ||   Inputs[i].AltNegativeButton == Inputs[j].AltPositiveButton   ||   Inputs[i].AltNegativeButton == Inputs[j].AltNegativeButton)  &&  Inputs[i].AltNegativeButton!=""))
					throw new System.Exception(Inputs[i].Name + " and " + Inputs[j].Name + " inputs have duplicate values in some virtual keys.");
			}
		}
	}
	
	
	// Record if there are inputs with same buttons but diferent modifiers
	// This method is called after the inputs are parsed
	private void checkModifiers() {
		foreach(cInput input in Inputs) {
			input.CheckModifiers();	
		}
	}
	
	#endregion
	
}
