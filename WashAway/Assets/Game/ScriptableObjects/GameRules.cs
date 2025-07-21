using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameRules", menuName = "ScriptableObjects/GameRules")]
public class GameRules : ScriptableObject
{
    [SerializeField] private GameObject pauseScreen;

    public GameObject PauseScreen { get { return pauseScreen; } }

    public delegate void OnValidateGameRules();
    public event OnValidateGameRules onValidateGameRules;

    private void OnValidate()
    {
        Debug.Log("Validate " + name);

        onValidateGameRules?.Invoke();
    }
}
