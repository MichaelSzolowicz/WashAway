using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float lagTime;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        Vector3 newPosition = Vector3.SmoothDamp(transform.position, target.position, ref velocity, lagTime);
        newPosition.z = transform.position.z;
        transform.position = newPosition;
    }
}
