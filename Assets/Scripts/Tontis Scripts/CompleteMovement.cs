using UnityEngine;

public class CompleteMovement : MonoBehaviour
{
    [Header("Collision")]
    
    Rigidbody2D myRigidbody;

    [SerializeField] ContactFilter2D[] contactFilters;

    [Header("Standard movement")]

    [SerializeField] float stopSpeed = 80;

    public float movementSpeed;
    public float movementSpeedCap = 4;


    [Header("Jumping")]

    [SerializeField] float jumpForce;

    public bool onGround;

    float jumpTime;

    bool jumpStart;

    [Header("Wall jump")]

    [SerializeField] float jumpForceWallVerti;
    [SerializeField] float jumpForceWallHori;

    public bool onWallR;
    public bool onWallL;


    [Header("Dash")]

    [SerializeField] float dashStrength = 200;
    [SerializeField] float dashStopSpeed;
    [SerializeField] float dashEnd;
    [SerializeField] Vector2 LookDirection;
    [SerializeField] bool lookingRight = true;

    public int numberOfDashes;
    public int maxDashes;

    public bool dashing;

    float dashTime;

    void Awake()
    {
        for (int i = 0; i < 3; i++)
        {
            contactFilters[i].useNormalAngle = true;
            contactFilters[i].minNormalAngle = 90 * i - 45;
            contactFilters[i].maxNormalAngle = 90 * i + 45;
        }
        
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        DoCollision();

        Dash();

        StandardMovement();

        WallJump();

        Jumping();
    }
    Vector2 GetLookDirection(bool right)
    {
        // reset
        Vector2 lookDirection = Vector2.zero;

        // Default depending on last pressed a/d key
        if (right)
        {
            if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)))
            {
                lookDirection.x += 1;
            }
        }
        else
        {
            if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)))
            {
                lookDirection.x -= 1;
            }
        }
        // left
        if (Input.GetKey(KeyCode.A))
        {
            lookDirection.x -= 1;
            // can't turn around in air so make stronger to cancel the facing direction
            if (!onGround)
            {
                lookDirection.x -= 1;
            }
        }
        // right
        if (Input.GetKey(KeyCode.D))
        {
            lookDirection.x += 1;
            // can't turn around in air so make stronger to cancel the facing direction
            if (!onGround)
            {
                lookDirection.x += 1;
            }
        }
        // up
        if (Input.GetKey(KeyCode.W))
        {
            lookDirection.y += 1;
        }
        // down
        if (Input.GetKey(KeyCode.S))
        {
            lookDirection.y -= 1;
        }
        // Keep max value at 1 or -1
        if (lookDirection.x > 1)
        {
            lookDirection.x = 1;
        }
        if (lookDirection.x < -1)
        {
            lookDirection.x = -1;
        }
        return lookDirection;
    }

    void DoCollision()
    {
        onGround = myRigidbody.IsTouching(contactFilters[1]);
        onWallL = myRigidbody.IsTouching(contactFilters[0]);
        onWallR = myRigidbody.IsTouching(contactFilters[2]);
    }
    void StandardMovement()
    {
        // Move left
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && myRigidbody.linearVelocityX >= -movementSpeedCap)
        {
            // turn around if not in air
            if (onGround)
            {
                lookingRight = false;
            }

            myRigidbody.linearVelocityX -= movementSpeed * Time.deltaTime;
        }

        // Move right
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) && myRigidbody.linearVelocityX <= movementSpeedCap)
        {
            // turn around if not in air
            if (onGround)
            {
                lookingRight = true;
            }

            myRigidbody.linearVelocityX += movementSpeed * Time.deltaTime;
        }

        // Deceleration if not going left or right
        if ((!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)))
        {

            if (myRigidbody.linearVelocityX < -stopSpeed * Time.deltaTime)
            {
                myRigidbody.linearVelocityX += stopSpeed * Time.deltaTime;
            }
            else if (myRigidbody.linearVelocityX > stopSpeed * Time.deltaTime)
            {
                myRigidbody.linearVelocityX -= stopSpeed * Time.deltaTime;
            }
            else
            {
                myRigidbody.linearVelocityX = 0;
            }
            
        }
    }
    void Jumping()
    {
        // Start jumping
        if (Input.GetKey(KeyCode.Space) && onGround && !jumpStart)
        {
            jumpStart = true;
            onGround = false;

            // If on a platform moving down the jump doesn't become shortened
            if (myRigidbody.linearVelocityY < 0)
            {
                myRigidbody.linearVelocityY = 0;
            }

            myRigidbody.linearVelocityY += jumpForce;
        }

        // Timing the jump
        if (jumpStart)
        {
            jumpTime += Time.deltaTime;
        }

        // Decrease jump depending on when space is released
        if (Input.GetKeyUp(KeyCode.Space) && jumpStart)
        {
            jumpStart = false;

            if (0.63f - jumpTime * 1.33f > 0)
            {
                myRigidbody.linearVelocityY -= jumpForce * (0.63f - jumpTime * 1.33f);
            }

            jumpTime = 0;
        }

        // Not jumping anymore if moving downwards
        if (myRigidbody.linearVelocityY < 0)
        {
            jumpStart = false;
            jumpTime = 0;
        }
    }
    void WallJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !onGround)
        {
            if (onWallR) 
            {
                myRigidbody.linearVelocityY = jumpForceWallVerti;
                myRigidbody.linearVelocityX = -jumpForceWallHori;
            }

            if (onWallL)
            {
                myRigidbody.linearVelocityY = jumpForceWallVerti;
                myRigidbody.linearVelocityX = jumpForceWallHori;
            }
        }
    }
    void Dash()
    {
        // Regain dashes
        if (onGround && numberOfDashes == 0)
        {
            numberOfDashes = maxDashes;
        }

        // Find what keys are being pressed
        LookDirection = GetLookDirection(lookingRight);
        // Dash
        if (!dashing && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && numberOfDashes > 0)
        {
            dashing = true;
            onGround = false;
            numberOfDashes -= 1;

            //cancel current movement to prepare for dash
            myRigidbody.linearVelocityX = 0;
            myRigidbody.linearVelocityY = 0;

            // Change dash direction based on what keys are being pressed
            switch (LookDirection)
            {
                // right
                case Vector2 v when v.Equals(Vector2.right):

                    myRigidbody.linearVelocityX += dashStrength;
                    break;

                //left
                case Vector2 v when v.Equals(Vector2.left):

                    myRigidbody.linearVelocityX -= dashStrength;
                    break;

                // up
                case Vector2 v when v.Equals(Vector2.up):

                    myRigidbody.linearVelocityY += dashStrength;
                    break;

                //down
                case Vector2 v when v.Equals(Vector2.down):

                    myRigidbody.linearVelocityY -= dashStrength;
                    break;

                //right and down
                case Vector2 v when v.Equals(new Vector2(1, -1)):

                    // trig for circle
                    myRigidbody.linearVelocityX += dashStrength * Mathf.Sqrt(2) / 2;
                    myRigidbody.linearVelocityY -= dashStrength * Mathf.Sqrt(2) / 2;
                    break;

                // right and up
                case Vector2 v when v.Equals(Vector2.one):

                    // trig for circle
                    myRigidbody.linearVelocityX += dashStrength * Mathf.Sqrt(2) / 2;
                    myRigidbody.linearVelocityY += dashStrength * Mathf.Sqrt(2) / 2;
                    break;

                //left and down
                case Vector2 v when v.Equals(new Vector2(-1, -1)):

                    // trig for circle
                    myRigidbody.linearVelocityX -= dashStrength * Mathf.Sqrt(2) / 2;
                    myRigidbody.linearVelocityY -= dashStrength * Mathf.Sqrt(2) / 2;
                    break;

                //left and up
                case Vector2 v when v.Equals(new Vector2(-1, 1)):

                    // trig for circle
                    myRigidbody.linearVelocityX -= dashStrength * Mathf.Sqrt(2) / 2;
                    myRigidbody.linearVelocityY += dashStrength * Mathf.Sqrt(2) / 2;
                    break;
            }
        }

        if (dashing)
        {
            dashTime += Time.deltaTime;
        }

        // dash timer
        if (dashTime >= dashEnd)
        {
            // decelerate depending on direction moving
            switch (myRigidbody.linearVelocityX)
            {
                case < 0:
                    myRigidbody.linearVelocityX += dashStopSpeed * Time.deltaTime;
                    break;
                case > 0:
                    myRigidbody.linearVelocityX -= dashStopSpeed * Time.deltaTime;
                    break;
            }

            switch (myRigidbody.linearVelocityY)
            {
                case < 0:
                    myRigidbody.linearVelocityY += dashStopSpeed * Time.deltaTime;
                    break;
                case > 0:
                    myRigidbody.linearVelocityY -= dashStopSpeed * Time.deltaTime;
                    break;
            }

            // End deceleration when slow
            if (Mathf.Abs(myRigidbody.linearVelocityY) <= 0.2 && Mathf.Abs(myRigidbody.linearVelocityX) <= 0.2)
            {
                dashTime = 0;
                dashing = false;
            }

            // Smoothness when trying to move after dash
            if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && Mathf.Abs(myRigidbody.linearVelocityY) <= 0.2 && Mathf.Abs(myRigidbody.linearVelocityX) <= movementSpeedCap)
            {
                dashTime = 0;
                dashing = false;
            }
        }
    }
}