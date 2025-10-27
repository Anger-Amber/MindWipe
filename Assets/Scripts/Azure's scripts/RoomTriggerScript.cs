using UnityEngine;

public class RoomTriggerScript : MonoBehaviour
{
    public RoomGeneration parentsRoomGenScript;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        parentsRoomGenScript = transform.parent.parent.GetComponent<RoomGeneration>();
        parentsRoomGenScript.CheckRooms(transform);
    }
}
