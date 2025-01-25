using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameState currentState;

    public static GameManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this; 
        DontDestroyOnLoad(gameObject); // Keep this object across scenes
    }

    private void OnMouseDown()
    {
        if (currentState == GameState.GameStart)
        {
            currentState = GameState.GameOn;
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case GameState.GameStart:
                Debug.Log("Game Starting...");
                break;

            case GameState.GameOn:
                Debug.Log("Game On!");
                Time.timeScale = 1;
                break;

            case GameState.GamePaused:
                Debug.Log("Game Paused.");
                Time.timeScale = 0; 
                break;

            case GameState.Win:
                Debug.Log("You Win!");
                break;

            case GameState.Fail:
                Debug.Log("You Fail!");
                break;
        }
    }

}

public enum GameState
{
    GameStart,
    GameOn,
    GamePaused,
    Win,
    Fail
}
