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
    [SerializeField] Vector2Int newRoomSize;
    bool enemiesAlive;
    bool repeatRoomCheck;
    [SerializeField] bool isIntroduction;
    [SerializeField] bool bossDead;
    [SerializeField] GameObject lastRoom;
    [SerializeField] GameObject emptyRoom;
    [SerializeField] GameObject solidRoom;
    [SerializeField] Tilemap roomTiles;

    void Awake()
    {
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
        

        // condensed code
        roomTiles = transform.GetChild(4).GetChild(Random.Range(0, transform.GetChild(4).transform.childCount)).GetComponent<Tilemap>();
        newRoomSize = roomTiles.GetComponent<RoomSize>().roomSize;
        Vector3Int triggerLocation = Vector3Int.FloorToInt(new Vector3((collision.transform.position.x + 36) / 32, (collision.transform.position.y + 15) / 22, 0));
        // checking if we can place a room reaching into the top left corner
        if (roomLayout.GetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y, 0)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y + newRoomSize.y - 1, 0)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x - newRoomSize.x, triggerLocation.y, 0)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x - newRoomSize.x, triggerLocation.y + newRoomSize.y - 1, 0)) == null)
        {
            if (roomTiles.GetComponent<RoomSize>().isShop && triggerLocation.y % 5 == 0)
            {
                offset = new Vector3Int((triggerLocation.x - newRoomSize.x) * 16, triggerLocation.y * 11);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y + newRoomSize.y - 1, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - newRoomSize.x, triggerLocation.y + newRoomSize.y - 1, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - newRoomSize.x, triggerLocation.y, 0), basicTile);
                GenerateRoom(basicTilemap, newRoomTemplateLocation, roomTiles);
            }
            else if (!roomTiles.GetComponent<RoomSize>().isShop)
            {
                offset = new Vector3Int((triggerLocation.x - newRoomSize.x) * 16, triggerLocation.y * 11);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y + newRoomSize.y - 1, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - newRoomSize.x, triggerLocation.y + newRoomSize.y - 1, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - newRoomSize.x, triggerLocation.y, 0), basicTile);
                GenerateRoom(basicTilemap, newRoomTemplateLocation, roomTiles);
            }
            else
            {
                repeatRoomCheck = true;
            }
        }
        else if (roomLayout.GetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y, 0)) == null)
        {
            repeatRoomCheck = true;
        }

        // checking if we can place a room reaching into the bottom left corner
        if (roomLayout.GetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y, 0)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y - newRoomSize.y + 1, 0)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x - newRoomSize.x, triggerLocation.y, 0)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x - newRoomSize.x, triggerLocation.y - newRoomSize.y + 1, 0)) == null)
        {
            if (roomTiles.GetComponent<RoomSize>().isShop && (triggerLocation.y - newRoomSize.y + 1) % 5 == 0)
            {
                offset = new Vector3Int((triggerLocation.x - newRoomSize.x) * 16, triggerLocation.y * 11 - 11);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y - newRoomSize.y + 1, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - newRoomSize.x, triggerLocation.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - newRoomSize.x, triggerLocation.y - newRoomSize.y + 1, 0), basicTile);
                GenerateRoom(basicTilemap, newRoomTemplateLocation, roomTiles);
            }
            else if (!roomTiles.GetComponent<RoomSize>().isShop)
            {
                offset = new Vector3Int((triggerLocation.x - newRoomSize.x) * 16, triggerLocation.y * 11 - 11);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y - newRoomSize.y + 1, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - newRoomSize.x, triggerLocation.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - newRoomSize.x, triggerLocation.y - newRoomSize.y + 1, 0), basicTile);
                GenerateRoom(basicTilemap, newRoomTemplateLocation, roomTiles);
            }
            else
            {
                repeatRoomCheck = true;
            }
        }
        else if (roomLayout.GetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y, 0)) == null)
        {
            repeatRoomCheck = true;
        }

        // checking if we can place a room reaching into the top right corner
        if (roomLayout.GetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y + newRoomSize.y - 1)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x + newRoomSize.x, triggerLocation.y)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x + newRoomSize.x, triggerLocation.y + newRoomSize.y - 1)) == null)
        {
            if (roomTiles.GetComponent<RoomSize>().isShop && (triggerLocation.y + newRoomSize.y - 1) % 5 == 0)
            {
                offset = new Vector3Int(triggerLocation.x * 16 + 16, triggerLocation.y * 11);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y + newRoomSize.y - 1, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + newRoomSize.x, triggerLocation.y + newRoomSize.y - 1, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + newRoomSize.x, triggerLocation.y, 0), basicTile);
                GenerateRoom(basicTilemap, newRoomTemplateLocation, roomTiles);
            }
            else if (!roomTiles.GetComponent<RoomSize>().isShop)
            {
                offset = new Vector3Int(triggerLocation.x * 16 + 16, triggerLocation.y * 11);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y + newRoomSize.y - 1, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + newRoomSize.x, triggerLocation.y + newRoomSize.y - 1, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + newRoomSize.x, triggerLocation.y, 0), basicTile);
                GenerateRoom(basicTilemap, newRoomTemplateLocation, roomTiles);
            }
            else
            {
                repeatRoomCheck = true;
            }
        }
        else if (roomLayout.GetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y)) == null)
        {
            repeatRoomCheck = true;
        }

        // checking if we can place a room reaching into the bottom right corner
        if (roomLayout.GetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y - newRoomSize.y + 1)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x + newRoomSize.x, triggerLocation.y)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x + newRoomSize.x, triggerLocation.y - newRoomSize.y + 1)) == null)
        {
            if (roomTiles.GetComponent<RoomSize>().isShop && (triggerLocation.y - newRoomSize.y + 1) % 5 == 0)
            {
                offset = new Vector3Int(triggerLocation.x * 16 + 16, triggerLocation.y * 11 - (11 * (newRoomSize.y - 1)));
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y - newRoomSize.y + 1, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + newRoomSize.x, triggerLocation.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + newRoomSize.x, triggerLocation.y - newRoomSize.y + 1, 0), basicTile);
                GenerateRoom(basicTilemap, newRoomTemplateLocation, roomTiles);
            }
            else if (!roomTiles.GetComponent<RoomSize>().isShop)
            {
                offset = new Vector3Int(triggerLocation.x * 16 + 16, triggerLocation.y * 11 - (11 * (newRoomSize.y - 1)));
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y - newRoomSize.y + 1, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + newRoomSize.x, triggerLocation.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + newRoomSize.x, triggerLocation.y - newRoomSize.y + 1, 0), basicTile);
                GenerateRoom(basicTilemap, newRoomTemplateLocation, roomTiles);
            }
            else
            {
                repeatRoomCheck = true;
            }
        }
        else if (roomLayout.GetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y)) == null)
        {
            repeatRoomCheck = true;
        }

        // checking if we can spawn a room on top reaching into the left
        if (roomLayout.GetTile(new Vector3Int(triggerLocation.x, triggerLocation.y + 1)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x, triggerLocation.y + newRoomSize.y)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x - newRoomSize.x + 1, triggerLocation.y + 1)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x - newRoomSize.x + 1, triggerLocation.y + newRoomSize.y)) == null &&
                !isIntroduction)
        {
            if (roomTiles.GetComponent<RoomSize>().isShop && (triggerLocation.y + newRoomSize.y - 1) % 5 == 0)
            {
                offset = new Vector3Int((triggerLocation.x - newRoomSize.x + 1) * 16, triggerLocation.y * 11 + 11);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x, triggerLocation.y + newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x, triggerLocation.y + 1, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - newRoomSize.x + 1, triggerLocation.y + newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - newRoomSize.x + 1, triggerLocation.y + 1, 0), basicTile);
                GenerateRoom(basicTilemap, newRoomTemplateLocation, roomTiles);
            }
            else if (!roomTiles.GetComponent<RoomSize>().isShop)
            {
                offset = new Vector3Int((triggerLocation.x - newRoomSize.x + 1) * 16, triggerLocation.y * 11 + 11);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x, triggerLocation.y + newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x, triggerLocation.y + 1, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - newRoomSize.x + 1, triggerLocation.y + newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - newRoomSize.x + 1, triggerLocation.y + 1, 0), basicTile);
                GenerateRoom(basicTilemap, newRoomTemplateLocation, roomTiles);
            }
            else
            {
                repeatRoomCheck = true;
            }
        }
        else if (roomLayout.GetTile(new Vector3Int(triggerLocation.x, triggerLocation.y + 1)) == null && !isIntroduction)
        {
            repeatRoomCheck = true;
        }

        // checking if we can spawn a room on top reaching into the right
        if (roomLayout.GetTile(new Vector3Int(triggerLocation.x, triggerLocation.y + 1)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x, triggerLocation.y + newRoomSize.y)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x + newRoomSize.x - 1, triggerLocation.y + 1, 0)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x + newRoomSize.x - 1, triggerLocation.y + newRoomSize.y, 0)) == null &&
                !isIntroduction)
        {
            if (roomTiles.GetComponent<RoomSize>().isShop && (triggerLocation.y + newRoomSize.y - 1) % 5 == 0)
            {
                offset = new Vector3Int((triggerLocation.x) * 16, triggerLocation.y * 11 + 11);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x, triggerLocation.y + newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x, triggerLocation.y + 1, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + newRoomSize.x - 1, triggerLocation.y + newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + newRoomSize.x - 1, triggerLocation.y + 1, 0), basicTile);
                GenerateRoom(basicTilemap, newRoomTemplateLocation, roomTiles);
            }
            else if (!roomTiles.GetComponent<RoomSize>().isShop)
            {
                offset = new Vector3Int((triggerLocation.x) * 16, triggerLocation.y * 11 + 11);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x, triggerLocation.y + newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x, triggerLocation.y + 1, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + newRoomSize.x - 1, triggerLocation.y + newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + newRoomSize.x - 1, triggerLocation.y + 1, 0), basicTile);
                GenerateRoom(basicTilemap, newRoomTemplateLocation, roomTiles);
            }
            else
            {
                repeatRoomCheck = true;
            }
        }
        else if (roomLayout.GetTile(new Vector3Int(triggerLocation.x, triggerLocation.y + 1)) == null && !isIntroduction)
        {
            repeatRoomCheck = true;
        }

        // checking if we can spawn a room at the bottom reaching into the right
        if (roomLayout.GetTile(new Vector3Int(triggerLocation.x, triggerLocation.y - 1, 0)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x, triggerLocation.y - newRoomSize.y, 0)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x + newRoomSize.x - 1, triggerLocation.y - 1, 0)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x + newRoomSize.x - 1, triggerLocation.y - newRoomSize.y, 0)) == null &&
                !isIntroduction)
        {
            if (roomTiles.GetComponent<RoomSize>().isShop && (triggerLocation.y - newRoomSize.y) % 5 == 0)
            {
                offset = new Vector3Int((triggerLocation.x) * 16, (triggerLocation.y - newRoomSize.y) * 11);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x, triggerLocation.y - newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x, triggerLocation.y - 1, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + newRoomSize.x - 1, triggerLocation.y - newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + newRoomSize.x - 1, triggerLocation.y - 1, 0), basicTile);
                GenerateRoom(basicTilemap, newRoomTemplateLocation, roomTiles);
            }
            else if (!roomTiles.GetComponent<RoomSize>().isShop)
            {
                offset = new Vector3Int((triggerLocation.x) * 16, (triggerLocation.y - newRoomSize.y) * 11);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x, triggerLocation.y - newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x, triggerLocation.y - 1, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + newRoomSize.x - 1, triggerLocation.y - newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + newRoomSize.x - 1, triggerLocation.y - 1, 0), basicTile);
                GenerateRoom(basicTilemap, newRoomTemplateLocation, roomTiles);
            }
            else
            {
                repeatRoomCheck = true;
            }
        }
        else if (roomLayout.GetTile(new Vector3Int(triggerLocation.x, triggerLocation.y - 1)) == null && !isIntroduction)
        {
            repeatRoomCheck = true;
        }

        // checking if we can spawn a room at the bottom reaching into the left
        if (roomLayout.GetTile(new Vector3Int(triggerLocation.x, triggerLocation.y - 1)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x, triggerLocation.y - newRoomSize.y)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x - newRoomSize.x + 1, triggerLocation.y - 1)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x - newRoomSize.x + 1, triggerLocation.y - newRoomSize.y)) == null &&
                !isIntroduction)
        {
            if (roomTiles.GetComponent<RoomSize>().isShop && (triggerLocation.y - newRoomSize.y) % 5 == 0)
            {
                offset = new Vector3Int((triggerLocation.x - newRoomSize.x + 1) * 16, (triggerLocation.y - newRoomSize.y) * 11);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x, triggerLocation.y - newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x, triggerLocation.y - 1, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - newRoomSize.x + 1, triggerLocation.y - newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - newRoomSize.x + 1, triggerLocation.y - 1, 0), basicTile);
                GenerateRoom(basicTilemap, newRoomTemplateLocation, roomTiles);
            }
            else if (!roomTiles.GetComponent<RoomSize>().isShop)
            {
                offset = new Vector3Int((triggerLocation.x - newRoomSize.x + 1) * 16, (triggerLocation.y - newRoomSize.y) * 11);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x, triggerLocation.y - newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x, triggerLocation.y - 1, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - newRoomSize.x + 1, triggerLocation.y - newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - newRoomSize.x + 1, triggerLocation.y - 1, 0), basicTile);
                GenerateRoom(basicTilemap, newRoomTemplateLocation, roomTiles);
            }
            else
            {
                repeatRoomCheck = true;
            }
        }
        else if (roomLayout.GetTile(new Vector3Int(triggerLocation.x, triggerLocation.y - 1)) == null && !isIntroduction)
        {
            repeatRoomCheck = true;
        }

        // checking the top right corner 
        if (roomLayout.GetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y + 1)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y + newRoomSize.y)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x + newRoomSize.x, triggerLocation.y + 1, 0)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x + newRoomSize.x, triggerLocation.y + newRoomSize.y, 0)) == null &&
                !isIntroduction)
        {
            if (roomTiles.GetComponent<RoomSize>().isShop && (triggerLocation.y + newRoomSize.y) % 5 == 0)
            {
                offset = new Vector3Int((triggerLocation.x + 1) * 16, triggerLocation.y * 11 + 11);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y + newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y + 1, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + newRoomSize.x, triggerLocation.y + newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + newRoomSize.x, triggerLocation.y + 1, 0), basicTile);
                GenerateRoom(basicTilemap, newRoomTemplateLocation, roomTiles);
            }
            else if (!roomTiles.GetComponent<RoomSize>().isShop)
            {
                offset = new Vector3Int((triggerLocation.x + 1) * 16, triggerLocation.y * 11 + 11);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y + newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y + 1, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + newRoomSize.x, triggerLocation.y + newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + newRoomSize.x, triggerLocation.y + 1, 0), basicTile);
                GenerateRoom(basicTilemap, newRoomTemplateLocation, roomTiles);
            }
            else
            {
                repeatRoomCheck = true;
            }
        }
        else if (roomLayout.GetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y + 1)) == null && !isIntroduction)
        {
            repeatRoomCheck = true;
        }

        // checking the top left corner 
        if (roomLayout.GetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y + 1)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y + newRoomSize.y)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x - newRoomSize.x, triggerLocation.y + 1, 0)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x - newRoomSize.x, triggerLocation.y + newRoomSize.y, 0)) == null &&
                !isIntroduction)
        {
            if (roomTiles.GetComponent<RoomSize>().isShop && (triggerLocation.y + newRoomSize.y) % 5 == 0)
            {
                offset = new Vector3Int((triggerLocation.x - newRoomSize.x) * 16, triggerLocation.y * 11 + 11);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y + newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y + 1, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - newRoomSize.x, triggerLocation.y + newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - newRoomSize.x, triggerLocation.y + 1, 0), basicTile);
                GenerateRoom(basicTilemap, newRoomTemplateLocation, roomTiles);
            }
            else if (!roomTiles.GetComponent<RoomSize>().isShop)
            {
                offset = new Vector3Int((triggerLocation.x - newRoomSize.x) * 16, triggerLocation.y * 11 + 11);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y + newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y + 1, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - newRoomSize.x, triggerLocation.y + newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - newRoomSize.x, triggerLocation.y + 1, 0), basicTile);
                GenerateRoom(basicTilemap, newRoomTemplateLocation, roomTiles);
            }
            else
            {
                repeatRoomCheck = true;
            }
        }
        else if (roomLayout.GetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y + 1)) == null && !isIntroduction)
        {
            repeatRoomCheck = true;
        }

        // checking the bottom right corner 
        if (roomLayout.GetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y - 1)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y - newRoomSize.y)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x + newRoomSize.x, triggerLocation.y - 1, 0)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x + newRoomSize.x, triggerLocation.y - newRoomSize.y, 0)) == null &&
                !isIntroduction)
        {
            if (roomTiles.GetComponent<RoomSize>().isShop && (triggerLocation.y - newRoomSize.y) % 5 == 0)
            {
                offset = new Vector3Int((triggerLocation.x + 1) * 16, (triggerLocation.y - newRoomSize.y) * 11);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y - newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y - 1, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + newRoomSize.x, triggerLocation.y - newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + newRoomSize.x, triggerLocation.y - 1, 0), basicTile);
                GenerateRoom(basicTilemap, newRoomTemplateLocation, roomTiles);
            }
            else if (!roomTiles.GetComponent<RoomSize>().isShop)
            {
                offset = new Vector3Int((triggerLocation.x + 1) * 16, (triggerLocation.y - newRoomSize.y) * 11);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y - newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y - 1, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + newRoomSize.x, triggerLocation.y - newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x + newRoomSize.x, triggerLocation.y - 1, 0), basicTile);
                GenerateRoom(basicTilemap, newRoomTemplateLocation, roomTiles);
            }
            else
            {
                repeatRoomCheck = true;
            }
        }
        else if (roomLayout.GetTile(new Vector3Int(triggerLocation.x + 1, triggerLocation.y - 1)) == null && !isIntroduction)
        {
            repeatRoomCheck = true;
        }

        // checking the bottom left corner 
        if (roomLayout.GetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y - 1)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y - newRoomSize.y)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x - newRoomSize.x, triggerLocation.y - 1, 0)) == null &&
                roomLayout.GetTile(new Vector3Int(triggerLocation.x - newRoomSize.x, triggerLocation.y - newRoomSize.y, 0)) == null &&
                !isIntroduction)
        {
            if (roomTiles.GetComponent<RoomSize>().isShop && (triggerLocation.y - newRoomSize.y) % 5 == 0)
            {
                offset = new Vector3Int((triggerLocation.x - newRoomSize.x) * 16, (triggerLocation.y - newRoomSize.y) * 11);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y - newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y - 1, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - newRoomSize.x, triggerLocation.y - newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - newRoomSize.x, triggerLocation.y - 1, 0), basicTile);
                GenerateRoom(basicTilemap, newRoomTemplateLocation, roomTiles);
            }
            else if (!roomTiles.GetComponent<RoomSize>().isShop)
            {
                offset = new Vector3Int((triggerLocation.x - newRoomSize.x) * 16, (triggerLocation.y - newRoomSize.y) * 11);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y - newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y - 1, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - newRoomSize.x, triggerLocation.y - newRoomSize.y, 0), basicTile);
                roomLayout.SetTile(new Vector3Int(triggerLocation.x - newRoomSize.x, triggerLocation.y - 1, 0), basicTile);
                GenerateRoom(basicTilemap, newRoomTemplateLocation, roomTiles);
            }
            else
            {
                repeatRoomCheck = true;
            }
        }
        else if (roomLayout.GetTile(new Vector3Int(triggerLocation.x - 1, triggerLocation.y - 1)) == null && !isIntroduction)
        {
            repeatRoomCheck = true;
        }

        if (repeatRoomCheck)
        {
            repeatRoomCheck = false;
            CheckRooms(collision);
        }

        roomLayout.SetTile(triggerLocation, basicTile);
    }
    public void GenerateRoom(Tilemap tileLayer, BoundsInt roomBounds, Tilemap roomLayout)
    {
        for (int i = 0; i < roomTiles.transform.childCount; i++)
        {
            roomTrigger = roomLayout.gameObject.transform.GetChild(i).gameObject;
            newRoomTrigger = Instantiate(roomTrigger);
            newRoomTrigger.GetComponent<RoomTriggerScript>().parentsRoomGenScript = transform.GetComponent<RoomGeneration>();
            newRoomTrigger.transform.position = offset * 2 + roomTrigger.transform.position;
            newRoomTrigger.transform.SetParent(transform.GetChild(3));
        }
        for (int c = 0; c < roomBounds.yMax - roomBounds.yMin; c++)
        {
            for (int i = 0; i < roomBounds.xMax - roomBounds.xMin; i++)
            {
                if (roomTiles.GetTile(new Vector3Int(i + roomBounds.xMin, c + roomBounds.yMin, 0)) != null)
                {
                    tileLayer.SetTile(new Vector3Int(i + roomBounds.xMin + offset.x, c + roomBounds.yMin + offset.y, 0),
                        roomTiles.GetTile(new Vector3Int(i + roomBounds.xMin, c + roomBounds.yMin, 0)));
                }
            }
        }
        if (!bossDead)
        {
            roomTiles = transform.GetChild(4).GetChild(Random.Range(0, transform.GetChild(4).transform.childCount)).GetComponent<Tilemap>();
        }
        else
        {
            roomTiles = lastRoom.GetComponent<Tilemap>();
            lastRoom = emptyRoom;
        }
        newRoomSize = roomTiles.GetComponent<RoomSize>().roomSize;
    }
}