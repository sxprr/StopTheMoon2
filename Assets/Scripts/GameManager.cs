using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    // this script will manage the victory and gameover.

    public static GameManager instance;
    public GameState State;

    public UnityEvent OnGameStateChanged;

    // Do I need this? Let me focus on Events and Delegates
    public enum GameState
    {
        Victory,
        Lose
    }

    private void Awake()
    {
        instance = this;
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