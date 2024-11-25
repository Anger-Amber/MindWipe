using UnityEngine;

public class ForwardingTransforms : MonoBehaviour
{
    public Transform myTransform;

    void Start()
    {
        myTransform = gameObject.transform;
    }
}
