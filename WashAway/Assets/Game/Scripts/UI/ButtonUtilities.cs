using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUtilities : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }

    public void Retry()
    {
        StartCoroutine(ReloadSceneAsync());
    }

    private IEnumerator ReloadSceneAsync()
    {
        AsyncOperation asyncReload = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        while(!asyncReload.isDone)
        {
            yield return null;
        }
    }

    public void Feedback()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSdOOPK08oyT__V_q2WbhGUcC7LhKDiJtoCMP-_GKwYG4wyTxQ/viewform?usp=preview");
        GameState.FeedbackViewed = true;
    }
}
