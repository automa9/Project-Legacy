using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem;

//MAKING ETOUCH AS REFRENCE TO THE ENHANCED TOUCH
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class PlayerTouchMovement : MonoBehaviour
{
    [SerializeField]
    private Vector2 JoystickSize = new Vector2(10,10);
    
    [SerializeField]
    private FloatingJoystick Joystick;

    [SerializeField]
    private NavMeshAgent playerNavMeshAgent;
    private Finger MovementFinger;
    private Vector2 MovementAmount;
    private Animator playerAnimator;

    public Vector3 velocity;

    void Start()
    {
        playerNavMeshAgent = GetComponent<NavMeshAgent>();
        playerAnimator = GetComponent<Animator>();
    }

    //ENABLING THE ENHANCED TOUCH SUPPORT ON MOVE UP AND DOWN
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandelFingerDown;
        ETouch.Touch.onFingerUp += HandleLoseFinger;
        ETouch.Touch.onFingerMove += HandleFingerMove;
    }


    //DISABLE THE ENHANCED TOUCH SUPPORT ON MOVE UP AND DOWN AND Inverse
    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= HandelFingerDown;
        ETouch.Touch.onFingerUp -= HandleLoseFinger;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
        EnhancedTouchSupport.Disable();
    }

    private void HandleFingerMove(Finger MovedFinger)
    {
        if(MovedFinger == MovementFinger)
        {
            Vector2 knobPosition;
            float maxMovement = JoystickSize.x / 2f;
            ETouch.Touch currentTouch = MovedFinger.currentTouch;

            if(Vector2.Distance(currentTouch.screenPosition, Joystick.RectTransform.anchoredPosition) > maxMovement)
            {
                knobPosition = (
                currentTouch.screenPosition - Joystick.RectTransform.anchoredPosition
                ).normalized 
                * maxMovement;

            }else {
                knobPosition = currentTouch.screenPosition - Joystick.RectTransform.anchoredPosition;
            }

            Joystick.knob.anchoredPosition = knobPosition;
            MovementAmount = knobPosition / maxMovement;
        }
    }

    private void HandleLoseFinger(Finger LostFinger)
    {
        if(LostFinger == MovementFinger)
        {
            MovementFinger = null;
            Joystick.knob.anchoredPosition = Vector2.zero;
            Joystick.gameObject.SetActive(false);
            MovementAmount = Vector2.zero; 
            //PlayerAnimator
        }
    }

    private void HandelFingerDown(Finger TouchedFinger)
    {
        if(MovementFinger == null && TouchedFinger.screenPosition.y <= Screen.height /2f)
        {
            MovementFinger = TouchedFinger;
            MovementAmount = Vector2.zero;
            Joystick.gameObject.SetActive(true);
            Joystick.RectTransform.sizeDelta = JoystickSize;
            Joystick.RectTransform.anchoredPosition = ClampStartPosition(TouchedFinger.screenPosition);
        }
    }

    private Vector2 ClampStartPosition(Vector2 StartPosition)
    {
        if(StartPosition.x < JoystickSize.x/2 )
        {
            StartPosition.x = JoystickSize.x/2 ;
        }

        if(StartPosition.y < JoystickSize.y / 2)
        {
            StartPosition.y = JoystickSize.y /2;
        }
        else if (StartPosition.y > Screen.height - JoystickSize.y / 2)
        {
            StartPosition.y = Screen.height - JoystickSize.y / 2;
        }
        return StartPosition;
    }

    private void FixedUpdate()
    {
        Vector3 scaledMovement = playerNavMeshAgent.speed * Time.deltaTime * new Vector3(MovementAmount.y, 0, -MovementAmount.x);
        playerNavMeshAgent.Move(scaledMovement);
        playerNavMeshAgent.transform.LookAt(playerNavMeshAgent.transform.position, Vector3.up);
        
        playerAnimator.SetFloat("moveX", MovementAmount.x);
        playerAnimator.SetFloat("moveZ", MovementAmount.y);

        //print("MovementAmount.x: " + MovementAmount.x);
        //print("MovementAmount.y: " + MovementAmount.y);

        if(MovementAmount.x == 0 &&MovementAmount.y ==0){
            playerAnimator.SetBool("isMoving",true);
        }

        if(MovementAmount.x > 0.5){//Move East
            playerAnimator.SetBool("isMoving",true);
        }
        

        
    }
}
