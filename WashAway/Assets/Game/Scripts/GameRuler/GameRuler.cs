using UnityEngine;

public class GameRuler : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject pauseScreen;


    private void Start()
    {
        canvas.SetActive(true);
        pauseScreen.SetActive(false);
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

