using UnityEngine;

public class Movement : MonoBehaviour
{
    //public floats
    public float jumpHeight = 1000.0f;
    public float jumpPower = 0.0f;
    public float jumpTimes = 0.0f;
    public float maxJumpTimes = 1.0f;
    public float maxJumpTimerLimit = 0.5f;
    public float jumpTimer = 0.5f;
    public float maxCoyoteFrames = 0.15f;
    public float coyoteFrames = 0.0f;

    //public bools
    public bool jumping = false;
    public bool dashing = false;
    public bool dashHeld = false;
    public bool jumpTimerOn = false;
    public bool coyoteFramesOn = false;
    public bool airborne = false;

    //bools
    [SerializeField] bool frictionActive = false;
    [SerializeField] bool dashEndActive = false;
    [SerializeField] bool jumpEndActive = false;

    //floats
    [SerializeField] float dashTimer = 0.0f;
    [SerializeField] float moveSpeed = 10.0f;
    [SerializeField] float maxSpeedX = 100.0f;
    [SerializeField] float maxSpeedY = 100.0f;
    [SerializeField] float gravity = 1.0f;
    [SerializeField] float dashPower = 0.2f;
    [SerializeField] float friction = 1.0f;

    //vectors
    [SerializeField] Vector2 maxSpeedXY = Vector2.zero;
    [SerializeField] Vector2 tempSpeedXY = Vector2.zero;

    //components
    [SerializeField] Rigidbody2D myRigidbody2D;
    [SerializeField] Transform myTransform;
    [SerializeField] Animator animator;
    public InventoryScript myInventory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myTransform = GetComponent<Transform>();
        moveSpeed *= 1000;
    }

    // Update is called once per frame
    void Update()
    {
        dashTimer -= Time.deltaTime;
        maxSpeedXY.x = maxSpeedX;
        maxSpeedXY.y = maxSpeedY;

        //CompleteMovementCap
        if (maxSpeedX < myRigidbody2D.linearVelocity.x)
        {
            if (dashing == false)
            {
                tempSpeedXY = myRigidbody2D.linearVelocity;
                tempSpeedXY.x = maxSpeedX;
                myRigidbody2D.linearVelocity = tempSpeedXY;
            }
        }
        if (-maxSpeedX > myRigidbody2D.linearVelocity.x)
        {
            if (dashing == false)
            {
                tempSpeedXY = myRigidbody2D.linearVelocity;
                tempSpeedXY.x = -maxSpeedX;
                myRigidbody2D.linearVelocity = tempSpeedXY;
            }
        }
        if (maxSpeedY < myRigidbody2D.linearVelocity.y)
        {
            tempSpeedXY = myRigidbody2D.linearVelocity;
            tempSpeedXY.y = maxSpeedY;
            myRigidbody2D.linearVelocity = tempSpeedXY;
        }
        if (-maxSpeedY > myRigidbody2D.linearVelocity.y)
        {
            tempSpeedXY = myRigidbody2D.linearVelocity;
            tempSpeedXY.y = -maxSpeedY;
            myRigidbody2D.linearVelocity = tempSpeedXY;
        }
        
        //friction Calculator
        if (myRigidbody2D.linearVelocityX < friction && 
            myRigidbody2D.linearVelocityX > -friction && 
            Input.anyKey == false)
        {
            myRigidbody2D.linearVelocityX = 0;
            frictionActive = false;
        }

        else if (dashing)
        {
            frictionActive = false;
        }

        else if (dashEndActive)
        {
            frictionActive = false;
        }

        else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            frictionActive = true;
        }

        else if (Input.GetKey(KeyCode.A))
        {
            frictionActive = false;
        }

        else if (Input.GetKey(KeyCode.D))
        {
            frictionActive = false;
        }

        else if (Input.GetKey(KeyCode.Space))
        {
            frictionActive = false;
        }

        else
        {
            frictionActive = true;
        }

        //When you are falling you aren't jumping
        if (myRigidbody2D.linearVelocityY <= (jumpTimer * 10) - 10f)
        {
            jumpEndActive = false;
            jumping = false;
        }

        

        //CompleteMovement
        if (Input.GetKey(KeyCode.D) && !dashing)
        {
            CompleteMovementSpeed(moveSpeed);
            myTransform.localScale = Vector3.one;
        }

        if (Input.GetKey(KeyCode.A) && !dashing)
        {
            CompleteMovementSpeed(-moveSpeed);
            myTransform.localScale = new Vector3(-1,1,1);
        }

        if (!Input.GetKey(KeyCode.S) && !jumping)
        {
            myRigidbody2D.gravityScale = gravity;
        }

        else if (!Input.GetKey(KeyCode.S) && jumping)
        {
            myRigidbody2D.gravityScale = gravity / 4;
        }

        else if (Input.GetKey(KeyCode.S))
        {
            myRigidbody2D.gravityScale = gravity * 6;
        }

        //jumping
        if ((Input.GetKeyDown(KeyCode.Space) && !dashing) || (jumpTimer > 0 && !dashing))
        {
            //Debug.Log("jump");
            if (jumpTimes > 0)
            {
                //Debug.Log("jumping with " + coyoteFrames + " coyoteframes");
                jumping = true;
                myRigidbody2D.AddForceY(jumpPower);
                jumpTimes--;
                jumpTimer = 0;
                jumpTimerOn = false;
                coyoteFrames = 0;
                coyoteFramesOn = false;
            }
            else if (jumpTimes <= 0 && jumpTimer <= 0) 
            {
                jumpTimerOn = true;
                jumpTimer = maxJumpTimerLimit;
                coyoteFramesOn = true;
                coyoteFrames = maxCoyoteFrames;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpEndActive = true;
            jumping = false;
        }

        if (jumpTimerOn)
        {
            if (jumpTimer > 0)
            {
                jumpTimer -= Time.deltaTime;
            }
            else
            {
                jumpTimerOn = false;
            }
        }
        if (coyoteFramesOn)
        {
            if (coyoteFrames > 0)
            {
                coyoteFrames -= Time.deltaTime;
            }
            else
            {
                coyoteFramesOn = false;
                jumpTimes = 0;
            }

        }

        //dashing
        if (Input.GetKeyDown(KeyCode.LeftShift) && (dashHeld == true))
        {
            dashing = true;
            dashHeld = false;
            if (myTransform.localScale.x > 0)
            {
                myRigidbody2D.AddRelativeForceX(moveSpeed * dashPower);
            }
            if (myTransform.localScale.x < 0)
            {
                myRigidbody2D.AddRelativeForceX(-moveSpeed * dashPower);
            }
            dashTimer = 0.25f;
        }

        if (dashTimer <= 0 && dashing)
        {
            dashEndActive = true;
        }

        // animations
        animator.SetBool("Jumping", jumping);
        animator.SetBool("Airborn", airborne);
        animator.SetBool("Dashing", dashing);
        animator.SetBool("DashEnd", dashEndActive);
        animator.SetBool("Hit(Anger)", false);
        animator.SetBool("Hit(Sad)", false);
        animator.SetBool("Hit(Happiness)", false);
        if ((myRigidbody2D.linearVelocityX < -10 || myRigidbody2D.linearVelocityX > 10) && !dashing && !airborne && !jumping)
        {
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Running", false);
        }
    }

    private void FixedUpdate()
    {
        //forwarding methods
        if (jumpEndActive == true)
        {
            JumpEnd();
        }

        if (dashEndActive == true)
        {
            DashEnd();
        }

        if (frictionActive == true)
        {
            FrictionUpdate();
        }
    }

    //slowing down the player after jump ends
    void JumpEnd()
    {
        if (true)
        {
            if (myRigidbody2D.linearVelocityY > 0)
            {
                float tempVelocityReducer = 0;
                tempVelocityReducer = myRigidbody2D.linearVelocityY;
                tempVelocityReducer = tempVelocityReducer / 1.2f;
                myRigidbody2D.linearVelocityY = tempVelocityReducer;
            }
            else
            {
                jumpEndActive = false;
            }
        }
    }

    //slowing down the player after dash ends
    void DashEnd()
    {


        if (myRigidbody2D.linearVelocityX > 30)
        {
            float tempVelocityReducer = myRigidbody2D.linearVelocityX;
            tempVelocityReducer = tempVelocityReducer / 1.2f;
            myRigidbody2D.linearVelocityX = tempVelocityReducer;
        }

        else if (myRigidbody2D.linearVelocityX < -30)
        {
            float tempVelocityReducer = myRigidbody2D.linearVelocityX;
            tempVelocityReducer = tempVelocityReducer / 1.2f;
            myRigidbody2D.linearVelocityX = tempVelocityReducer;
        }

        else 
        { 
            
            dashEndActive = false;
            dashing = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("blepp");
        if (collision.transform.CompareTag("Item") == true)
        {
            //Debug.Log("Peck");
            myInventory.AddToInventory(collision.gameObject, collision.gameObject.GetComponent<ItemScript>().size);
        }
    }

    //moving
    void CompleteMovementSpeed(float moveSpeed)
    {
        myRigidbody2D.AddForceX(moveSpeed * Time.deltaTime);
    }

    //friction update
    void FrictionUpdate()
    {
        myRigidbody2D.linearVelocityX *= 0.9f;
    }
}
