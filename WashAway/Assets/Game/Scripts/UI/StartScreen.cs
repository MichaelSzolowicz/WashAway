using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private bool requireFeedback = true;

    private void Start()
    {
        if (requireFeedback)
        {
            continueButton.interactable = false;

            GameState.onToggleFeedbackViewed += EnableContinueButton;
        }
        else
        {
            GameState.FeedbackViewed = true;
        }

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
    }
}
