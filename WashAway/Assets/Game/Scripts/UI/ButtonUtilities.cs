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
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSf4WuAm5bBNaBVtu9qTkLZ5AvLCRxtVDtodNG_7IGoxGQHCYg/viewform?usp=sharing&ouid=116477818506344356711");
        GameState.FeedbackViewed = true;
    }
}
