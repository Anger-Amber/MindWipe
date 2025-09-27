using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] Health healthScript;
    [SerializeField] Targetables parent;
    [SerializeField] bool isMobile = false;
    [SerializeField] bool isUnkillable;
    [SerializeField] bool isInteractable;
    [SerializeField] bool isShop;
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
        if (!isShop)
        {
            itemDropped = new GameObject[transform.childCount + 3];
            itemDropped[0] = transform.GetChild(0).gameObject;
            itemDropped[1] = transform.GetChild(1).gameObject;
            itemDropped[2] = transform.GetChild(2).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If the gameobject is dead, roll on a table of items to drop
        // The chance of a better item increases for each bad item dropped
        healthPoints = healthScript.healthPoints;
        if (healthPoints <= 0 && !isUnkillable)
        {
            int randomNumber = Random.Range(0, parent.itemDropCounter);
            if (randomNumber <= transform.childCount - 1)
            {
                itemDropped[randomNumber].gameObject.SetActive(true);
                specificItemDropped = Instantiate(itemDropped[randomNumber], transform.position, 
                    Quaternion.Euler(transform.rotation.x, transform.rotation.y,
                    transform.rotation.z + Random.Range(-45,45)));
                // Adds a boost to the item dropped for comedic effect
                specificItemDropped.GetComponent<Rigidbody2D>().AddRelativeForceY(1000);
                itemDropped[randomNumber].gameObject.SetActive(false);
            }
            // Resets the pity counter if the gameobject dropped an item
            if (randomNumber == 0)
            {
                parent.itemDropCounter = 7;
            }
            // Increases the pity counter if not
            else if (parent.itemDropCounter > 0)
            {
                parent.itemDropCounter--;
            }
            Debug.Log(randomNumber);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isInteractable && collision.CompareTag("Player"))
        {
            if (isShop)
            {

            }
        }
        else
        {
            Debug.Log(collision.transform.name);
        }
    }
}