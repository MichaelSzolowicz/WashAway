using UnityEngine;

public class GameRuler : MonoBehaviour
{
    [SerializeField] private GameRules gameRules;

    [SerializeField] [HideInInspector] private GameObject canvas;
    [SerializeField] [HideInInspector] private GameObject pauseScreen;


    private void Start()
    {
        if(gameRules != null)
        {
            canvas = Instantiate(gameRules.Canvas);
            if(canvas == null)
            {
                canvas = new GameObject();
                canvas.name = "Canvas";
            }

            pauseScreen = Instantiate(gameRules.PauseScreen, canvas.transform, false);
            if(pauseScreen == null)
            {
                pauseScreen = new GameObject();
                pauseScreen.name = "PauseScreen";
            }
        }
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

