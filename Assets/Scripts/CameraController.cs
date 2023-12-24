using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 _offset;


    void Start()
    {
        _offset = transform.position - player.position;
    }

    private void FixedUpdate()
    {
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, _offset.z + player.position.z);
        transform.position = newPosition;
    }
}