using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1.0f;
    public float damage = 10.0f;
    public float size = 1.0f;
    public float timer = 1f;
    public bool doActions = false;
    public bool playerBullet = false;
    public bool missile = false;

    [SerializeField] Rigidbody2D myRigidbody2D;
    [SerializeField] CircleCollider2D myCircleCollider2D;
    [SerializeField] Transform myTransform;

    private void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myCircleCollider2D = GetComponent<CircleCollider2D>();
        myTransform = GetComponent<Transform>();
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if (doActions)
        {
            if (!missile)
            {
                myRigidbody2D.AddForce(100 * speed * transform.right);
                doActions = false;
            }
            else
            {
                myRigidbody2D.AddForce(1000 * speed * transform.right * Time.deltaTime);
            }
        }

        if (timer < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !playerBullet)
        {
            Debug.Log("ouch");
        }

        if (collision.CompareTag("Hunter") && playerBullet)
        {
            Debug.Log("bonk");
        }

        if (collision.CompareTag("Security") && playerBullet)
        {
            Debug.Log("blip");
        }
        Destroy(gameObject);
    }
}
