using System.Data;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ArmedadilloScript : MonoBehaviour
{
    BoxCollider2D myBoxCollider2D;
    Rigidbody2D myRigidbody2D;
    ParticleSystem myParticleSystem;
    GameObject dupedParticleSystem;
    TilemapCollider2D groundTilesCollider;
    Tilemap groundTiles;
    [SerializeField] GameObject player;

    [SerializeField] float actionTimer;
    [SerializeField] int chosenAction;
    [SerializeField] Animator biteAnimation;
    void Awake()
    {
        myBoxCollider2D = GetComponent<BoxCollider2D>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myParticleSystem = transform.GetChild(2).GetComponent<ParticleSystem>();
    }

    void Update()
    {
        myRigidbody2D.linearVelocityX = (player.transform.position.x - transform.position.x) * 0.5f;
        myRigidbody2D.linearVelocityX += 10f;
        actionTimer += Time.deltaTime;
        if (actionTimer > 0)
        {
            chosenAction = Random.Range(0, 4);
            switch (chosenAction)
            {
                case 0: // bite

                    break;
                case 1: // lazer
                    break;
                case 2: // saw
                    break;
                case 3: // bait
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DoCollision(collision.gameObject);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        DoCollision(collision.gameObject);
    }
    bool CheckTile(int tileCheckLocationx, int tileCheckLocationy)
    {
        
        return (groundTiles.GetTile(Vector3Int.FloorToInt
                (new Vector3(transform.position.x * 0.5f + myBoxCollider2D.size.x * 0.25f + tileCheckLocationx, tileCheckLocationy, 0))) != null);
    }    
    void DeleteTile(int tileDeleteLocationx, int tileDeleteLocationy)
    {
        groundTiles.SetTile(Vector3Int.FloorToInt
                (new Vector3(transform.position.x * 0.5f + myBoxCollider2D.size.x * 0.25f + tileDeleteLocationx, tileDeleteLocationy, 0)), null);
        dupedParticleSystem = Instantiate(myParticleSystem.gameObject, Vector3Int.FloorToInt
                (new Vector3(transform.position.x * 0.5f + myBoxCollider2D.size.x * 0.25f + tileDeleteLocationx, tileDeleteLocationy, 0)) * 2, Quaternion.identity);   
        dupedParticleSystem.SetActive(true);
    }
    void DoCollision(GameObject collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            groundTilesCollider = collision.GetComponent<TilemapCollider2D>();
            groundTiles = collision.GetComponent<Tilemap>();
            if (CheckTile(0, -1))
            {
                DeleteTile(0, -1);
            }
            if (CheckTile(0, 0))
            {
                DeleteTile(0, 0);
            }
            if (CheckTile(0, 1))
            {
                DeleteTile(0, 1);
            }
            if (CheckTile(-1, -1))
            {
                DeleteTile(-1, -1);
            }
            if (CheckTile(-1, 0))
            {
                DeleteTile(-1, 0);
            }
            if (CheckTile(-1, 1))
            {
                DeleteTile(-1, 1);
            }
        }
    }
}
