using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    [SerializeField] private Button continueButton;

    private void Start()
    {
        continueButton.interactable = false;

        GameState.onToggleFeedbackViewed += EnableContinueButton;

        GameState.Paused = true;
    }

    private void EnableContinueButton()
    {
        continueButton.interactable = GameState.FeedbackViewed;
    }

    public void DisableStartScreen()
    {
        gameObject.SetActive(false);
        GameState.Paused = false;
        GameState.StartScreenRead = true;
    }
}
