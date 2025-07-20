using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameRules", menuName = "ScriptableObjects/GameRules")]
public class GameRules : ScriptableObject
{
    [SerializeField] private GameObject pauseScreen;

    public GameObject PauseScreen { get { return pauseScreen; } }
}
