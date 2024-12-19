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
    [SerializeField] float hitLagTimer;
    [SerializeField] float hitLagTimerMax;

    private void Awake()
    {
        targets = GetComponentInParent<Targetables>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Time.timeScale < 1)
        {
            if (hitLagTimer > hitLagTimerMax)
            {
                Time.timeScale = 1;
                hitLagTimer = 0;
            }
            else
            {
                hitLagTimer += Time.deltaTime / Time.timeScale;
            }
        }
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
