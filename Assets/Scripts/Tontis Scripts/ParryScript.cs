using UnityEngine;

public class ParryScript : MonoBehaviour
{
    [SerializeField] CompleteMovement parent;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Untagged"))
        {
            parent.ParryBoost();
            Debug.Log(other.transform.name);
            Destroy(other.gameObject);
        }
    }
}
