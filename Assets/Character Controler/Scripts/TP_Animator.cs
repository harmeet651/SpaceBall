using UnityEngine;
using System.Collections;

public class TP_Animator : MonoBehaviour 
{
    public enum Direction
    {
        Stationary, Forward, Backward, Left, Right,
        LeftForward, RightForward, LeftBackward, RightBackward
    }

    public enum CharacterState 
    {
        Idle, Running, WalkingBackwards, WalkingBackwardLeft, WalkingBackwardRigh, 
        RunningLeft, RunningRight, RunningLeftForward, RunningRightForward, Jumping, Falling,
        Landing, Sliding, Using, Dead, ActionLocked
    }

    public static TP_Animator Instance;

    private CharacterState lastState;

    private Vector3 initialPosition = Vector3.zero;
    private Quaternion initialRotation = Quaternion.identity;

    public Direction MoveDirection { get; set; }
    public CharacterState State { get; set; }
    public bool IsDead { get; set; }


    void Awake()
    {
        Instance = this;
        
    }

    void Start()
    {
        initialPosition = TP_Controler.Instance._transform.position;
        initialRotation = TP_Controler.Instance._transform.rotation;
    }
    	
	void Update () 
    {
        DetermineCurrentState();
        ProcessCurrentState();
	}

    public void DetermineCurrentMoveDirection()
    {
        bool forward = false;
        bool backward = false;
        bool left = false;
        bool right = false;

        if (TP_Motor.Instance.MoveVector.z > 0)
            forward = true;
        if (TP_Motor.Instance.MoveVector.z < 0)
            backward = true;
        if (TP_Motor.Instance.MoveVector.x > 0)
            right = true;
        if (TP_Motor.Instance.MoveVector.x < 0)
            left = true;

        if (forward)
        {
            if (left)
                MoveDirection = Direction.LeftForward;
            else if (right)
                MoveDirection = Direction.RightForward;
            else
                MoveDirection = Direction.Forward;
        }
        else if (backward)
        {
            if (left)
                MoveDirection = Direction.LeftBackward;
            else if (right)
                MoveDirection = Direction.RightBackward;
            else
                MoveDirection = Direction.Backward;
        }
        else if (left)
            MoveDirection = Direction.Left;
        else if (right)
            MoveDirection = Direction.Right;
        else
            MoveDirection = Direction.Stationary;

    }

    void DetermineCurrentState()
    {
        if (State == CharacterState.Dead)
            return;

        if (!TP_Controler.CharacterController.isGrounded)
        {
            if (State != CharacterState.Falling  &&   
                State != CharacterState.Jumping  &&  
                State != CharacterState.Landing)
            {
                // We should be falling
                Fall();
            }
        }

        if (State != CharacterState.Falling &&
            State != CharacterState.Jumping &&
            State != CharacterState.Landing &&
            State != CharacterState.Using &&
            State != CharacterState.Sliding)
        {
            switch (MoveDirection)
            {
                case Direction.Stationary:
                    State = CharacterState.Idle;
                    break;
                case Direction.Forward:
                    State = CharacterState.Running;
                    break;
                case Direction.Backward:
                    State = CharacterState.WalkingBackwards;
                    break;
                case Direction.Left:
                    State = CharacterState.RunningLeft;
                    break;
                case Direction.Right:
                    State = CharacterState.RunningRight;
                    break;
                case Direction.LeftForward:
                    State = CharacterState.RunningLeftForward;
                    break;
                case Direction.RightForward:
                    State = CharacterState.RunningRightForward;
                    break;
                case Direction.LeftBackward:
                    State = CharacterState.WalkingBackwardLeft;
                    break;
                case Direction.RightBackward:
                    State = CharacterState.WalkingBackwardRigh;
                    break;
            }
        }
    }

    void ProcessCurrentState()
    {
        switch (State)
        {
            case CharacterState.Idle:
                Idle();
                break;
            case CharacterState.Running:
                Running();
                break;
            case CharacterState.WalkingBackwards:
                WalkingBackwards();
                break;
            case CharacterState.RunningLeft:
                RunningLeft();
                break;
            case CharacterState.RunningRight:
                RuningRight();
                break;
            case CharacterState.Jumping:
                Jumping();
                break;
            case CharacterState.Falling:
                Falling();
                break;
            case CharacterState.Landing:
                Landing();
                break;
            case CharacterState.Sliding:
                Sliding();
                break;
            case CharacterState.Using:
                Using();
                break;
            case CharacterState.Dead:
                Dead();
                break;
            case CharacterState.ActionLocked:
                ActionLocked();
                break;
        }
    }



    #region Charachter State Methods

    void Idle()
    {
        //animation.CrossFade("Idle");
    }

    void Running()
    {
        //animation.CrossFade("Running");
    }

    void WalkingBackwards()
    {
        //animation.CrossFade("WalkingBackwards");
    }

    void WalkingBackwardLeft()
    {
        //animation.CrossFade("WalkingBackwardLeft");
    }

    void WalkingBackwardRight()
    {
        //animation.CrossFade("WalkingBackwardRight");
    }

    void RunningLeft()
    {
        //animation.CrossFade("RunningLeft");
    }

    void RuningRight()
    {
        //animation.CrossFade("RunningRight");
    }

    void RunningLeftForward()
    {
        //animation.CrossFade("RunningLeftForward");
    }

    void RunningRightForward()
    {
        //animation.CrossFade("RunningRightForward");
    }

    void Jumping()
    {
        if ((!GetComponent<Animation>().isPlaying && TP_Controler.CharacterController.isGrounded) ||
            TP_Controler.CharacterController.isGrounded)
        {
            if (lastState == CharacterState.Running)
            {
                // animation.CrossFade("RunLand");
            }
            else
            {
                // animation.CrossFade("JumpLand");
            }
        }
        else if (!GetComponent<Animation>().IsPlaying("Jump"))
        {
            State = CharacterState.Falling;
            // animation.CrossFade("Falling");
        }
        else
        {
            State = CharacterState.Jumping;
            // Help determine if we fell to far
        }
    }

    void Falling()
    {
        if (TP_Controler.CharacterController.isGrounded)
        {
            if (lastState == CharacterState.Running)
            {
                // animation.CrossFade("RunLand");
            }
            else
            {
                // animation.CrossFade("JumpLand");
            }
            State = CharacterState.Landing;
        }
    }

    void Landing()
    {
        if (lastState == CharacterState.Running)
        {
            if (!GetComponent<Animation>().IsPlaying("RunLand"))
            {
                State = CharacterState.Running;
                // animation.Play("Running");
            }
        }
        else
        {
            if (!GetComponent<Animation>().IsPlaying("JumpLand"))
            {
                State = CharacterState.Idle;
                // animation.Play("Idle");
            }
        }
    }

    void Sliding()
    {
        if (!TP_Motor.Instance.IsSliding)
        {
            State = CharacterState.Idle;
            // animation.CrossFade("Idle");
        }
    }

    void Using()
    {
        if (!GetComponent<Animation>().isPlaying)
        {
            State = CharacterState.Idle;
            //animation.CrossFade("Idle");
        }
    }

    void Dead()
    {
        State = CharacterState.Dead;
    }

    void ActionLocked()
    {

    }

    #endregion


    #region Start Action Method

    public void Use()
    {
        State = CharacterState.Using;
        //animation.CrossFade("Using");
    }

    public void Jump()
    {
        if (!TP_Controler.CharacterController.isGrounded || IsDead || State == CharacterState.Jumping)
            return;

        lastState = State;
        State = CharacterState.Jumping;
        // animation.CrossFade("Jumping");
    }

    public void Fall()
    {
        if (IsDead)
            return;

        lastState = State;
        State = CharacterState.Falling;
        // animation.CrossFade("Falling");
    }

    public void Slide()
    {
        State = CharacterState.Sliding;
        // animation.CrossFade("Sliding");
    }

    public void Die()
    {
        // Initialize everything we need to die
        Dead();
    }

    public void Reset()
    {
        // Reset player to play again
        TP_Controler.Instance._transform.position = initialPosition;
        TP_Controler.Instance._transform.rotation = initialRotation;
    }
    #endregion
} 
