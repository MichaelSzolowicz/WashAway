using UnityEngine;

public class GameRuler : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject levelClearScreen;

    private void Start()
    {
        canvas.SetActive(true);
        levelClearScreen.SetActive(false);

        GameState.onToggleCurrentLevelClear += OnToggleLevelClear;
    }

    private void OnToggleLevelClear()
    {
        if (GameState.CurrentLevelClear)
        {
            levelClearScreen.SetActive(true);
        }
        else
        {
            levelClearScreen.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        GameState.onToggleCurrentLevelClear -= OnToggleLevelClear;
    }
}

