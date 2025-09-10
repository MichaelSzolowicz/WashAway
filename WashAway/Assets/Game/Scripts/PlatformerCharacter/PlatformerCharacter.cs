using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerCharacter : MonoBehaviour
{
    [System.Serializable]   
    private class PlatformerCharacterDebugConfig
    {
        public bool disableDamage = false;
    }

    [SerializeField] private PlatformerMovement platformerMovement;
    [SerializeField] private DropSpawner dropSpawner;
    [SerializeField] private WAArtist maskGenerator;

    [Header("")]
    [SerializeField] private PlatformerCharacterDebugConfig debug;

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
        if (debug.disableDamage) return;

        GameState.CharacterDead = true;

        dropSpawner.ResetDrops();

        platformerMovement.transform.position = startPosition;
        platformerMovement.ResetPhysicsState();
        if(platformerMovement.FacingLeft != startFacingLeft)
        {
            platformerMovement.TurnAround();
        }
        platformerMovement.blockInput = true;

        maskGenerator.ClearRenderTarget();

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

            float x = (platformerMovement.transform.localPosition.x * maskGenerator.InverseWidthMeters) + .5f;
            float y = (platformerMovement.transform.localPosition.y * maskGenerator.InverseWidthMeters) + .5f;
            pixelReader.ReadPixelAsync(maskGenerator.TargetRenderTexture, 0, (int)(x*maskGenerator.TargetRenderTexture.width), 1, (int)(y*maskGenerator.TargetRenderTexture.height), 1);
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
