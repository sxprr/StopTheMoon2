using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    // this script will manage the victory and gameover.

    public static GameManager instance;
    public GameState State;
    public UnityEvent OnGameStateChanged;

    // 1. Variable must be declared here (Class Scope)
    private bool victoryTriggered = false;
    public int ePressesToWin = 100;
    private int currentPresses = 0;

    private void OnEnable() => GameEvents.OnMoonResist += CheckProgress;
    private void OnDisable() => GameEvents.OnMoonResist -= CheckProgress;

    public enum GameState
    {
        Victory,
        Lose
    }

    private void Awake()
    {
        instance = this;
    }

    void CheckProgress()
    {
        if (victoryTriggered) return;

        currentPresses++;
        if (currentPresses >= ePressesToWin)
        {
            victoryTriggered = true;
            GameEvents.TriggerVictory(); // The big shout!
        }
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.Victory:
                break;
            case GameState.Lose:
                break;
        }

        
    }
        
}