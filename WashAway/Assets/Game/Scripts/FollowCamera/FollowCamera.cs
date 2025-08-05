using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed;

    private Vector3 startOffest;

    void Start()
    {
        startOffest = transform.position - target.position;
    }

    void LateUpdate()
    {
        Vector3 delta = target.position - (transform.position);
        delta.z = 0;

        transform.position = transform.position + delta * speed;
    }
}
