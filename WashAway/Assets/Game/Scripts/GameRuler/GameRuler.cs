using System.Collections;
using System.Collections.Generic;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;

[ExecuteInEditMode]
public class GameRuler : MonoBehaviour
{
    [SerializeField] private GameRules gameRules;

    [SerializeField] private bool forceValidateTest;

    [SerializeField] [HideInInspector] private GameObject canvas;
    [SerializeField] [HideInInspector] private GameObject pauseScreen;
    [SerializeField][HideInInspector] private GameObject pauseScreenTemplate;

    private bool spawnCanvas = false;
    private bool spawnPauseScreen = false;

    private void Update()
    {
        if(Application.isPlaying)
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
        else
        {
            if(spawnCanvas)
            {
                canvas = new GameObject();
                canvas.name = "Canvas";

                Canvas canvasComponent = canvas.AddComponent<Canvas>();
                canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;

                spawnCanvas = false;

                print("Spawn Canvas");
            }

            if(spawnPauseScreen)
            {
                SpawnPauseScreen();
            }

#if UNITY_EDITOR
            EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
#endif
        }
    }

    private void SpawnPauseScreen()
    {
        if (pauseScreen != null)
        {
            DestroyImmediate(pauseScreen);
        }

        if (gameRules != null 
            && gameRules.PauseScreen != null)
        {
            pauseScreenTemplate = gameRules.PauseScreen;
            pauseScreen = Instantiate(pauseScreenTemplate);
        }
        else
        {
            pauseScreen = pauseScreenTemplate = new GameObject();
            pauseScreen.name = "PauseScreen";
        }

        pauseScreen.transform.SetParent(canvas.transform, false);
        pauseScreen.SetActive(false);

        spawnPauseScreen = false;

        print("Spawn Pause Screen");
    }

    private void ValidateSubobjects()
    {
        spawnCanvas = canvas == null;

        if(gameRules != null)
        {
            spawnPauseScreen = gameRules.PauseScreen != pauseScreenTemplate || pauseScreen == null;
        }
    }

    private void OnValidate()
    {
        if(gameRules != null)
        {
            gameRules.onValidateGameRules += ValidateSubobjects;
        }

        ValidateSubobjects();
    }

    private void OnDestroy()
    {
        if(gameRules != null)
        {
            gameRules.onValidateGameRules -= ValidateSubobjects;
        }
    }
}

