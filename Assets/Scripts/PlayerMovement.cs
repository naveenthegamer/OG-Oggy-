using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveInput;
    private float HorizontalMovement;

    public GameObject colorchange;
    SpriteRenderer guyColor;

    [Header("Movement")]
    [SerializeField] Rigidbody2D rb;
    private bool canMove;
    //[SerializeField] float speed;
    [SerializeField] float test;
    private bool isFacingRight = true; 

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
    private float currentSpeedX=0;
    private float accelRate;
    int dir;
    float targetSpeedX;

    [Header("Jump Settings")]
    [SerializeField] float jumpForce;// 10f;
    [SerializeField] float chargeForceIncrement;//5f; // extra force per charge level\
    [SerializeField] float chargeInterval;// = 2f;
    [SerializeField] int maxChargeLevel;// = 3;        // max time to reach full charge
    public int chargeLevel = 0;
    private bool canChargeJump;
    Vector2 jumpDir;

    private float chargeTimer = 0f;
    
    private bool isCharging = false;
    private float totalForce;
    private int newLevel;

    [Header("Gravity")]
    public float baseGravity;
    public float maxFallSpeed;
    public float fallSpeedMultiplier;

    [Header("Ground Check")]
    public Vector2 boxSize;
    public float groundCheckSize;
    public LayerMask groundLayer;
    public bool grounded;

    [Header("Air Control")]
    [SerializeField] float airControlMultiplier;
    private float moveSpeed;




    void Start()
    {
        currentMaxSpeed = baseMaxSpeed;
    }


    void Update()
    {
        //Debug.Log($"current x speed is {currentSpeedX}");
        Gravity();
        isGrounded();
        FlagChecks();
        flip();

    }

    private void FixedUpdate()  
    {
      PhysicsMove(HorizontalMovement);
    }

    void LateUpdate()
    {
        if (isCharging)
        {
            chargeTimer += Time.deltaTime;

            // Update charge level based on whole intervals
            newLevel = Mathf.FloorToInt(chargeTimer / chargeInterval);
            chargeLevel = Mathf.Clamp(newLevel, 0, maxChargeLevel);
        }
    }

    private void FlagChecks()
    {
        if (chargeLevel > 0)
        {
            currentSpeedX = 0;
            canMove = false;
            canChargeJump = true;

        }
        if (!isCharging)
        {
            chargeLevel = 0;
        }
        if (chargeLevel == 0)
        {
            canMove = true;
            //canChargeJump = false;
        }

        if (chargeLevel == 0)
        {
            guyColor = colorchange.GetComponent<SpriteRenderer>();
            guyColor.color = Color.white;
        }
        else if (chargeLevel==1)
        {
            guyColor = colorchange.GetComponent<SpriteRenderer>();
            guyColor.color = Color.yellow;
        }
        else if (chargeLevel == 2)
        {
            guyColor = colorchange.GetComponent<SpriteRenderer>();
            guyColor.color = Color.orange;
        }
        if (chargeLevel == 3)
        {
            guyColor = colorchange.GetComponent<SpriteRenderer>();
            guyColor.color = Color.red;
        }
    }
    public void PhysicsMove(float horDire)
    {
        // Track direction (-1, 0, 1)
        //dir = horDire > 0 ? 1 : horDire < 0 ? -1 : 0;
        dir = (int)Mathf.Sign(horDire);


        if (dir != 0 && grounded)
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

        moveSpeed = grounded ? currentMaxSpeed : currentMaxSpeed * airControlMultiplier;

        // Target speed
        targetSpeedX = horDire * moveSpeed;

        // Accel/decel
        accelRate = (Mathf.Abs(targetSpeedX) > 0.01f) ? acceleration : deceleration;
        currentSpeedX = Mathf.MoveTowards(currentSpeedX, targetSpeedX, accelRate * Time.fixedDeltaTime);
        if (canMove)
        {
            rb.linearVelocityX = currentSpeedX;
        }
    }

    public void Gravity()
    {
        if (rb.linearVelocityY < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.linearVelocityY = Mathf.Max(rb.linearVelocityY, -maxFallSpeed);
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    public void isGrounded()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, groundCheckSize, groundLayer))
        {
            
            grounded = true;
        }

        else
        {
            grounded = false;
        }
    }

    public void ChargeJump()
    {
        if (chargeLevel == 0 && grounded)
        {
            // Normal jump
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            Debug.Log("Normal jump performed");
        }
        else if (chargeLevel > 0 &&
                 (Vector2.Dot(moveInput.normalized, Vector2.right) > 0.9f ||
                  Vector2.Dot(moveInput.normalized, Vector2.left) > 0.9f))
        {
            // Horizontal charged jump (left or right)
            totalForce = jumpForce + (chargeLevel * chargeForceIncrement);

            // Pick direction
            Vector2 jumpDir = Vector2.zero;
            if (Vector2.Dot(lhs: moveInput.normalized, Vector2.right) > 0.9f)
            {
                jumpDir = new Vector2(test, 1f);
                Debug.Log(message: "right");
            }
            else if (Vector2.Dot(moveInput.normalized, Vector2.left) > 0.9f)
            {
                jumpDir = new Vector2(-test, 1f); // left
                Debug.Log("right");
            }

            rb.AddForce(jumpDir * totalForce, ForceMode2D.Impulse);
            Debug.Log($"Horizontal charged jump performed at level {chargeLevel}, force {totalForce}, direction {jumpDir}");
        }
        else if (chargeLevel > 0)
        {
            // Vertical charged jump
            totalForce = jumpForce + (chargeLevel * chargeForceIncrement);
            rb.AddForce(Vector2.up * totalForce, ForceMode2D.Impulse);
            Debug.Log($"Charged jump performed at level {chargeLevel}, force {totalForce}");
        }
    }

    public void flip()
    {
        if (isFacingRight && HorizontalMovement < 0 || !isFacingRight && HorizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector2 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = grounded ? Color.green : Color.red;

        // Draw the BoxCast area
        Vector3 castOrigin = transform.position;
        Vector3 castDirection = -transform.up * groundCheckSize;

        // Draw the box at the cast origin
        Gizmos.DrawWireCube(castOrigin + castDirection, boxSize);

    }




    //-----------------------------------------------------------------------------------------------------------------------------

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // Start charging
            isCharging = true;
            chargeTimer = 0f;
            chargeLevel = 0;


        }
        else if (context.canceled)
        {
            // Release jump
            isCharging = false;

            ChargeJump();

        }

    }
    public void GetHorizontalDirection(InputAction.CallbackContext context)
    {
        HorizontalMovement = context.ReadValue<Vector2>().x;        
    }

    public void AxisDirection(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void toggleMask(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // Get the current tag
            string currentTag = gameObject.tag;

            // Toggle between "Player" and "Robot"
            if (currentTag == "Player")
            {
                gameObject.tag = "Robot";
            }
            else if (currentTag == "Robot")
            {
                gameObject.tag = "Player";
            }
        }
    }



    //coyote time
    //jump input buffering
    //current vector
    //rb.velocity to 0 when shooting in opposite direction of movement
    //crouch

}
