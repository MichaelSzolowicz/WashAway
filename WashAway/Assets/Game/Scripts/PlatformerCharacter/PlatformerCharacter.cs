using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerCharacter : MonoBehaviour
{
    [SerializeField] private PlatformerMovement platformerMovement;
    [SerializeField] private FollowCamera followCamera;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("DamageCauser"))
        {
            Respawn();
            print(name + " on trigger enter 2d.");
        }
    }

    private void Respawn()
    {
        platformerMovement.transform.position = startPosition;
        platformerMovement.ResetPhysicsState();
    }
}
