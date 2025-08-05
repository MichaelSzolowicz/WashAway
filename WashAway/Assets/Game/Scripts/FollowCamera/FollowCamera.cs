using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform followTransform;

    private Vector3 startOffest;

    void Start()
    {
        startOffest = transform.position - followTransform.position;
    }

    void LateUpdate()
    {
        transform.position = followTransform.position + startOffest;
    }
}
