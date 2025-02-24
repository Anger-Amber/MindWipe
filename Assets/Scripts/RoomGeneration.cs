using UnityEngine;
using UnityEngine.Tilemaps;


public class RoomGeneration : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] TileBase basicTile;
    [SerializeField] BoundsInt newRoomTemplateLocation;
    [SerializeField] TileBase[] newRoomTiles;
    [SerializeField] Vector3Int offset;
    [SerializeField] Vector3Int tileLocation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tilemap = transform.GetChild(0).GetComponent<Tilemap>();
        newRoomTemplateLocation.xMin = -10;
        newRoomTemplateLocation.xMax = 22;
        newRoomTemplateLocation.yMin = -2;
        newRoomTemplateLocation.yMax = 30;
        GenerateRoom(tilemap, newRoomTemplateLocation);
    }

    void GenerateRoom(Tilemap tileLayer, BoundsInt roomBounds)
    {
        Tilemap roomtiles = tilemap.transform.GetChild(0).GetComponent<Tilemap>();
        for (int i = 0; i < roomBounds.yMax - roomBounds.yMin; i++)
        {
            for (int c = 0; c < roomBounds.xMax - roomBounds.xMin; c++)
            {
                if (roomtiles.GetTile(new Vector3Int(i + roomBounds.xMin, c + roomBounds.yMin, 0)) != null)
                {
                    tileLayer.SetTile(new Vector3Int(i + roomBounds.xMin + offset.x, c + roomBounds.yMin + offset.y, 0),
                        roomtiles.GetTile(new Vector3Int(i + roomBounds.xMin, c + roomBounds.yMin, 0)));
                    Debug.Log("check");
                }
            }
        }
    }
}
