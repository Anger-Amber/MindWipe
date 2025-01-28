using Unity.VisualScripting;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    [SerializeField] GameObject inventoryInterface;

    private void Start()
    {
        inventoryInterface = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Debug.Log("blep");

        if (inventoryInterface != null)
        {
            inventoryInterface.SetActive(!inventoryInterface.activeSelf);
        }
        else
        {
            Application.Quit();
        }
    }
}
