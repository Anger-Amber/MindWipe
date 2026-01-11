using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class ArmedadilloScript : MonoBehaviour
{
    BoxCollider2D myBoxCollider2D;
    Rigidbody2D myRigidbody2D;
    ParticleSystem myParticleSystem;
    GameObject dupedParticleSystem;
    TilemapCollider2D groundTilesCollider;
    Tilemap groundTiles;
    [SerializeField] GameObject player;
    [SerializeField] GameObject headController;
    [SerializeField] GameObject gunController;
    [SerializeField] GameObject firePoint;

    [SerializeField] float actionTimer;
    [SerializeField] float telegraphWindow;
    [SerializeField] Vector2 launchPower;
    [SerializeField] int chosenAction;
    [SerializeField] bool lazerUp;
    [SerializeField] bool immunityFramesUp;
    [SerializeField] Color warningFade;
    [SerializeField] SpriteRenderer biteRenderer;
    [SerializeField] Animator idleAnimator;
    [SerializeField] AnimationClip idleAnim;
    [SerializeField] Animator biteAnimation;
    [SerializeField] BoxCollider2D biteCollider;
    void Awake()
    {
        myBoxCollider2D = GetComponent<BoxCollider2D>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myParticleSystem = transform.GetChild(2).GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (transform.GetComponent<Health>().healthPoints > 0)
        {
            myRigidbody2D.linearVelocityX = (player.transform.position.x - transform.position.x) * 0.5f;
            myRigidbody2D.linearVelocityX += 10f;
            idleAnimator.speed = myRigidbody2D.linearVelocityX / 20;

            actionTimer += Time.deltaTime;
            if (actionTimer > 0)
            {
                switch (chosenAction)
                {
                    case 0: // bite
                        firePoint.GetComponent<LazerFire>().playerImmune = false;
                        firePoint.GetComponent<LazerFire>().isActive = false;
                        firePoint.GetComponent<Light2D>().enabled = false;
                        gunController.GetComponent<Light2D>().enabled = false;
                        biteAnimation.SetBool("Chomp", true);
                        warningFade = biteRenderer.color;
                        warningFade.a += Time.deltaTime / telegraphWindow;
                        biteRenderer.color = warningFade;
                        if (actionTimer > telegraphWindow)
                        {
                            biteCollider.GetComponent<ZoneDamage>().immuneObjects[0] = null;
                            biteCollider.enabled = true;
                            actionTimer = -telegraphWindow;
                            warningFade.a = 0f;
                            biteRenderer.color = warningFade;
                            chosenAction = Random.Range(0, 2);
                            biteAnimation.SetBool("Chomp", false);
                            immunityFramesUp = true;
                        }
                        break;
                    case 1: // lazer up
                        biteCollider.enabled = false;
                        firePoint.GetComponent<Light2D>().enabled = true;
                        gunController.GetComponent<Light2D>().intensity += Time.deltaTime / telegraphWindow;
                        gunController.GetComponent<Light2D>().enabled = true;
                        firePoint.GetComponent<LazerFire>().isActive = true;
                        if (lazerUp) { gunController.transform.Rotate(0, 0, Time.deltaTime * telegraphWindow * 400); }
                        if (!lazerUp) { gunController.transform.Rotate(0, 0, Time.deltaTime * telegraphWindow * -400); }
                        if (actionTimer > telegraphWindow)
                        {
                            chosenAction = Random.Range(0, 2);
                            actionTimer = -telegraphWindow;
                            lazerUp = !lazerUp;
                            firePoint.GetComponent<LazerFire>().playerImmune = false;
                            immunityFramesUp = true;
                        }
                        break;
                    case 2: // saw
                        break;
                    case 3: // bait?
                        break;
                }
            }
        }

        else
        {
            myRigidbody2D.bodyType = RigidbodyType2D.Static;
            biteAnimation.enabled = false;
            idleAnimator.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DoCollision(collision.gameObject);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        DoCollision(collision.gameObject);
    }
    bool CheckTile(int tileCheckLocationx, int tileCheckLocationy)
    {
        
        return (groundTiles.GetTile(Vector3Int.FloorToInt
                (new Vector3(transform.position.x * 0.5f + myBoxCollider2D.size.x * 0.25f + tileCheckLocationx, tileCheckLocationy, 0))) != null);
    }    
    void DeleteTile(int tileDeleteLocationx, int tileDeleteLocationy)
    {
        groundTiles.SetTile(Vector3Int.FloorToInt
                (new Vector3(transform.position.x * 0.5f + myBoxCollider2D.size.x * 0.25f + tileDeleteLocationx, tileDeleteLocationy, 0)), null);
        dupedParticleSystem = Instantiate(myParticleSystem.gameObject, Vector3Int.FloorToInt
                (new Vector3(transform.position.x * 0.5f + myBoxCollider2D.size.x * 0.25f + tileDeleteLocationx, tileDeleteLocationy, 0)) * 2, Quaternion.identity);   
        dupedParticleSystem.SetActive(true);
    }
    void DoCollision(GameObject collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            groundTilesCollider = collision.GetComponent<TilemapCollider2D>();
            groundTiles = collision.GetComponent<Tilemap>();
            if (CheckTile(0, -1))
            {
                DeleteTile(0, -1);
            }
            if (CheckTile(0, 0))
            {
                DeleteTile(0, 0);
            }
            if (CheckTile(0, 1))
            {
                DeleteTile(0, 1);
            }
            if (CheckTile(-1, -1))
            {
                DeleteTile(-1, -1);
            }
            if (CheckTile(-1, 0))
            {
                DeleteTile(-1, 0);
            }
            if (CheckTile(-1, 1))
            {
                DeleteTile(-1, 1);
            }
        }
        else if (collision.transform.CompareTag("Player"))
        {
            if (!immunityFramesUp)
            {
                player.GetComponent<Health>().healthPoints -= headController.transform.GetChild(0).GetComponent<ZoneDamage>().damage;
                immunityFramesUp = true;
            }
            player.GetComponent<Rigidbody2D>().linearVelocityY = launchPower.y;
            player.GetComponent<Rigidbody2D>().linearVelocityX = launchPower.x;
        }
    }
}
