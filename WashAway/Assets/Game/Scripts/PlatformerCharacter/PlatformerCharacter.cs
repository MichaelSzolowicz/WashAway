using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerCharacter : MonoBehaviour
{
    [SerializeField] private CharacterMovement lineCharacterMovement;
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
        }
    }

    private void Respawn()
    {
        transform.position = startPosition;
        lineCharacterMovement.ResetPhysicsState();
    }
}
