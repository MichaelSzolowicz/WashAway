using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ViewGameState : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        GameState.StartScreenRead = GameState.StartScreenRead;
        GameState.Paused = GameState.Paused;
    }
}
