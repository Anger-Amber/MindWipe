using TMPro;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    [SerializeField] GameObject inventoryInterface;
    [SerializeField] GameObject player;
    [SerializeField] Transform slotCategory;
    [SerializeField] Transform[] largeInventorySlots;
    [SerializeField] Transform[] smallInventorySlots;
    [SerializeField] TMPro.TextMeshProUGUI scrapUI;
    [SerializeField] TMPro.TextMeshProUGUI DMGUI;
    public int scrap;
    [SerializeField] int amountOfLargeInventorySlots;
    [SerializeField] int amountOfSmallInventorySlots;
    [SerializeField] float time;
    [SerializeField] float damageMultiplier = 1f;
    [SerializeField] bool inventory;
    public GameObject pickedUpItem;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player.transform.parent.tag == "Player")
        {
            player = player.transform.parent.gameObject;
        }
        
        if (inventory == true)
        {
            inventoryInterface = transform.GetChild(0).gameObject;
            player.GetComponent<Movement>().myInventory = gameObject.GetComponent<InventoryScript>();
            slotCategory = inventoryInterface.transform.GetChild(0);
            largeInventorySlots = new Transform[amountOfLargeInventorySlots];
            smallInventorySlots = new Transform[amountOfSmallInventorySlots];
            for (int i = 0; i < slotCategory.childCount; i++)
            {
                if (i < amountOfLargeInventorySlots)
                {
                    largeInventorySlots[i] = slotCategory.GetChild(i);
                }
                else
                {
                    smallInventorySlots[i - amountOfLargeInventorySlots] = slotCategory.GetChild(i);
                }
            }
        }

    }
    private void Update()
    {
        if (inventory)
        {
            time += Time.deltaTime;
            if (time > 1)
            {
                time--;
                InventoryCheck();
            }
        }
        if (inventory == true)
        {

            scrapUI = inventoryInterface.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
            DMGUI = inventoryInterface.transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>();
        }
        if (pickedUpItem != null)
        {
            pickedUpItem.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pickedUpItem.transform.position = new Vector3(pickedUpItem.transform.position.x, pickedUpItem.transform.position.y, -11f);
        }
    }
    private void OnMouseDown()
    {
        if (inventoryInterface != null)
        {
            inventoryInterface.SetActive(!inventoryInterface.activeSelf);
        }
        else
        {
            Application.Quit();
        }
    }

    public void AddToInventory(GameObject addedItem, bool size)
    {
        if (addedItem.GetComponent<ItemScript>().isScrap == false)
        {
            for (int i = 0; i < smallInventorySlots.Length; i++)
            {
                if (smallInventorySlots[i].transform.childCount == 0 && size == false)
                {
                    GameObject addedItemClone = new
                        (
                        addedItem.name,
                        typeof(SpriteRenderer),
                        typeof(ItemScript)
                        );
                    addedItemClone.GetComponent<Transform>().position = smallInventorySlots[i].position;
                    addedItemClone.GetComponent<Transform>().parent = smallInventorySlots[i];
                    addedItemClone.GetComponent<SpriteRenderer>().sprite = addedItem.GetComponent<SpriteRenderer>().sprite;
                    addedItemClone.GetComponent<SpriteRenderer>().sortingOrder = 30000;
                    addedItemClone.GetComponent<ItemScript>().id = addedItem.GetComponent<ItemScript>().id;
                    addedItemClone.GetComponent<ItemScript>().size = addedItem.GetComponent<ItemScript>().size;
                    Destroy(addedItem);
                    break;
                }
            }
            for (int i = 0; i < largeInventorySlots.Length; i++)
            {
                if (largeInventorySlots[i].transform.childCount == 0 && size)
                {
                    GameObject addedItemClone = new
                        (
                        addedItem.name,
                        typeof(SpriteRenderer),
                        typeof(ItemScript)
                        );
                    addedItemClone.GetComponent<Transform>().position = largeInventorySlots[i].position;
                    addedItemClone.GetComponent<Transform>().parent = largeInventorySlots[i];
                    addedItemClone.GetComponent<SpriteRenderer>().sprite = addedItem.GetComponent<SpriteRenderer>().sprite;
                    addedItemClone.GetComponent<SpriteRenderer>().sortingOrder = 30000;
                    addedItemClone.GetComponent<ItemScript>().id = addedItem.GetComponent<ItemScript>().id;
                    addedItemClone.GetComponent<ItemScript>().size = addedItem.GetComponent<ItemScript>().size;
                    Destroy(addedItem);
                    break;
                }
            }
        }
        else
        {
            scrap += addedItem.GetComponent<ItemScript>().id;
            scrapUI.text = scrap.ToString();
            Destroy(addedItem);
        }
    }

    void InventoryCheck()
    {
        damageMultiplier = 1f;
        for (int i = 0; i < slotCategory.childCount; i++)
        {
            if (i < largeInventorySlots.Length)
            {
                if (largeInventorySlots[i].childCount == 1)
                {
                    int itemID;
                    itemID = largeInventorySlots[i].GetChild(0).GetComponent<ItemScript>().id;
                    switch (itemID)
                    {
                        case 0:
                            damageMultiplier *= 2;
                            break;
                    }
                }
            }
            else
            {
                if (smallInventorySlots[i - largeInventorySlots.Length].childCount == 1)
                {
                    int itemID;
                    itemID = smallInventorySlots[i - largeInventorySlots.Length].GetChild(0).GetComponent<ItemScript>().id;
                    switch (itemID)
                    {
                        case 0:
                            damageMultiplier *= 2;
                            break;
                    }
                }
            }
        }
        player.transform.GetChild(2).GetComponent<ProjectileShooter>().damageMultiplier = damageMultiplier;
        DMGUI.text = damageMultiplier.ToString();
    }
}