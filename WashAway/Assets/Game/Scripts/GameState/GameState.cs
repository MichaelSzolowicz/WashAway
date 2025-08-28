using UnityEngine;

public class GameState
{
    public delegate void OnToggle();

    private class ToggleState
    {
        public bool value;
        private bool defaultValue;
        public event OnToggle onToggle;

        public ToggleState(bool value = false)
        {
            this.value = value;
            defaultValue = value;
        }

        public bool DefaultValue { get { return defaultValue; } }
        
        public void ValueChanged() { onToggle?.Invoke(); }
    }

    private static ToggleState paused = new ToggleState();
    public static bool Paused { get { return ReadToggleState(paused); } set { ChangeToggleState(paused, value); } }
    public static event OnToggle onTogglePause { add { paused.onToggle += value; } remove { paused.onToggle -= value; } }

    private static ToggleState currentLevelClear = new ToggleState();
    public static bool CurrentLevelClear { get { return ReadToggleState(currentLevelClear); } set { ChangeToggleState(currentLevelClear, value); } }
    public static event OnToggle onToggleCurrentLevelClear { add { currentLevelClear.onToggle += value; } remove { currentLevelClear.onToggle -= value; } }

    private static ToggleState feedbackViewed = new ToggleState();
    public static bool FeedbackViewed { get { return ReadToggleState(feedbackViewed); } set { ChangeToggleState(feedbackViewed, value); } }
    public static event OnToggle onToggleFeedbackViewed { add { feedbackViewed.onToggle += value; } remove { feedbackViewed.onToggle -= value; } }

    private static ToggleState characterDead = new ToggleState();
    public static bool CharacterDead { get { return ReadToggleState(characterDead); } set { ChangeToggleState(characterDead, value); } }
    public static event OnToggle onToggleCharacterDead { add { characterDead.onToggle += value; } remove { characterDead.onToggle -= value; } }

    private static void ChangeToggleState(ToggleState toggleState, bool newValue)
    {
        if(!CheckApplicationIsPlaying())
        {
            return;
        }

        bool previousValue = toggleState.value;

        toggleState.value = newValue;

        if (newValue != previousValue)
        {
            toggleState.ValueChanged();
        }
    }   
    
    private static bool ReadToggleState(ToggleState toggleState)
    {
        if(!CheckApplicationIsPlaying()) return toggleState.DefaultValue;

        return toggleState.value;
    }

    private static bool CheckApplicationIsPlaying()
    {
        bool result = Application.isPlaying;
        if (!result)
        {
            Debug.LogWarning("Tried to access Game State in edit mode. Will use default value for state.");
        }

        return result;
    }
}
