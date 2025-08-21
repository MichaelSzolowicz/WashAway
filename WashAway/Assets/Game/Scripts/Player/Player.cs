using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject levelClearScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject startScreen;

    private void Start()
    {
        canvas.SetActive(true);
        levelClearScreen.SetActive(false);
        pauseScreen.SetActive(false);

        //startScreen.SetActive(!GameState.StartScreenRead);

        GameState.onToggleCurrentLevelClear += OnToggleLevelClear;
        GameState.onTogglePause += OnTogglePause;

        GameState.Paused = false;
        GameState.CurrentLevelClear = false;
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

    private void OnToggleLevelClear()
    {
        levelClearScreen.SetActive(GameState.CurrentLevelClear);
    }

    private void OnTogglePause()
    {
        if (startScreen.activeInHierarchy) return;
        if (levelClearScreen.activeInHierarchy) return;

        pauseScreen.SetActive(GameState.Paused);
    }

    private void OnDestroy()
    {
        GameState.onToggleCurrentLevelClear -= OnToggleLevelClear;
        GameState.onTogglePause -= OnTogglePause;
    }
}
