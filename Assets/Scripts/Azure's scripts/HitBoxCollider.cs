using UnityEngine;

public class HitBoxCollider : MonoBehaviour
{
    [SerializeField] Health myHealthbar;
    [SerializeField] Health parentsHealthBar;

    private void Awake()
    {
        DefineHealthBars();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Bullet>() != null)
        {
            myHealthbar.healthPoints -= collision.gameObject.GetComponent<Bullet>().damage;
            Time.timeScale = 0.1f;
        }
        else if (collision.gameObject.GetComponent<ZoneDamage>() != null)
        {
            myHealthbar.healthPoints -= collision.gameObject.GetComponent<ZoneDamage>().damage;
            Time.timeScale = 0.1f;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Bullet>() != null)
        {
            myHealthbar.healthPoints -= collision.gameObject.GetComponent<Bullet>().damage;
            Time.timeScale = 0.1f;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<ZoneDamage>() != null)
        {
            if (collision.gameObject.GetComponent<ZoneDamage>().immuneObjects[0] != gameObject)
            {
                myHealthbar.healthPoints -= collision.gameObject.GetComponent<ZoneDamage>().damage;
                collision.gameObject.GetComponent<ZoneDamage>().immuneObjects[0] = gameObject;
                Time.timeScale = 0.1f;
            }
        }
    }

    void DefineHealthBars()
    {
        if (myHealthbar == null)
        {
            myHealthbar = GetComponent<Health>();

            if (myHealthbar == null)
            {
                myHealthbar = GetComponentInParent<Health>();
                parentsHealthBar = GetComponentInParent<Health>();
            }
        }
    }
}
