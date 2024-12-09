using TMPro;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] Targetables targets;
    [SerializeField] Transform[] player;
    [SerializeField] Rigidbody2D myRigidbody2D;
    [SerializeField] Vector3 position;
    [SerializeField] int playerListLenght;
    [SerializeField] float cameraMoveSpeed;

    private void Awake()
    {
        targets = GetComponentInParent<Targetables>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        position = Vector3.zero;
        player = targets.playerTransforms;
        playerListLenght = 0;
        for (int i = 0; i < player.Length; i++)
        {
            if (player[i] != null)
            {
                position += player[i].transform.position;
                playerListLenght++;
            }
        }
        if (playerListLenght > 0)
        {
            position /= playerListLenght;
        }
        else
        {
            position = Vector3.zero;
        }

        myRigidbody2D.linearVelocity = Vector3.zero;
        //myRigidbody2D.AddForce((position - transform.position) * cameraMoveSpeed);
        transform.position = Vector2.Lerp(transform.position, position, cameraMoveSpeed);
    }
}
