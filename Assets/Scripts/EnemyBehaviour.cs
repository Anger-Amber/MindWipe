using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] bool isMobile = false;
    [SerializeField] float healthPoints;
    [SerializeField] Health healthScript;

    void Awake()
    {
        healthScript = GetComponent<Health>();
        healthScript.healthPoints = healthPoints;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
