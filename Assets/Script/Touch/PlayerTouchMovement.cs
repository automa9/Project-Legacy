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

    public Transform playerTransform;

    void Start()
    {
        //Joystick.gameObject.SetActive(false);
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
        if(MovedFinger == MovementFinger && Joystick != null)
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
            MovementAmount.x = 0;
            MovementAmount.y= 0;
            ResetAnim();
            //PlayerAnimator
        }
    }

    private void HandelFingerDown(Finger TouchedFinger)
    {
        if(Joystick != null){
            if(MovementFinger == null && TouchedFinger.screenPosition.y <= Screen.height /2f)
            {
                MovementFinger = TouchedFinger;
                MovementAmount = Vector2.zero;
                Joystick.gameObject.SetActive(true);
                Joystick.RectTransform.sizeDelta = JoystickSize;
                Joystick.RectTransform.anchoredPosition = ClampStartPosition(TouchedFinger.screenPosition);
            }
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

    public void ResetAnim(){
        playerAnimator.SetBool("SideLeft",false);
        playerAnimator.SetBool("Backward",false);
        playerAnimator.SetBool("SideRight",false);
        playerAnimator.SetBool("Front",false);
    }

    private void FixedUpdate()
    {
       // GetPlayerFacingDirection();

        Vector3 scaledMovement = playerNavMeshAgent.speed * Time.deltaTime * new Vector3(MovementAmount.y, 0, -MovementAmount.x);

        playerNavMeshAgent.Move(scaledMovement);

        //playerNavMeshAgent.transform.LookAt(playerNavMeshAgent.transform.position + scaledMovement, Vector3.up);

        // print();
        // print("MovementAmount.x: " + MovementAmount.x + "MovementAmount.y: " + MovementAmount.y );

    

//==================================================================
        if(MovementAmount.x !=0 && MovementAmount.y !=0){
            playerAnimator.SetBool("isMoving",true);
        }else{
            playerAnimator.SetBool("isMoving",false);
            playerAnimator.SetBool("SideLeft",false);
            playerAnimator.SetBool("Backward",false);
            playerAnimator.SetBool("SideRight",false);
            playerAnimator.SetBool("Front",false);
        }
        
        

         Vector3 playerToThis = transform.position - playerTransform.position;
        playerToThis.y = 0; // Consider only the horizontal plane

        // Normalize the vector to get the facing direction
        Vector3 normalizedDir = playerToThis.normalized;

        // Calculate the angle between the normalized direction and North (positive Z axis)
        float angle = Vector3.SignedAngle(Vector3.forward, normalizedDir, Vector3.up);

        // Convert angle to positive value
        if (angle < 0)
            angle += 360;

        //Debug.Log(angle);
        // Determine the facing direction based on the angle
        if (angle >= 22.5f && angle < 67.5f){
            Debug.Log("NorthEast");
            playerAnimator.SetFloat("moveX", MovementAmount.x);
            playerAnimator.SetFloat("moveZ", -MovementAmount.y);
        }
           
        else if (angle >= 67.5f && angle < 112.5f){//facing East
            Debug.Log("East");

            playerAnimator.SetFloat("moveX", MovementAmount.y);
            playerAnimator.SetFloat("moveZ", -MovementAmount.x);
        }
            
        else if (angle >= 112.5f && angle < 157.5f){
            Debug.Log("SouthEast");
            playerAnimator.SetFloat("moveX", MovementAmount.y);
            playerAnimator.SetFloat("moveZ", -MovementAmount.x);
        }
            
        else if (angle >= 157.5f && angle < 202.5f){
           Debug.Log("South");
            playerAnimator.SetFloat("moveX", -MovementAmount.x);
            playerAnimator.SetFloat("moveZ", MovementAmount.y);
        }
            
        else if (angle >= 202.5f && angle < 247.5f){
            Debug.Log("SouthWest");
            playerAnimator.SetFloat("moveX", -MovementAmount.x);
            playerAnimator.SetFloat("moveZ", MovementAmount.y);
        }
            
        else if (angle >= 247.5f && angle < 292.5f){
            Debug.Log("West");
            playerAnimator.SetFloat("moveX", -MovementAmount.y);
            playerAnimator.SetFloat("moveZ", MovementAmount.x);
        }
            
        else if (angle >= 292.5f && angle < 337.5f){
            Debug.Log("NorthWest");
            playerAnimator.SetFloat("moveX", MovementAmount.x);
            playerAnimator.SetFloat("moveZ", -MovementAmount.y);
        }else{
            Debug.Log("North");
            playerAnimator.SetFloat("moveX", MovementAmount.x);
            playerAnimator.SetFloat("moveZ", -MovementAmount.y);
        }

    }
}
