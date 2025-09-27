using UnityEngine;

public class ClickingScript : MonoBehaviour
{
    [SerializeField] BoxCollider2D boxCollider2D;
    [SerializeField] GameObject parent;
    [SerializeField] GameObject tempParent;
    [SerializeField] GameObject pickedUpItem;
    [SerializeField] GameObject tempPickedUpItem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (parent == null)
        {
            parent = transform.parent.parent.parent.gameObject;
        }
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {

    }
    private void OnMouseOver()
    {
        
    }
    private void OnMouseDown()
    {
        if (transform.childCount > 0 && (parent.GetComponent<InventoryScript>().pickedUpItem != transform.GetChild(0).gameObject))
        {
            if (parent.GetComponent<InventoryScript>().pickedUpItem != null)
            {
                tempPickedUpItem = transform.GetChild(0).gameObject;
                pickedUpItem = parent.GetComponent<InventoryScript>().pickedUpItem;
                tempParent = pickedUpItem.transform.parent.gameObject;
                pickedUpItem.transform.parent = transform;
                pickedUpItem.transform.localPosition = Vector3.zero;
                parent.GetComponent<InventoryScript>().pickedUpItem = tempPickedUpItem;
                tempPickedUpItem.transform.parent = tempParent.transform;
            }
            else
            {
                parent.GetComponent<InventoryScript>().pickedUpItem = transform.GetChild(0).gameObject;
            }
        }
        else if ((transform.childCount == 0 && parent.GetComponent<InventoryScript>().pickedUpItem != null) || (transform.childCount == 1 && parent.GetComponent<InventoryScript>().pickedUpItem == transform.GetChild(0).gameObject))
        {
            pickedUpItem = parent.GetComponent<InventoryScript>().pickedUpItem;
            pickedUpItem.transform.parent = transform;
            pickedUpItem.transform.localPosition = Vector3.zero;
            parent.GetComponent<InventoryScript>().pickedUpItem = null;
        }
    }
}