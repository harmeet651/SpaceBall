using UnityEngine;
using System.Collections;

// This is only to draw the GUI for the menu
public class cGUI : MonoBehaviour {


    private bool showMenu = false;

    private enum MenuType {KeyBinding, Camera, Player};
    private MenuType currentMenu = MenuType.KeyBinding;
    private Vector2 scrollPosition = Vector2.zero;

	void Start () {
	
	}
	

	void Update () {
        if (cInput.GetButtonDown("Menu"))
            showMenu = !showMenu;
	}

    void OnGUI()
    {
        if (showMenu)
        {
            GUI.BeginGroup(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 200, 600, 400));
            // All rectangles are now adjusted to the group. (0,0) is the topleft corner of the group.

            // We'll make a box so you can see where the group is on-screen.
            GUI.Box(new Rect(0, 0, 600, 400), "Settings");


            // Left buttons
            GUI.BeginGroup(new Rect(5, 30, 150, 400));
            if (GUI.Button(new Rect(0, 0, 140, 25), "Key Binding"))
                currentMenu = MenuType.KeyBinding;
            if (GUI.Button(new Rect(0, 35, 140, 25), "Camera"))
                currentMenu = MenuType.Camera;
            if (GUI.Button(new Rect(0, 70, 140, 25), "Player"))
                currentMenu = MenuType.Player;
            GUI.EndGroup();



            // Right side
            if (currentMenu == MenuType.KeyBinding)
            {

                scrollPosition = GUI.BeginScrollView(new Rect(160, 30, 440, 400), scrollPosition, new Rect(0, 0, 400, 400));
                int i = 0;
                foreach (cInput input in cInputManager.Instance.Inputs)
                {
                    if (!input.Lock)
                    {
                        GUI.Label(new Rect(0, i * 25, 130, 25), input.PositiveButtonName);
                        if (GUI.Button(new Rect(130, i * 25, 130, 20), input.PositiveButton))
                            cInput.SetKey(input.Name, cInput.ButtonType.PositiveButton);
                        if (GUI.Button(new Rect(260, i * 25, 130, 20), input.AltPositiveButton))
                            cInput.SetKey(input.Name, cInput.ButtonType.AltPositiveButton);
                        i++;

                        if (input.Type == cInput.InputType.Axis)
                        {
                            GUI.Label(new Rect(0, i * 25, 130, 25), input.NegativeButtonName);
                            if (GUI.Button(new Rect(130, i * 25, 130, 20), input.NegativeButton))
                                cInput.SetKey(input.Name, cInput.ButtonType.NegativeButton);
                            if (GUI.Button(new Rect(260, i * 25, 130, 20), input.AltNegativeButton))
                                cInput.SetKey(input.Name, cInput.ButtonType.AltNegativeButton);
                            i++;
                        }
                    }
                }

                GUI.EndScrollView();
            }
            else if (currentMenu == MenuType.Camera)
            {
                GUI.BeginGroup(new Rect(160, 30, 440, 400));
                TP_Camera.Instance.RMBRotate = GUI.Toggle(new Rect(0, 0, 130, 20), TP_Camera.Instance.RMBRotate, "RMB for rotation");

                GUI.Label(new Rect(0, 25, 200, 20), "Minimum distance:");
                TP_Camera.Instance.DistanceMin = GUI.HorizontalSlider(new Rect(200, 30, 200, 20), TP_Camera.Instance.DistanceMin, 0.5f, 2.0f);

                GUI.Label(new Rect(0, 50, 200, 20), "Maximum distance:");
                TP_Camera.Instance.DistanceMax = GUI.HorizontalSlider(new Rect(200, 55, 200, 20), TP_Camera.Instance.DistanceMax, 2.1f, 20.0f);

                GUI.Label(new Rect(0, 75, 200, 20), "Mouse sensitivity:");
                TP_Camera.Instance.MouseSensitivity = GUI.HorizontalSlider(new Rect(200, 80, 200, 20), TP_Camera.Instance.MouseSensitivity, 1.0f, 10.0f);

                GUI.Label(new Rect(0, 100, 200, 20), "Zoom sensitivity:");
                TP_Camera.Instance.MouseWheelSensitivity = GUI.HorizontalSlider(new Rect(200, 105, 200, 20), TP_Camera.Instance.MouseWheelSensitivity, 1.0f, 15.0f);

                GUI.Label(new Rect(0, 125, 200, 20), "Occlusion resume speed:");
                TP_Camera.Instance.DistanceResumeSmooth = GUI.HorizontalSlider(new Rect(200, 130, 200, 20), TP_Camera.Instance.DistanceResumeSmooth, 0.0f, 2.0f);

                GUI.Label(new Rect(0, 150, 200, 20), "Zoom smoothing:");
                TP_Camera.Instance.DistanceSmooth = GUI.HorizontalSlider(new Rect(200, 155, 200, 20), TP_Camera.Instance.DistanceSmooth, 0.0f, 2.0f);

                GUI.Label(new Rect(0, 175, 200, 20), "Fade starting distance:");
                TP_Camera.Instance.FadeDistance = GUI.HorizontalSlider(new Rect(200, 180, 200, 20), TP_Camera.Instance.FadeDistance, 1.0f, 5.0f);

                GUI.Label(new Rect(0, 200, 300, 100), "There are much more camera options in the inspector panel when you are building your game and you have more control. This is just for demonstration. Read the ReadMe file for more details.");
                GUI.EndGroup();
            }
            else if (currentMenu == MenuType.Player) 
            {
                GUI.BeginGroup(new Rect(160, 30, 440, 400));
                GUI.Label(new Rect(0, 0, 200, 20), "Run speed:");
                TP_Motor.Instance.RunSpeed = GUI.HorizontalSlider(new Rect(200, 0, 200, 20), TP_Motor.Instance.RunSpeed, 4.0f, 20f);

                GUI.Label(new Rect(0, 25, 200, 20), "Walk speed:");
                TP_Motor.Instance.WalkSpeed = GUI.HorizontalSlider(new Rect(200, 30, 200, 20), TP_Motor.Instance.WalkSpeed, 0.2f, 4.0f);

                GUI.Label(new Rect(0, 50, 200, 20), "Jump:");
                TP_Motor.Instance.JumpSpeed = GUI.HorizontalSlider(new Rect(200, 55, 200, 20), TP_Motor.Instance.JumpSpeed, 1.0f, 20.0f);

                GUI.Label(new Rect(0, 75, 200, 20), "Rotation speed:");
                TP_Motor.Instance.RotationSpeed = GUI.HorizontalSlider(new Rect(200, 80, 200, 20), TP_Motor.Instance.RotationSpeed, 1.0f, 10.0f);

                GUI.Label(new Rect(0, 100, 200, 20), "Gravity:");
                TP_Motor.Instance.Gravity = GUI.HorizontalSlider(new Rect(200, 105, 200, 20), TP_Motor.Instance.Gravity, 5.0f, 50.0f);

                GUI.Label(new Rect(0, 125, 200, 20), "Slide threshold:");
                TP_Motor.Instance.SlideThreshold = GUI.HorizontalSlider(new Rect(200, 130, 200, 20), TP_Motor.Instance.SlideThreshold, 0.01f, 0.99f);

                GUI.Label(new Rect(0, 150, 200, 20), "Max controlable slide:");
                TP_Motor.Instance.MaxControlableSlide = GUI.HorizontalSlider(new Rect(200, 155, 200, 20), TP_Motor.Instance.MaxControlableSlide, 0.01f, 0.99f);

                GUI.EndGroup();
            }

            /*
            Rect textArea = new Rect(5, 0, Screen.width, Screen.height);            
            TP_Camera.Instance.RMBRotate = GUI.Toggle(new Rect(5, 40, 130, 20), TP_Camera.Instance.RMBRotate, "RMB for rotation");
             * */
            GUI.EndGroup();
        }
        else
        {
            Rect textArea = new Rect(5, 0, Screen.width, Screen.height);
            GUI.Label(textArea, " ========== ESC - Settings ========== ");
            GUI.enabled = true;
        }

    }
}
