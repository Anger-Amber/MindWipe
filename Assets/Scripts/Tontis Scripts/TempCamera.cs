using UnityEngine;

public class TempCamera : MonoBehaviour
{
    [SerializeField] Transform Player;
    [SerializeField] float cameraSpeed;

    Vector3 cameraPosition;    

    void Update()
    {
        cameraPosition = Vector2.Lerp(Player.position, transform.position, Mathf.Pow(0.5f, cameraSpeed * Time.deltaTime));
        cameraPosition.z = -10;
        transform.position = cameraPosition;    
    }
}
