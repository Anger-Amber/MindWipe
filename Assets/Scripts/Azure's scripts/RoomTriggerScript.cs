using UnityEngine;

public class RoomTriggerScript : MonoBehaviour
{
    public RoomGeneration parentsRoomGenScript;
    public GameObject[] enemies;
    public GameObject newEnemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        parentsRoomGenScript = transform.parent.parent.GetComponent<RoomGeneration>();
        parentsRoomGenScript.CheckRooms(transform);
        enemies = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            enemies[i] = transform.GetChild(i).gameObject;
            enemies[i].SetActive(true);
            newEnemy = Instantiate(enemies[i]);
            enemies[i].SetActive(false);
            newEnemy.transform.position += transform.position;
            newEnemy.transform.parent = GameObject.FindGameObjectsWithTag("Targetables")[0].transform;
            newEnemy.GetComponent<EnemyBehaviour>().parent = newEnemy.transform.parent.GetComponent<Targetables>();
            newEnemy.transform.GetComponent<EnemyBehaviour>().RunAwake();
            newEnemy.transform.GetComponent<EnemyBehaviour>().enabled = true;
            Destroy(gameObject.transform.GetChild(i).gameObject);
        }
    }
}
