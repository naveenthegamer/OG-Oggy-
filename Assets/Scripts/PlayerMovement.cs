using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveInput;
    private float HorizontalMovement;

    [Header("Movement")]
    [SerializeField] Rigidbody2D rb;
    //[SerializeField] float speed;

    [Header("Acceleration")]
    //[SerializeField] float MaxSpeed;
    [SerializeField] float baseMaxSpeed;//8
    [SerializeField] float boostedMaxSpeed;//12
    [SerializeField] float acceleration;//20f
    [SerializeField] float deceleration; //30;

    [Header("Boost Settings")]
    [SerializeField] float boostTime;// = 2f; // time in seconds to trigger boost
    private float timeInSameDirection = 0f;
    private int lastDirection = 0; // -1 = left, 1 = right, 0 = none
    private float currentMaxSpeed;
    private float currentSpeedX;
    private float accelRate;
    int dir;
    float targetSpeedX;



    void Start()
    {
        currentMaxSpeed = baseMaxSpeed;
    }


    void Update()
    {
        Debug.Log($"current x speed is {currentSpeedX}");
    }

    private void FixedUpdate()
    {
        PhysicsMove(HorizontalMovement);
    }

    public void PhysicsMove(float horDire)
    {
        // Track direction (-1, 0, 1)
            dir = horDire > 0 ? 1 : horDire < 0 ? -1 : 0;

        if (dir != 0)
        {
            if (dir == lastDirection)
            {
                timeInSameDirection += Time.fixedDeltaTime;
                if (timeInSameDirection >= boostTime)
                    currentMaxSpeed = boostedMaxSpeed;
            }
            else
            {
                // Direction changed → reset
                timeInSameDirection = 0f;
                currentMaxSpeed = baseMaxSpeed;
            }
        }
        else
        {
            // No input → reset
            timeInSameDirection = 0f;
            currentMaxSpeed = baseMaxSpeed;
        }

        lastDirection = dir;

        // Target speed
        targetSpeedX = horDire * currentMaxSpeed;

        // Accel/decel
        accelRate = (Mathf.Abs(targetSpeedX) > 0.01f) ? acceleration : deceleration;
        currentSpeedX = Mathf.MoveTowards(rb.linearVelocityX, targetSpeedX, accelRate * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector2(currentSpeedX, rb.linearVelocity.y);
    }




//-----------------------------------------------------------------------------------------------------------------------------


    public void GetHorizontalDirection(InputAction.CallbackContext context)
    {
        HorizontalMovement = context.ReadValue<Vector2>().x;        
    }

    public void AxisDirection(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        Debug.Log("jumpingtime");
        //charge jump
    }





    public void aircontrol()
    {

    }
    //coyote time
    //jump input buffering
    //current vector
    //charge boots shouldnt change velocity they should apply force OMG!
    //rb.velocity to 0 when shooting in opposite direction of movement
    //crouch

}
