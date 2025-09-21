using TMPro;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text maskPercentText;

    private const string maskPercentLabel = "Cleared: ";

    public void EnableAsPauseScreen(float maskPercent, float requiredPercent)
    {
        titleText.color = Color.white;
        titleText.text = "Paused";
        FinishEnable(maskPercent, requiredPercent);
    }

    public void EnableAsDeathScreen(float maskPercent, float requiredPercent)
    {
        titleText.color = Color.red;
        titleText.text = "Try Again";
        FinishEnable(maskPercent, requiredPercent);
    }

    public void EnableAsVictoryScreen(float maskPercent, float requiredPercent)
    {
        titleText.color = Color.yellow;
        titleText.text = "Level Clear";
        FinishEnable(maskPercent, requiredPercent);
    }

    private void FinishEnable(float maskPercent, float requiredPercent)
    {
        maskPercentText.text = maskPercentLabel + maskPercent + " / " + requiredPercent + "%";
        gameObject.SetActive(true);
    }
}
