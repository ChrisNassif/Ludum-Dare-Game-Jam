using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public static Camera Camera;
    public static CharacterController charController;

    public PlayerInput playerControls;

    public InputAction Forward;
    public bool isPressedForward = false;
    public InputAction Back;
    public bool isPressedBack = false;
    public InputAction Right;
    public bool isPressedRight = false;
    public InputAction Left;
    public bool isPressedLeft = false;
    public InputAction Jump;
    public bool isPressedJump = false;
    public InputAction MouseX;
    public float mouseX;
    public float rotationX = 0f;
    public float sensitivityX = 5f;
    public float clampX = 85f;
    public InputAction MouseY;
    public float mouseY;
    public float rotationY = 0f;
    public float sensitivityY = 0.15f;

    public float walkSpeed = 27.5f;
    public float jumpVelocity = 40f;
    public float gravityAcceleration = -3f;
    public float maxGravitySpeed = 20f;

    public float dX = 0;
    public float dY = 0;
    public float dZ = 0;

    //states
    public enum States
    {
        Standing,
        Airborne,
        Jumping,
        CanTranslate,
    }

    public static States currentState;
    public static Dictionary<Enum, List<Enum>> stateMap;

    //materials


    public void onEnable()
    {
        Forward.Enable();
    }

    public void onDisable()
    {
        Forward.Disable();
    }

    void onForwardPressed(InputAction.CallbackContext context)
    {
        isPressedForward = true;
    }   

    void onForwardReleased(InputAction.CallbackContext context)
    {
        isPressedForward = false;
        dX = 0;
        dZ = 0;
    } 

    void onBackPressed(InputAction.CallbackContext context)
    {
        isPressedBack = true;
    }   

    void onBackReleased(InputAction.CallbackContext context)
    {
        isPressedBack = false;
        dX = 0;
        dZ = 0;
    } 

    void onRightPressed(InputAction.CallbackContext context)
    {
        isPressedRight = true;
    }   

    void onRightReleased(InputAction.CallbackContext context)
    {
        isPressedRight = false;
        dX = 0;
        dZ = 0;
    } 

    void onLeftPressed(InputAction.CallbackContext context)
    {
        isPressedLeft = true;
    }   

    void onLeftReleased(InputAction.CallbackContext context)
    {
        isPressedLeft = false;
        dX = 0;
        dZ = 0;
    }
    
    void onJumpPressed(InputAction.CallbackContext context)
    {
        isPressedJump = true;
    }   

    void onJumpReleased(InputAction.CallbackContext context)
    {
        isPressedJump = false;
    } 


    public static bool isGrounded()
    {
        return charController.isGrounded;
    }

    public static void resetState()
    {
        if (isGrounded())
            currentState = States.Standing;
        else
            currentState = States.Airborne;
    }   

    public static bool canChangeState(Enum State)
    {
        return stateMap[currentState].Contains(State);
    }

    public void ControlsAwake(PlayerInput input)
    {
        if (input != null)
        {
            playerControls = input;

            Forward = playerControls.actions["Forward"];
            Forward.started += context => onForwardPressed(context);
            Forward.canceled += context => onForwardReleased(context);

            Back = playerControls.actions["Back"];
            Back.started += context => onBackPressed(context);
            Back.canceled += context => onBackReleased(context);

            Right = playerControls.actions["Right"];
            Right.started += context => onRightPressed(context);
            Right.canceled += context => onRightReleased(context);

            Left = playerControls.actions["Left"];
            Left.started += context => onLeftPressed(context);
            Left.canceled += context => onLeftReleased(context);

            Jump = playerControls.actions["Jump"];
            Jump.started += context => onJumpPressed(context);
            Jump.canceled += context => onJumpReleased(context);

            MouseX = playerControls.actions["MouseX"];
            MouseX.performed += context => mouseX = context.ReadValue<float>() * sensitivityX;

            MouseY = playerControls.actions["MouseY"];
            MouseY.performed += context => mouseY = context.ReadValue<float>() * sensitivityY;
        }
    }

    void Awake()
    {
        //instantiate objects
        charController = GetComponent<CharacterController>();
        Camera = transform.Find("Main Camera").GetComponent<Camera>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //initialize state map
        stateMap = new Dictionary<Enum, List<Enum>>();
        stateMap.Add(States.Standing, new List<Enum> {States.Jumping,  States.CanTranslate});
        stateMap.Add(States.Airborne, new List<Enum> {States.CanTranslate});
    }

    void FixedUpdate()
    {

        //
        //reset all variables
        //
        
        dX = 0;
        dZ = 0;

        if (isGrounded())
        {         
            dY = gravityAcceleration; //weird bug
        }
        else
        {
            if (gravityAcceleration < -1 * maxGravitySpeed)
                dY += -1 * maxGravitySpeed;
            else
                dY += gravityAcceleration;
        }


        //
        //rotation
        //

        transform.Rotate(Vector3.up, mouseX * Time.fixedDeltaTime); //rotate horizontally
        
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -clampX, clampX); //clamp mouseX
        Vector3 targetRotation = transform.eulerAngles; //rotate only the camera
        targetRotation.x = rotationX;
        Camera.transform.eulerAngles = targetRotation;


        //
        //translation
        //

        if (isPressedForward && canChangeState(States.CanTranslate))
        {
            dX += (transform.forward * walkSpeed).x;
            dZ += (transform.forward * walkSpeed).z;
        }
        if (isPressedBack && canChangeState(States.CanTranslate))
        {
            dX += -1 * (transform.forward * walkSpeed).x;
            dZ += -1 * (transform.forward * walkSpeed).z;
        }
        if (isPressedRight && canChangeState(States.CanTranslate))
        {
            dX += (transform.right * walkSpeed).x;
            dZ += (transform.right * walkSpeed).z;
        }
        if (isPressedLeft && canChangeState(States.CanTranslate))
        {
            dX += -1 * (transform.right * walkSpeed).x;
            dZ += -1 * (transform.right * walkSpeed).z;
        }


        //
        //change state
        //

        if (currentState == States.Airborne && isGrounded())
        {
            currentState = States.Standing;
        }

        if (isPressedJump && canChangeState(States.Jumping) && isGrounded())
        {
            if (canChangeState(States.Airborne))
                currentState = States.Airborne;

            dY = jumpVelocity;
        }


        //
        //Actually doing stuff with states
        //
        
        


        /*try
        {
            Debug.Log("Camera Direction: " + Camera.gameObject.transform.forward + "           Object Position: " + affectedObject.transform.position);
        }
        catch{

        }*/

        charController.Move(new Vector3(dX, dY, dZ) * Time.fixedDeltaTime);
        //Debug.Log("Push Value: " + pushValue);
        //Debug.Log("current state: " + currentState  + "        isGrounded: " + isGrounded() + "          Y displacement: " + dY + "             X displacement: " + dX + "           Z displacement: " + dZ);
    }
}