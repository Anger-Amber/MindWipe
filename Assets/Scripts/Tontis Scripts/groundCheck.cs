using UnityEngine;

public class groundCheck : MonoBehaviour
{
    CompleteMovement movement;
    [SerializeField] float coyoteFrames = 0.1f;
    bool leftGround;
    float time;

    void Start()
    {
        movement = GetComponentInParent<CompleteMovement>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            movement.onGround = true;
            leftGround = false;
            time = 0;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (movement.dashing)
        {
            if (other.CompareTag("Ground"))
            {
                movement.onGround = true;
                leftGround = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            leftGround = true;
            time = 0;
        }
    }
    void Update()
    {
        if (leftGround)
        {
            time += Time.deltaTime;
        }

        if (time >= coyoteFrames)
        {
            movement.onGround = false;
            leftGround = false;
        }
    }
}
