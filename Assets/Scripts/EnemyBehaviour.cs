using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] Health healthScript;
    [SerializeField] Targetables parent;
    [SerializeField] bool isMobile = false;
    [SerializeField] float maxHealthPoints;
    [SerializeField] float healthPoints;

    [SerializeField] GameObject[] itemDropped;
    [SerializeField] GameObject specificItemDropped;
    [SerializeField] int speciesID;
    //id 0 = turret

    [SerializeField] int itemID;

    void Awake()
    {
        healthScript = GetComponent<Health>();
        parent = transform.parent.GetComponent<Targetables>();
        healthScript.healthPoints = maxHealthPoints;
        itemDropped = new GameObject[7];
        itemDropped[0] = transform.GetChild(0).gameObject;
        itemDropped[1] = transform.GetChild(1).gameObject;
        itemDropped[2] = transform.GetChild(2).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        healthPoints = healthScript.healthPoints;
        if (healthPoints <= 0)
        {
            int randomNumber = Random.Range(0, parent.itemDropCounter);
            if (randomNumber <= 2)
            {
                itemDropped[randomNumber].gameObject.SetActive(true);
                specificItemDropped = Instantiate(itemDropped[parent.itemDropCounter], transform.position, 
                    Quaternion.Euler(transform.rotation.x, transform.rotation.y,
                    transform.rotation.z + Random.Range(-45,45)));
                specificItemDropped.GetComponent<Rigidbody2D>().AddRelativeForceY(1000);
                
            }
            if (parent.itemDropCounter == 0)
            {
                parent.itemDropCounter = 7;
            }
            else if (parent.itemDropCounter > 0)
            {
                parent.itemDropCounter--;
            }
            Destroy(gameObject);
        }
    }
}