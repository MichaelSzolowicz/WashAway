using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ViewGameState : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        GameState.Paused = GameState.Paused;
    }
}
