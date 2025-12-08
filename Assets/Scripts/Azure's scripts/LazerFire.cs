using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LazerFire : MonoBehaviour
{
    // number Structs
    public bool isActive;
    [SerializeField] int currentFrame;
    [SerializeField] int reflections;
    [SerializeField] int orderInLayer;
    [SerializeField] float maxRayLength;
    [SerializeField] float time;
    [SerializeField] float animationRate;
    [SerializeField] float damage;
    public bool playerImmune;

    // non-Number Structs
    [SerializeField] Ray ray;
    [SerializeField] RaycastHit hit;
    [SerializeField] Vector2 offset;
    [SerializeField] Vector3 direction;
    [SerializeField] LayerMask ignoreLayer;

    // components
    [SerializeField] Material[] myMaterial;
    [SerializeField] Transform targetHitTransform;

    // arrays
    [SerializeField] Transform[] newMirrors;
    [SerializeField] Transform[] oldMirrors;
    [SerializeField] LineRenderer[] myLineRenderer;
    [SerializeField] GameObject[] children;

    void Awake()
    {
        myLineRenderer = new LineRenderer[reflections];
        children = new GameObject[reflections];
        myLineRenderer[0] = GetComponent<LineRenderer>();
    }

    void Update()
    {
        for (int i = 0; i < children.Length; i++)
        {
            Destroy(children[i]);
        }

        if (isActive)
        {
            time += Time.deltaTime;
            if (time > animationRate)
            {
                time -= animationRate;
                if (currentFrame < myMaterial.Length - 1)
                {
                    currentFrame += 1;
                }
                else
                {
                    currentFrame = 0;
                }
            }

            ray = new Ray(transform.position, transform.right);
            float remainingRayLength = maxRayLength;

            for (int i = 0; i < reflections; i++)
            {
                children[i] = new GameObject("Line " + (i + 1), typeof(LineRenderer));
                children[i].GetComponent<LineRenderer>().material = myMaterial[currentFrame];
                children[i].GetComponent<LineRenderer>().material.mainTextureOffset = offset;
                children[i].GetComponent<LineRenderer>().textureMode = LineTextureMode.Tile;
                children[i].GetComponent<LineRenderer>().sortingOrder = orderInLayer + i;
                children[i].GetComponent<LineRenderer>().widthCurve = transform.GetComponent<LineRenderer>().widthCurve;
                children[i].GetComponent<LineRenderer>().sortingOrder = transform.GetComponent<LineRenderer>().sortingOrder;
                children[i].transform.parent = transform;
                myLineRenderer[i] = children[i].GetComponent<LineRenderer>();
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, remainingRayLength, ignoreLayer);
                if (hit)
                {
                    if (hit.transform.GetChild(0) != null && !playerImmune)
                    {
                        hit.transform.GetComponent<Health>().healthPoints -= damage;
                        playerImmune = true;
                    }
                    myLineRenderer[i].positionCount = 2;
                    myLineRenderer[i].SetPosition(0, ray.origin);
                    myLineRenderer[i].SetPosition(1, hit.point);
                    remainingRayLength -= Vector3.Distance(ray.origin, hit.point);
                    ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
                    if (hit.collider.CompareTag("Mirror") == false)
                    {
                        break;
                    }
                    hit.transform.gameObject.layer = 10;
                    newMirrors = new Transform[oldMirrors.Length + 1];
                    for (int f = 0; f < newMirrors.Length - 1; f++)
                    {
                        newMirrors[f] = oldMirrors[f];
                        newMirrors[f].gameObject.layer = 0;
                        newMirrors[f].tag = "Untagged";
                    }
                    newMirrors[newMirrors.Length - 1] = hit.transform;
                    oldMirrors = newMirrors;
                }
                else
                {
                    myLineRenderer[i].positionCount = 2;
                    myLineRenderer[i].SetPosition(0, ray.origin);
                    myLineRenderer[i].SetPosition(1, ray.origin + ray.direction * remainingRayLength);
                }
            }
            for (int i = 0; i < newMirrors.Length; i++)
            {
                newMirrors[i].gameObject.layer = 0;
                newMirrors[i].tag = "Mirror";
            }
            newMirrors = new Transform[0];
            oldMirrors = new Transform[0];
        }
    }
}
