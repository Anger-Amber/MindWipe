using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public Transform healthFill;
    public Vector3 scale = Vector3.one;
    public float healthPoints = 180;
    public bool isPlayer = false;

    private void Awake()
    {
        if (gameObject.transform.CompareTag("Player"))
        {
            isPlayer = true;
        }
    }

    private void FixedUpdate()
    {
        if (healthPoints >= 0 && isPlayer)
        {
            scale = healthFill.transform.localScale;
            scale.x = healthPoints / 10;
            healthFill.transform.localScale = scale;
        }
        else if (healthPoints == 0)
        {
            if (isPlayer)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else if (healthPoints < 0)
        {
            if (isPlayer)
            {
                healthPoints = 0;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                healthPoints = 0;
                Destroy(gameObject);
            }
        }

    }
}
