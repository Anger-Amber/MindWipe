using UnityEngine;

public class CompleteMovement : MonoBehaviour
{
    [Header("Standard movement")]

    [SerializeField] float stopSpeed;

    public float movementSpeed;
    public float movementSpeedCap = 4;

    Rigidbody2D myRigidbody;

    [Header("Jumping")]

    [SerializeField] float jumpForce;

    public bool onGround;

    float jumpTime;

    bool jumpStart;

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
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Regain dashes
        if (onGround && numberOfDashes == 0)
        {
            numberOfDashes = maxDashes;
        }

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

            switch (myRigidbody.linearVelocityX)
            {
                case < 0:
                    myRigidbody.linearVelocityX += stopSpeed * Time.deltaTime;
                    break;

                case > 0:
                    myRigidbody.linearVelocityX -= stopSpeed * Time.deltaTime;
                    break;
            }
        }

        // Start jumping
        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            jumpStart = true;
            onGround = false;
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

        // loop jump if still holding jump
        if (onGround && Input.GetKey(KeyCode.Space))
        {
            onGround = false;
            if (myRigidbody.linearVelocityY < 0)
            {
                myRigidbody.linearVelocityY = 0;
            }
            myRigidbody.linearVelocityY += jumpForce;

            jumpTime = 0;
        }

        LookDirection = GetLookDirection(lookingRight);
        if (!dashing && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && numberOfDashes > 0 )
        {
            dashing = true;
            onGround = false;
            numberOfDashes -= 1;
            switch (LookDirection)
            {
                case Vector2 v when v.Equals(Vector2.right):
                    if (myRigidbody.linearVelocityX < 0)
                    {
                        myRigidbody.linearVelocityX = 0;
                    }

                    myRigidbody.linearVelocityX += dashStrength;
                    break;

                case Vector2 v when v.Equals(Vector2.left):
                    if (myRigidbody.linearVelocityX > 0)
                    {
                        myRigidbody.linearVelocityX = 0;
                    }

                    myRigidbody.linearVelocityX -= dashStrength;
                    break;

                case Vector2 v when v.Equals(Vector2.up):
                    if (myRigidbody.linearVelocityY < 0)
                    {
                        myRigidbody.linearVelocityY = 0;
                    }

                    myRigidbody.linearVelocityY += dashStrength;
                    break;

                case Vector2 v when v.Equals(Vector2.down):
                    if (myRigidbody.linearVelocityY > 0)
                    {
                        myRigidbody.linearVelocityY = 0;
                    }

                    myRigidbody.linearVelocityY -= dashStrength;
                    break;

                case Vector2 v when v.Equals(new Vector2(1, -1)):
                    if (myRigidbody.linearVelocityX < 0)
                    {
                        myRigidbody.linearVelocityX = 0;
                    }

                    if (myRigidbody.linearVelocityY > 0)
                    {
                        myRigidbody.linearVelocityY = 0;
                    }

                    myRigidbody.linearVelocityX += dashStrength * Mathf.Sqrt(2) / 2;
                    myRigidbody.linearVelocityY -= dashStrength * Mathf.Sqrt(2) / 2;
                    break;

                case Vector2 v when v.Equals(Vector2.one):
                    if (myRigidbody.linearVelocityX < 0)
                    {
                        myRigidbody.linearVelocityX = 0;
                    }

                    if (myRigidbody.linearVelocityY < 0)
                    {
                        myRigidbody.linearVelocityY = 0;
                    }

                    myRigidbody.linearVelocityX += dashStrength * Mathf.Sqrt(2) / 2;
                    myRigidbody.linearVelocityY += dashStrength * Mathf.Sqrt(2) / 2;
                    break;

                case Vector2 v when v.Equals(new Vector2(-1, -1)):
                    if (myRigidbody.linearVelocityX > 0)
                    {
                        myRigidbody.linearVelocityX = 0;
                    }

                    if (myRigidbody.linearVelocityY > 0)
                    {
                        myRigidbody.linearVelocityY = 0;
                    }

                    myRigidbody.linearVelocityX -= dashStrength * Mathf.Sqrt(2) / 2;
                    myRigidbody.linearVelocityY -= dashStrength * Mathf.Sqrt(2) / 2;
                    break;

                case Vector2 v when v.Equals(new Vector2(-1, 1)):

                    if (myRigidbody.linearVelocityX > 0)
                    {
                        myRigidbody.linearVelocityX = 0;
                    }

                    if (myRigidbody.linearVelocityY < 0)
                    {
                        myRigidbody.linearVelocityY = 0;
                    }

                    myRigidbody.linearVelocityX -= dashStrength * Mathf.Sqrt(2) / 2;
                    myRigidbody.linearVelocityY += dashStrength * Mathf.Sqrt(2) / 2;
                    break;
            }
        }

        if (dashing)
        {
            dashTime += Time.deltaTime;
        }

        if (dashTime >= dashEnd)
        {
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

            if (Mathf.Abs(myRigidbody.linearVelocityY) <= 1 && Mathf.Abs(myRigidbody.linearVelocityX) <= 1)
            {
                dashTime = 0;
                dashing = false;
            }
        }
    }
    Vector2 GetLookDirection(bool right)
    {
        Vector2 lookDirection = Vector2.zero;
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
        if (Input.GetKey(KeyCode.A))
        {
            lookDirection.x -= 1;
            if (!onGround)
            {
                lookDirection.x -= 1;
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            lookDirection.x += 1;
            if (!onGround)
            {
                lookDirection.x += 1;
            }
        }
        if (Input.GetKey(KeyCode.W))
        {
            lookDirection.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            lookDirection.y -= 1;
        }
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
}
