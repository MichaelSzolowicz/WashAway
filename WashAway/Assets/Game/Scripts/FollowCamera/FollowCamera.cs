using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float lagTime;

    private Vector3 startOffet;

    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        startOffet = transform.position - target.position;
    }

    void LateUpdate()
    {
        Vector3 newPosition = Vector3.SmoothDamp(transform.position, target.position + startOffet, ref velocity, lagTime);
        newPosition.z = transform.position.z;
        transform.position = newPosition;
    }
}
