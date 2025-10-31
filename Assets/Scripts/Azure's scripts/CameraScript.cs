using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] Targetables targets;
    [SerializeField] Transform[] player;
    [SerializeField] Rigidbody2D myRigidbody2D;
    [SerializeField] Vector3 position;
    [SerializeField] Vector2 aliveFieldMax;
    [SerializeField] Vector2 aliveFieldMin;
    [SerializeField] int playerListLength;
    [SerializeField] float cameraMoveSpeed;
    [SerializeField] float hitLagTimer;
    [SerializeField] float hitLagTimerMax;
    public bool isHooked;

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
        playerListLength = 0;
        for (int i = 0; i < player.Length; i++)
        {
            if (player[i] != null)
            {
                position += player[i].transform.position;
                playerListLength++;
            }
        }
        if (playerListLength > 0)
        {
            position /= playerListLength;
        }
        else
        {
            position = Vector3.zero;
        }

        myRigidbody2D.linearVelocity = Vector3.zero;


        transform.position = Vector2.Lerp(transform.position, position, cameraMoveSpeed);
    }
}
