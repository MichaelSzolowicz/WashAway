using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuickTest
{
    public class FinishLine : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            GameState.Paused = true;
        }
    }
}
