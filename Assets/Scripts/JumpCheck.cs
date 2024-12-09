using JetBrains.Annotations;
using UnityEngine;

public class JumpCheck : MonoBehaviour
{
    [SerializeField] BoxCollider2D myBoxCollider2D;
    [SerializeField] Movement parentsMovement;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myBoxCollider2D = GetComponent<BoxCollider2D>();
        parentsMovement = GetComponentInParent<Movement>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        parentsMovement.jumpPower = parentsMovement.jumpHeight;
        parentsMovement.jumpTimes = parentsMovement.maxJumpTimes;
        if (parentsMovement.dashHeld == false && parentsMovement.dashing == false)
        {
            parentsMovement.dashHeld = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (parentsMovement.jumping == false)
        {
            //Debug.Log("left ground");
            parentsMovement.coyoteFramesOn = true;
            parentsMovement.coyoteFrames = parentsMovement.maxCoyoteFrames;
        }
    }
}
