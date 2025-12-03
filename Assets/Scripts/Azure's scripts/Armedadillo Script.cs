using System.Data;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ArmedadilloScript : MonoBehaviour
{
    BoxCollider2D myBoxCollider2D;
    ParticleSystem myParticleSystem;
    GameObject dupedParticleSystem;
    TilemapCollider2D groundTilesCollider;
    Tilemap groundTiles;
    [SerializeField] GameObject player;
    void Awake()
    {
        myBoxCollider2D = GetComponent<BoxCollider2D>();
        myParticleSystem = transform.GetChild(2).GetComponent<ParticleSystem>();
    }

    void Update()
    {
        
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
