using UnityEngine;
using UnityEngine.Tilemaps;


public class RoomGeneration : MonoBehaviour
{
    [SerializeField] Targetables targets;
    [SerializeField] TileBase basicTile;
    [SerializeField] Tilemap roomLayout;
    [SerializeField] GameObject roomTrigger;
    [SerializeField] GameObject newRoomTrigger;
    [SerializeField] BoundsInt newRoomTemplateLocation;
    [SerializeField] Vector3Int offset;
    [SerializeField] Vector3Int tileLocation;
    [SerializeField] bool enemiesAlive;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targets = GameObject.FindGameObjectWithTag("Targetables").GetComponent<Targetables>();
    }

    private void FixedUpdate()
    {
        enemiesAlive = false;
        for (int i = 0; i < targets.compressedShooterList.Length; i++)
        {
            if (targets.compressedShooterList[i] != null)
            {
                enemiesAlive = true;
            }
        }
        targets.transform.GetChild(0).GetComponent<CameraScript>().isHooked = enemiesAlive;
        transform.GetChild(3).GetComponent<TilemapCollider2D>().enabled = enemiesAlive;
        transform.GetChild(3).GetComponent<TilemapRenderer>().enabled = enemiesAlive;
        if (enemiesAlive)
        {
            GenerateRoom(transform.GetChild(0).GetComponent<Tilemap>(), newRoomTemplateLocation);
        }
    }

    public void GenerateRoom(Tilemap tileLayer, BoundsInt roomBounds)
    {
        Tilemap roomtiles = transform.GetChild(4).GetChild(Random.Range(0, transform.GetChild(4).transform.childCount)).GetComponent<Tilemap>();
        roomTrigger = transform.GetChild(4).GetChild(0).GetChild(0).gameObject;
        newRoomTrigger = Instantiate(roomTrigger);
        newRoomTrigger.transform.position = offset*2 + roomTrigger.transform.position;
        newRoomTrigger.transform.SetParent(transform.GetChild(3));
        for (int i = 0; i < roomBounds.yMax - roomBounds.yMin; i++)
        {
            for (int c = 0; c < roomBounds.xMax - roomBounds.xMin; c++)
            {
                if (roomtiles.GetTile(new Vector3Int(i + roomBounds.xMin, c + roomBounds.yMin, 0)) != null)
                {
                    tileLayer.SetTile(new Vector3Int(i + roomBounds.xMin + offset.x, c + roomBounds.yMin + offset.y, 0),
                        roomtiles.GetTile(new Vector3Int(i + roomBounds.xMin, c + roomBounds.yMin, 0)));
                }
            }
        }
    }
}