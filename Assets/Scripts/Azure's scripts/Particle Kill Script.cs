using UnityEngine;

public class ParticleKillScript : MonoBehaviour
{
    void Update()
    {
        if (transform.GetComponent<ParticleSystem>().IsAlive() == false)
        {
            Destroy(gameObject);
        }
    }
}
