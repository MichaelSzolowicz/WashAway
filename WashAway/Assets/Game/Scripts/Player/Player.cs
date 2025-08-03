using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    void Update()
    {
        if (
            Input.GetKeyDown(KeyCode.Escape)
            && !GameState.CurrentLevelClear
            )
        {
            GameState.Paused = !GameState.Paused;
        }
    }
}
