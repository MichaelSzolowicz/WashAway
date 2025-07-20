using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState
{
    private static bool paused = false;

    public static bool  Paused { get { return paused; } set { paused = value; } }

    public static void Pause()
    {

    }
}
