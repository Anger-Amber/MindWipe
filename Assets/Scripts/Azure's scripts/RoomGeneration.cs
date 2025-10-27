using UnityEngine;
using UnityEngine.Tilemaps;


public class RoomGeneration : MonoBehaviour
{
    Targetables targets;
    public TileBase basicTile;
    public Tilemap roomLayout;
    public Tilemap basicTilemap;
    public GameObject roomTrigger;
    public GameObject newRoomTrigger;
    public BoundsInt newRoomTemplateLocation;
    public Vector3Int offset;
    [SerializeField] bool enemiesAlive;
    [SerializeField] Tilemap roomTiles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        roomTiles = transform.GetChild(4).GetChild(Random.Range(0, transform.GetChild(4).transform.childCount)).GetComponent<Tilemap>();
        targets = GameObject.FindGameObjectWithTag("Targetables").GetComponent<Targetables>();
        basicTilemap = transform.GetChild(0).GetComponent<Tilemap>();
        roomLayout = transform.GetChild(5).GetComponent<Tilemap>();
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
    }
    public void CheckRooms(Transform collision)
    {
        Vector3Int triggerLocation = Vector3Int.FloorToInt(new Vector3((collision.transform.position.x + 10) / 32, (collision.transform.position.y + 2) / 22, 0));
        if (roomTiles.GetComponent<RoomSize>().roomSize.y == 2)
        {
            if (roomLayout.GetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y)) == null &&
                ((roomLayout.GetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y + 1)) == null) ||
                roomLayout.GetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y - 1)) == null))
            {
                if (roomLayout.GetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y + 1)) == null)
                {
                    offset = new Vector3Int(triggerLocation.x * 16 - 16, triggerLocation.y * 22);
                    Debug.Log(offset);
                    GenerateRoom(basicTilemap, newRoomTemplateLocation);
                    roomLayout.SetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y + 1, triggerLocation.z), basicTile);
                    roomLayout.SetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y, triggerLocation.z), basicTile);
                }
                else if (roomLayout.GetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y - 1)) == null)
                {
                    offset = new Vector3Int(triggerLocation.x * 16 - 16, triggerLocation.y * 22 - 22);
                    Debug.Log(offset);
                    GenerateRoom(basicTilemap, newRoomTemplateLocation);
                    roomLayout.SetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y, triggerLocation.z), basicTile);
                    roomLayout.SetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y - 1, triggerLocation.z), basicTile);
                }
            }
            else
            {
                Debug.Log("Beep boop I am a robot and I eat batteries, instead of normal people foods");
                Debug.Log(collision.transform.name);
            }

            if (roomLayout.GetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y)) == null &&
                ((roomLayout.GetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y + 1)) == null) ||
                roomLayout.GetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y - 1)) == null))
            {
                if (roomLayout.GetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y + 1)) == null)
                {
                    offset = new Vector3Int(triggerLocation.x * 16 + 16, triggerLocation.y * 22);
                    Debug.Log(triggerLocation);
                    GenerateRoom(basicTilemap, newRoomTemplateLocation);
                    roomLayout.SetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y + 1, triggerLocation.z), basicTile);
                    roomLayout.SetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y, triggerLocation.z), basicTile);
                }
                if (roomLayout.GetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y - 1)) == null)
                {
                    offset = new Vector3Int(triggerLocation.x * 16 + 16, triggerLocation.y * 22 - 22);
                    Debug.Log(offset);
                    GenerateRoom(basicTilemap, newRoomTemplateLocation);
                    roomLayout.SetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y, triggerLocation.z), basicTile);
                    roomLayout.SetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y - 1, triggerLocation.z), basicTile);
                }
            }
            else
            {
                Debug.Log("Beep boop I am a robot and I eat batteries, instead of normal people foods");
                Debug.Log(roomLayout.GetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y)) == null);
                Debug.Log(triggerLocation);
            }
            roomLayout.SetTile(triggerLocation, basicTile);
        }
    }
    public void GenerateRoom(Tilemap tileLayer, BoundsInt roomBounds)
    {
        roomTrigger = transform.GetChild(4).GetChild(0).GetChild(0).gameObject;
        newRoomTrigger = Instantiate(roomTrigger);
        newRoomTrigger.GetComponent<RoomTriggerScript>().parentsRoomGenScript = transform.GetComponent<RoomGeneration>();
        newRoomTrigger.transform.position = offset * 2 + roomTrigger.transform.position;
        newRoomTrigger.transform.SetParent(transform.GetChild(3));
        for (int i = 0; i < roomBounds.yMax - roomBounds.yMin; i++)
        {
            for (int c = 0; c < roomBounds.xMax - roomBounds.xMin; c++)
            {
                if (roomTiles.GetTile(new Vector3Int(i + roomBounds.xMin, c + roomBounds.yMin, 0)) != null)
                {
                    tileLayer.SetTile(new Vector3Int(i + roomBounds.xMin + offset.x, c + roomBounds.yMin + offset.y, 0),
                        roomTiles.GetTile(new Vector3Int(i + roomBounds.xMin, c + roomBounds.yMin, 0)));
                }
            }
        }
    }
}