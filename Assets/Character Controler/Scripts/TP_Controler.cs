using UnityEngine;
using System.Collections;

public class TP_Controler : MonoBehaviour {
    public static CharacterController CharacterController;
    public static TP_Controler Instance;
    


    public Transform _transform { get; set; }
    


	void Awake () {
        CharacterController = GetComponent("CharacterController") as CharacterController;
        Instance = this;
        TP_Camera.AttachCamera();
        _transform = transform;

        
	}

	

	void Update () {
        if (Camera.main == null)
            return;

        TP_Motor.Instance.ResetMotor();

        if (!TP_Animator.Instance.IsDead &&
            TP_Animator.Instance.State != TP_Animator.CharacterState.Using &&
            TP_Animator.Instance.State != TP_Animator.CharacterState.Landing)
        {
            GetLocomotionInput();
            HandleActionInput();
        }
        else if (TP_Animator.Instance.IsDead)
        {
            if (Input.anyKey)
                TP_Animator.Instance.Reset();
        }
        
        
        TP_Motor.Instance.UpdateMotor();

        
	}

    void GetLocomotionInput()
    {
        var deadZone = 0.1f;
        if (cInput.GetAxis("Vertical") > deadZone || cInput.GetAxis("Vertical") < -deadZone  ||  cInput.GetAxis("VerticalRun") > deadZone || cInput.GetAxis("VerticalRun") < -deadZone)
            TP_Motor.Instance.MoveVector += new Vector3(0, 0, cInput.GetAxis("Vertical") + cInput.GetAxis("VerticalRun"));

        if (cInput.GetAxis("Horizontal") > deadZone || cInput.GetAxis("Horizontal") < -deadZone  ||  cInput.GetAxis("HorizontalRun") > deadZone || cInput.GetAxis("HorizontalRun") < -deadZone)
            TP_Motor.Instance.MoveVector += new Vector3(cInput.GetAxis("Horizontal") + cInput.GetAxis("HorizontalRun"), 0, 0);

        

        if (cInput.GetAxis("Turn") > deadZone || cInput.GetAxis("Turn") < -deadZone)
        {
            TP_Motor.Instance.Turn += cInput.GetAxis("Turn");
        }
		
		

        TP_Animator.Instance.DetermineCurrentMoveDirection();
        
    }

    void HandleActionInput()
    {
        if (cInput.GetButton("Jump"))
        {
            Jump();
        }

        if (cInput.GetButtonDown("Use"))
        {
            Use();
        }
    }

    void Jump()
    {
        TP_Motor.Instance.Jump();
        TP_Animator.Instance.Jump(); 
    }

    void Use()
    {
        TP_Animator.Instance.Use();
    }
}
