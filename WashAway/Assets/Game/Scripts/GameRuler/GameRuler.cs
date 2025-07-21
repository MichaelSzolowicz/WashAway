using UnityEngine;

public class GameRuler : MonoBehaviour
{
    [SerializeField] private GameRules gameRules;

    [SerializeField] [HideInInspector] private GameObject canvas;
    [SerializeField] [HideInInspector] private GameObject pauseScreen;


    private void Start()
    {
        canvas = Instantiate(gameRules.Canvas);
        pauseScreen = Instantiate(gameRules.PauseScreen, canvas.transform, false);
    }

    private void Update()
    {
        if (GameState.Paused)
        {
            pauseScreen.SetActive(true);
        }
        else
        {
            pauseScreen.SetActive(false);
        }
    }
}

