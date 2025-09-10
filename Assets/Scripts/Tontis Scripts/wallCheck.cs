using UnityEngine;

public class wallCheck : MonoBehaviour
{
    CompleteMovement movement;
    [SerializeField] bool rightCollider;

    void Start()
    {
        movement = GetComponentInParent<CompleteMovement>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ground") && rightCollider)
        {
            movement.onWallR = true;
        }
        if (other.CompareTag("Ground") && !rightCollider)
        {
            movement.onWallL = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground") && rightCollider)
        {
            movement.onWallR = false;
        }
        if (other.CompareTag("Ground") && !rightCollider)
        {
            movement.onWallL = false;
        }
    }
}
