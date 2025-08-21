using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerCharacter : MonoBehaviour
{
    [SerializeField] private PlatformerMovement platformerMovement;
    [SerializeField] private FollowCamera followCamera;
    [SerializeField] private RenderTexture mask;
    [SerializeField] private WAArtist maskGenerator;
    [SerializeField] private float pixelsPerMeter;

    private Vector3 startPosition;

    private PixelReader pixelReader;

    private void Start()
    {
        pixelReader = new PixelReader();

        startPosition = platformerMovement.transform.position;
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

    private void Update()
    {
        return;
        if (!pixelReader.dirty)
        {
            float x = (platformerMovement.transform.localPosition.x / (maskGenerator.Size / pixelsPerMeter) * 30) + .5f;
            float y = (platformerMovement.transform.localPosition.y / (maskGenerator.Size / pixelsPerMeter) * 30) + .5f;
            Debug.Log(x + ", " + y);
            Debug.Log((int)(x * mask.width) + ", " + (int)(y * mask.height));
            pixelReader.ReadPixelAsync(mask, 0, (int)(x*mask.width), 1, (int)(y*mask.height), 1);
            pixelReader.dirty = true;
        }
        else
        {
            Debug.Log(pixelReader.result);
            pixelReader.dirty = false;
        }
    }
}
