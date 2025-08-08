using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState
{
    public delegate void OnStateChange();

    private static bool paused = false;
    public static event OnStateChange onTogglePause;
    public static bool  Paused 
    { 
        get 
        { 
            return paused; 
        } 
        set 
        {
            bool previousValue = paused;

            paused = value;

            if(previousValue != paused)
            {
                onTogglePause?.Invoke();
            }
        } 
    }

    private static bool currentLevelClear = false;
    public static event OnStateChange onToggleCurrentLevelClear;
    public static bool CurrentLevelClear
    {
        get { return currentLevelClear; }
        set
        {
            bool previousValue = currentLevelClear;

            currentLevelClear = value;

            if(previousValue != currentLevelClear)
            {
                onToggleCurrentLevelClear?.Invoke();
            }
        }
    }
}
