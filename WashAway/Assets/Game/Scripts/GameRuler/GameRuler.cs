using UnityEngine;

public class GameRuler : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject levelClearScreen;
    [SerializeField] private GameObject pauseScreen;

    private void Start()
    {
        canvas.SetActive(true);
        levelClearScreen.SetActive(false);
        pauseScreen.SetActive(false);

        GameState.onToggleCurrentLevelClear += OnToggleLevelClear;
        GameState.onTogglePause += OnTogglePause;
    }

    private void OnToggleLevelClear()
    {
        levelClearScreen.SetActive(GameState.CurrentLevelClear);
    }

    private void OnTogglePause()
    {
        pauseScreen.SetActive(GameState.Paused);

    }

    private void OnDestroy()
    {
        GameState.onToggleCurrentLevelClear -= OnToggleLevelClear;
        GameState.onTogglePause -= OnTogglePause;
    }
}

