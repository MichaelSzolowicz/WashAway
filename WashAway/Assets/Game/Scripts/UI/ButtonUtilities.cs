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
        Application.OpenURL("https://forms.gle/5LddnxPTrpeaMCUbA");
        GameState.FeedbackViewed = true;
    }
}
