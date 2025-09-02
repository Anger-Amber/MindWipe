using Unity.Mathematics;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    //components
    [SerializeField] Transform firePoint;
    [SerializeField] Transform origin;
    [SerializeField] Transform parentsTransform;

    [SerializeField] Transform[] possibleTargets;
    [SerializeField] Transform targetTransform;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] SpriteRenderer bulletSprite;
    [SerializeField] CircleCollider2D colliderSettings;
    [SerializeField] Targetables parentsTargetList;
    [SerializeField] Camera cam;

    [SerializeField] float[] bulletSpreadAndAmount =
    {
        -10,
        0,
        10,
    };

    // bullet Variables
    [SerializeField] float bulletSpeed = 1f;
    [SerializeField] float originalDamage = 10f;
    public float damageMultiplier = 1f;
    [SerializeField] float bulletGravity = 0f;
    [SerializeField] float bulletScale = 1f;
    [SerializeField] float bulletTimer = 1f;
    [SerializeField] float fireRate = 1f;
    [SerializeField] float time = 0f;

    [SerializeField] bool isPlayer = false;
    [SerializeField] bool isSecurity = false;
    [SerializeField] bool missile = false;

    [SerializeField] Vector2 targetPosition = new (0,0);

    [SerializeField] quaternion tempRotation;

    private void Awake()
    {
        // Getting the bullet-prefab, firepoint and setting variables
        bulletPrefab = gameObject.transform.GetChild(0).gameObject;
        firePoint = gameObject.transform.GetChild(1).gameObject.transform;

        bulletSprite = bulletPrefab.GetComponent<SpriteRenderer>();
        colliderSettings = bulletPrefab.GetComponent<CircleCollider2D>();
        origin = gameObject.transform;

        // Getting if you are a player or not
        parentsTransform = GetComponentInParent<Transform>();
        cam = Camera.main;
        if (parentsTransform.CompareTag("Player"))
        {
            isPlayer = true;
            time = -1;
        }
        else
        {
            parentsTargetList = GetComponentInParent<Targetables>();
        }
    }

    private void Update()
    {
        
        if (!isPlayer)
        {
            //makes the shooter look at the target
            possibleTargets = parentsTargetList.compressedTargetList;
            targetTransform = GetClosestEnemy(possibleTargets);
            targetPosition = targetTransform.position - gameObject.transform.position;
            float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg - 0f;
            if (angle > 90f)
            {
                origin.transform.rotation = Quaternion.Euler(180, 0, -angle);
            }
            else if (angle < -90f)
            {
                origin.transform.rotation = Quaternion.Euler(180, 0, -angle);
            }
            else
            {
                origin.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }

        // temporary Scuff Priority 4
        // player Looks At The Cursor
        if (isPlayer)
        {

            targetPosition = new Vector2();
            if (time > 0)
            {
                targetPosition = cam.ScreenToWorldPoint(Input.mousePosition) - gameObject.transform.position;
            }
            float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg - 0f;
            if (angle > 90f && parentsTransform.localScale.x == 1)
            {
                origin.transform.rotation = Quaternion.Euler(180, 0, -angle);
                origin.transform.localScale = Vector3.one;
            }
            else if (angle < -90f && parentsTransform.localScale.x == 1)
            {
                origin.transform.rotation = Quaternion.Euler(180, 0, -angle);
                origin.transform.localScale = Vector3.one;
            }
            else if (angle < 90 && angle > -90 && parentsTransform.localScale.x == 1)
            {
                origin.transform.rotation = Quaternion.Euler(0, 0, angle);
                origin.transform.localScale = Vector3.one;
            }

            if (angle > 90f && parentsTransform.localScale.x == -1)
            {
                origin.transform.rotation = Quaternion.Euler(180, 0, -angle);
                origin.localScale = new Vector3(-1,1,1);
            }
            else if (angle < -90f && parentsTransform.localScale.x == -1)
            {
                origin.transform.rotation = Quaternion.Euler(180, 0, -angle);
                origin.localScale = new Vector3(-1, 1, 1);
            }
            else if (angle < 90 && angle > -90 && parentsTransform.localScale.x == -1)
            {
                origin.transform.rotation = Quaternion.Euler(0, 0, angle);
                origin.localScale = new Vector3(-1, 1, 1);
            }

        }

        // Capping firerate at the maximum
        if (time < fireRate)
        {
            time += Time.deltaTime;
        }
        else
        {
            time = fireRate;
        }

        // Shooting if not the  player
        if (time >= fireRate && !isPlayer)
        {
            for (int i = 0; i < bulletSpreadAndAmount.Length; i++)
            {
                Shoot(bulletSpreadAndAmount[i]);
            }
            time -= fireRate;
        }

        // Shooting if you are the player
        if (time >= fireRate && Input.GetMouseButtonDown(0) && isPlayer)
        {
            for (int i = 0; i < bulletSpreadAndAmount.Length; i++)
            {
                Shoot(bulletSpreadAndAmount[i]);
            }
            time -= fireRate;
        } 
    }

    void Shoot(float rotation)
    {
        //making The Bullet(I Don't like instansiate);   <-- get it cause programming :O
        //i Have Sleep Deprivation
        //i Will Probably delete this sometime
        //ha ha fuck you I won't
        // I'm not going to change the grammar or spelling of these comments, because it's funny
        GameObject shotBullet;
        shotBullet = new GameObject
        (
            "bullet",
            typeof(Rigidbody2D),
            typeof(Bullet),
            typeof(SpriteRenderer),
            typeof(CircleCollider2D)
        );
        //scale
        shotBullet.transform.localScale = new Vector3(bulletScale,bulletScale,bulletScale);

        //position
        shotBullet.transform.position = firePoint.position;
        shotBullet.transform.rotation = gameObject.transform.rotation;
        shotBullet.transform.Rotate(0,0,rotation);

        //getting The Components Through Code
        SpriteRenderer spriteRenderer = shotBullet.GetComponent<SpriteRenderer>();
        Rigidbody2D bulletRigidbody = shotBullet.GetComponent<Rigidbody2D>();
        Bullet bulletScript = shotBullet.GetComponent<Bullet>();
        CircleCollider2D circleCollider = shotBullet.GetComponent<CircleCollider2D>();

        //rigidbody Settings 
        bulletRigidbody.freezeRotation = true;

        //collider Settings
        circleCollider.includeLayers = colliderSettings.includeLayers;
        circleCollider.excludeLayers = colliderSettings.excludeLayers;
        circleCollider.radius = circleCollider.radius * 0.40f;
        circleCollider.isTrigger = true;

        //settings Of The Bullets
        spriteRenderer.sprite = bulletSprite.sprite;
        spriteRenderer.color = bulletSprite.color;
        spriteRenderer.sortingOrder = 100;
        bulletRigidbody.gravityScale = bulletGravity;
        bulletScript.damage = originalDamage * damageMultiplier;
        bulletScript.speed = bulletSpeed;
        bulletScript.doActions = true;
        bulletScript.timer = bulletTimer;
        bulletScript.missile = missile;
    }

    // Getting the closest enemy through code
    Transform GetClosestEnemy(Transform[] enemies)
    {
        Transform targetMinimum = null;
        float minimumDistance = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Transform t in enemies)
        {
            if (t != null)
            {
                float distance = Vector3.Distance(t.position, currentPos);
                if (distance < minimumDistance &&
                    t != origin &&
                    t != parentsTransform)
                {
                    targetMinimum = t;
                    minimumDistance = distance;
                }
            }
        }
        return targetMinimum;
    }
}