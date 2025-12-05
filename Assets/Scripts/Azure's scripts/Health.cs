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
        // Changes the script if the object is a player
        if (gameObject.transform.CompareTag("Player"))
        {
            isPlayer = true;
        }
    }

    private void FixedUpdate()
    {
        // Restart key
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        // Changes the healthbar if the player is alive
        if (healthPoints > 0 && isPlayer)
        {
            scale = healthFill.transform.localScale;
            scale.x = healthPoints / 10;
            healthFill.transform.localScale = scale;
        }
        // Reloads the scene if the player is dead
        else if (healthPoints <= 0 && isPlayer)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}