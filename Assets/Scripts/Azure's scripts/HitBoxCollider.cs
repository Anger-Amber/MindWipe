using UnityEngine;

public class HitBoxCollider : MonoBehaviour
{
    [SerializeField] Health myHealthbar;
    [SerializeField] Health parentsHealthBar;

    private void Awake()
    {
        DefineHealthBars();

        //Debug.Log("My health bar is: " + myHealthbar);
        //Debug.Log("My parents health bar is: " + parentsHealthBar);
    }

    void DefineHealthBars()
    {
        if (myHealthbar == null)
        {
            //Debug.Log("My health bar is null");
            myHealthbar = GetComponent<Health>();

            if (myHealthbar == null)
            {
                //Debug.Log("My health bar is STILL null");
                myHealthbar = GetComponentInParent<Health>();
                parentsHealthBar = GetComponentInParent<Health>();
            }
        }
    }

    //private void FixedUpdate()
    //{
    //    if (myHealthbar == null)
    //    {
    //        DefineHealthBars();
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Bullet>() != null)
        {
            myHealthbar.healthPoints -= collision.gameObject.GetComponent<Bullet>().damage;
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
}
