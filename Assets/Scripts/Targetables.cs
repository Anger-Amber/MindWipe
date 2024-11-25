using UnityEngine;

public class Targetables : MonoBehaviour
{
    public Transform[] compressedTargetList;
    public Transform[] compressedShooterList;
    [SerializeField] string[] species =
    {
        
    };
    [SerializeField] int uncompressedListLength;

    void FixedUpdate()
    {
        uncompressedListLength = transform.childCount;
        compressedTargetList = new Transform[uncompressedListLength];
        compressedShooterList = new Transform[uncompressedListLength];
        for (int i = 0; i < uncompressedListLength; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.CompareTag("Player") || child.CompareTag("Hunter"))
            {
                compressedTargetList[i] = child.GetComponent<Transform>();
            }
            if (child.CompareTag("Security"))
            {
                compressedShooterList[i] = child.GetComponent<Transform>();
            }
        }
    }
}