using UnityEngine;

public class ClickingScript : MonoBehaviour
{
    [SerializeField] BoxCollider2D boxCollider2D;
    [SerializeField] GameObject parent;
    [SerializeField] GameObject pickedUpItem;

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
        while (pickedUpItem != null)
        {
            pickedUpItem.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
    private void OnMouseOver()
    {
        Debug.Log("Peck");
    }
    private void OnMouseDown()
    {
        Debug.Log("Peck");
        if (transform.childCount > 0)
        {
            pickedUpItem = transform.GetChild(0).gameObject;
        }
    }
}
