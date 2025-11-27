using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public bool size;
    // true If Big False If Small
    public bool isScrap;
    // false If It isn't Scrap Item
    public int id;
    // item ID 0 = damage multiplier * 1.1
    // item ID 1 = damage multiplier * 1.2
    // item ID 2 = damage multiplier * 1.4
    // item ID 3 = damage multiplier * 2
    // item ID 4 = damage reduction + 1%
    // item ID 4 = damage reduction + 1.5%
    // item ID 4 = damage reduction + 2%
    // item ID 4 = damage reduction + 4%
}
