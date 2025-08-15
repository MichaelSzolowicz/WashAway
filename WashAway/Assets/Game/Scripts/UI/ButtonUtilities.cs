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
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSdumFWm7Qtra5huqvl9IesvvXo8C6Qc7E1qzpjM2an08NBINg/viewform?usp=sharing&ouid=116477818506344356711");
    }
}
