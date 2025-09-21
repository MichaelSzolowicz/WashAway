using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerCharacter : MonoBehaviour
{
    [System.Serializable]   
    private class PlatformerCharacterDebugConfig
    {
        public bool disableDamage = false;
        public bool showProbes = false;
        public bool startFacingLeft = false;
    }

    [SerializeField] private PlatformerMovement platformerMovement;
    [SerializeField] private DropSpawner dropSpawner;
    [SerializeField] private WAArtist maskGenerator;

    [SerializeField] private float probeHeight = .5f;

    [Header("")]
    [SerializeField] private PlatformerCharacterDebugConfig debug;

    private Vector3 startPosition;
    private bool startFacingLeft = false;

    private PixelReader pixelReader;

    private bool showMaskPercent = false;
    private float maskPercent = 0;

    private void Start()
    {
        pixelReader = new PixelReader();

        startPosition = platformerMovement.transform.position;
        startFacingLeft = platformerMovement.FacingLeft;

        GameState.onTogglePause += OnPause;
        GameState.onToggleCurrentLevelClear += OnPause;

        if (debug.startFacingLeft && !platformerMovement.FacingLeft) { platformerMovement.TurnAround(); }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("DamageCauser"))
        {
            //Respawn();
            OnDeath();
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

    private void OnDeath()
    {
        GameState.CharacterDead = true;
        platformerMovement.enabled = false;

        //GameState.Paused = true;

        //MaskPercentCalculator calc = new MaskPercentCalculator(ShowMaskPercent);
        //calc.RequestPercentCleared(maskGenerator.TargetRenderTexture);
    }

    private void ShowMaskPercent(float percent)
    {
        maskPercent = percent;
        showMaskPercent = true;
    }

    private void Update()
    {
        if (pixelReader.Available)
        {
            if (pixelReader.result.a >= 255 / 2)
            {
                //Respawn();
                OnDeath();
            }

            if (debug.showProbes)
            {
                Vector3 probePos = platformerMovement.transform.position;
                probePos.y += probeHeight;
                Debug.DrawLine(probePos, probePos + Vector3.back, Color.red, Time.deltaTime * 2);
            }

            float x = (platformerMovement.transform.localPosition.x * maskGenerator.InverseWidthMeters) + .5f;
            float y = ((platformerMovement.transform.localPosition.y + probeHeight) * maskGenerator.InverseWidthMeters) + .5f;

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

    private void OnGUI()
    {
        if(showMaskPercent)
        {
            GUILayout.BeginVertical();
            GUILayout.Space(20);
            GUILayout.Label("mask percent: " + maskPercent);
            GUILayout.EndVertical();
        }
    }
}
