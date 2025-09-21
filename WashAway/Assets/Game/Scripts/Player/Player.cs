using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    [System.Serializable]
    private class PlayerDebugConfig
    {
        public bool disableStartScreen = false;
    }

    [Header("UI")]
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject levelClearScreen;
    [SerializeField] private PauseScreen pauseScreen;
    [SerializeField] private GameObject startScreen;

    [Header("Level")]
    [SerializeField] private RenderTexture mask;
    [SerializeField] private float minClearPercent;

    [Header("")]
    [SerializeField] private PlayerDebugConfig debug;

    private void Start()
    {
        canvas.SetActive(true);
        levelClearScreen.SetActive(false);
        pauseScreen.gameObject.SetActive(false);

        if(!debug.disableStartScreen)
        {
            startScreen.SetActive(!GameState.FeedbackViewed);
        }

        GameState.onToggleCharacterDead += OnToggleCharacterDead;
        GameState.onTogglePause += OnTogglePause;

        GameState.Paused = false;
        GameState.CharacterDead = false;
    }

    void Update()
    {
        if (
            Input.GetKeyDown(KeyCode.Escape)
            && !GameState.CurrentLevelClear
            )
        {
            GameState.Paused = !GameState.Paused;
        }
    }

    private void OnToggleCharacterDead()
    {
        //levelClearScreen.SetActive(GameState.CurrentLevelClear);

        if(GameState.CharacterDead)
        {
            MaskPercentCalculator calc = new MaskPercentCalculator(ShowDeathScreen);
            calc.RequestPercentCleared(mask);
        }
        else
        {
            pauseScreen.gameObject.SetActive(false);
        }
    }

    private void OnTogglePause()
    {
        if (startScreen.activeInHierarchy) return;
        if (levelClearScreen.activeInHierarchy) return;

        if(GameState.Paused)
        {
            MaskPercentCalculator calc = new MaskPercentCalculator(ShowPauseScreen);
            calc.RequestPercentCleared(mask);
        }
        else
        {
            pauseScreen.gameObject.SetActive(false);
        }
    }

    private void ShowPauseScreen(float maskPercent)
    {
        pauseScreen.EnableAsPauseScreen(maskPercent, minClearPercent);
    }

    private void ShowDeathScreen(float maskPercent)
    {
        if(maskPercent >= minClearPercent)
        {
            pauseScreen.EnableAsVictoryScreen(maskPercent, minClearPercent);
        }
        else
        {
            pauseScreen.EnableAsDeathScreen(maskPercent, minClearPercent);
        }
    }

    private void OnDestroy()
    {
        GameState.onToggleCharacterDead -= OnToggleCharacterDead;
        GameState.onTogglePause -= OnTogglePause;
    }
}
