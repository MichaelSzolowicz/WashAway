using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private RenderTexture mask;
    [SerializeField] private float paintingWidthMeters;
    [SerializeField] private float deathTime = .5f;

    private float timeSinceLastRead = 0;
    private float timeInVoid = 0;

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

        StartCoroutine(CheckPixel());

        if (debug.startFacingLeft && !platformerMovement.FacingLeft) { platformerMovement.TurnAround(); }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("DamageCauser"))
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        if(debug.disableDamage) return;

        GameState.CharacterDead = true;
        platformerMovement.enabled = false;
        dropSpawner.stopSpawning = true;
    }

    /// <summary>
    /// Any pixel reading needs to be done after the first time a camera writes to the render target.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckPixel()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();

            if (pixelReader.Available)
            {
                if (pixelReader.result.b >= 255.0f / 2.0f)
                {
                    timeInVoid += timeSinceLastRead;

                    if (timeInVoid > deathTime)
                    {
                        OnDeath();
                    }
                }

                if (debug.showProbes)
                {
                    Vector3 probePos = platformerMovement.transform.position;
                    Debug.DrawLine(probePos, probePos + Vector3.back, Color.red, Time.deltaTime * 2);
                }

                float x = (platformerMovement.transform.position.x * 1 / paintingWidthMeters) + .5f;
                float y = ((platformerMovement.transform.position.y) * 1 / paintingWidthMeters) + .5f;

                pixelReader.ReadPixelAsync(mask, 0, (int)(x * mask.width), 1, (int)(y * mask.height), 1);

                timeSinceLastRead = 0;
            }

            timeSinceLastRead += Time.deltaTime;
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
