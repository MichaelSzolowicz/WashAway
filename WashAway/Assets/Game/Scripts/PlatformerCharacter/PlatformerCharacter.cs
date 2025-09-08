using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerCharacter : MonoBehaviour
{
    [SerializeField] private PlatformerMovement platformerMovement;
    [SerializeField] private FollowCamera followCamera;
    [SerializeField] private DropSpawner dropSpawner;
    [SerializeField] private RenderTexture mask;
    [SerializeField] private WAArtist maskGenerator;
    [SerializeField] private float pixelsPerMeter;
    [SerializeField] private float blockInputRespawnDuration = .5f;

    private Vector3 startPosition;
    private bool startFacingLeft = false;

    private PixelReader pixelReader;

    private void Start()
    {
        pixelReader = new PixelReader();

        startPosition = platformerMovement.transform.position;
        startFacingLeft = platformerMovement.FacingLeft;

        GameState.onTogglePause += OnPause;
        GameState.onToggleCurrentLevelClear += OnPause;
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
        GameState.CharacterDead = true;

        dropSpawner.ResetDrops();

        platformerMovement.transform.position = startPosition;
        platformerMovement.ResetPhysicsState();
        if(platformerMovement.FacingLeft != startFacingLeft)
        {
            platformerMovement.TurnAround();
        }
        platformerMovement.blockInput = true;
        GameState.CharacterDead = false;
    }

    private void Update()
    {
        if (pixelReader.Available)
        {
            if (pixelReader.result.a >= 255 / 2)
            {
                Respawn();
            }

            float x = (platformerMovement.transform.localPosition.x * pixelsPerMeter / maskGenerator.Size) + .5f;
            float y = (platformerMovement.transform.localPosition.y * pixelsPerMeter / maskGenerator.Size) + .5f;
            pixelReader.ReadPixelAsync(mask, 0, (int)(x*mask.width), 1, (int)(y*mask.height), 1);
        }

        if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) {
            platformerMovement.blockInput = false;
        }
    }

    private void OnPause()
    {
        dropSpawner.Paused = GameState.Paused;
    }

    private void OnDestroy()
    {
        GameState.onTogglePause -= OnPause;
        GameState.onToggleCurrentLevelClear -= OnPause;
    }
}
