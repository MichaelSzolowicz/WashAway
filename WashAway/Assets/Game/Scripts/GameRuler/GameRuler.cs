using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRuler : MonoBehaviour
{
    [SerializeField] private GameRules gameRules;

    private GameObject canvas;
    private GameObject pauseScreen;

    private void Update()
    {
        if(GameState.Paused)
        {
            pauseScreen.SetActive(true);
        }
        else
        {
            pauseScreen.SetActive(false);
        }
    }

    private void OnValidate()
    {
        print("Validate " + name);

        if(canvas == null)
        {
            canvas = new GameObject();
            canvas.name = "Canvas";

            Canvas canvasComponent = canvas.AddComponent<Canvas>();
            canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        if(pauseScreen == null)
        {
            if(gameRules != null 
                && gameRules.PauseScreen != null)
            {
                pauseScreen = Instantiate(gameRules.PauseScreen);
            }
            else
            {
                pauseScreen = new GameObject();
                pauseScreen.name = "PauseScreen";
            }

            pauseScreen.transform.SetParent(canvas.transform, false);
            pauseScreen.SetActive(false);
        }
    }
}
