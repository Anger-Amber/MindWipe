using UnityEngine;

public class Health : MonoBehaviour
{
    public Transform healthFill;
    public Vector3 scale = Vector3.one;
    public float healthPoints;
    [SerializeField] bool isPlayer = false;


    private void FixedUpdate()
    {
        if (healthPoints >= 0)
        {
            scale = healthFill.transform.localScale;
            scale.x = healthPoints / 10;
            healthFill.transform.localScale = scale;
        }

        else
        {
            healthPoints = 0;
        }
    }
}
