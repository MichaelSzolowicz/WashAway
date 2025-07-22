using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState
{
    public delegate void OnTogglePause();
    public static event OnTogglePause onTogglePause;

    private static bool paused = false;

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
}
