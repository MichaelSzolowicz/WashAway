using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameRules", menuName = "ScriptableObjects/GameRules")]
public class GameRules : ScriptableObject
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject pauseScreen;

    public GameObject PauseScreen { get { return pauseScreen; } }
    public GameObject Canvas { get { return canvas; } }
}
