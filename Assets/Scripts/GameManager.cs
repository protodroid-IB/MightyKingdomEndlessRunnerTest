﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UniRx;
using UnityEngine;
using UnityEngine.Events;


public enum GameDifficulty
{
    VerySlow,
    Slow,
    Normal,
    Fast,
    VeryFast,
    VeryVeryFast,
    ExtremleyFast,
    InsanelyFast,
    Stop
}

public enum GameState
{
    Menu,
    Game
}

public class GameManager : MonoBehaviour
{
    #region  singleton

    public static GameManager instance;

    private void Awake()
    {
        if (FindObjectsOfType<GameManager>().Length > 1)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    #endregion

    [SerializeField]
    private ReactiveProperty<GameState> state = new ReactiveProperty<GameState>(GameState.Menu);
    public ReactiveProperty<GameState> State
    {
        get => state;
        set
        {
            state = value;
            onGameStateChange?.Invoke();
        }
    }

    [Space(10)]

    // difficulty events and variables
    public UnityEvent onStartGame, onDifficultyChange, onStopGame, onResetGame, onGameStateChange;

    private GameDifficulty difficulty = GameDifficulty.Stop;
    public GameDifficulty Difficulty
    {
        get => difficulty;
        set
        {
            difficulty = value;
            ChangeDifficulty();
        }
    }

    // this controls the speed of all repeating objects in game
    public float GameSpeed { get; private set; }




    // calls the change difficulty event and changes the overall game speed
    private void ChangeDifficulty()
    {
        switch (Difficulty)
        {
            case GameDifficulty.VerySlow:
                GameSpeed = 0.5f;
                break;

            case GameDifficulty.Slow:
                GameSpeed = 0.65f;
                break;

            case GameDifficulty.Normal:
                GameSpeed = 0.75f;
                break;

            case GameDifficulty.Fast:
                GameSpeed = 0.9f;
                break;

            case GameDifficulty.VeryFast:
                GameSpeed = 1.0f;
                break;

            case GameDifficulty.VeryVeryFast:
                GameSpeed = 1.0f;
                break;

            case GameDifficulty.ExtremleyFast:
                GameSpeed = 1.1f;
                break;

            case GameDifficulty.InsanelyFast:
                GameSpeed = 1.2f;
                break;

            case GameDifficulty.Stop:
                GameSpeed = 0.0f;
                break;
        }

        onDifficultyChange?.Invoke();
    }


    // starts the game
    [ContextMenu("Start Game")]
    public void StartGame()
    {
        Difficulty = GameDifficulty.VerySlow;

        onStartGame?.Invoke();
    }

    [ContextMenu("Stop Game")]
    public void StopGame()
    {
        Difficulty = GameDifficulty.Stop;

        onStopGame?.Invoke();
    }


    [ContextMenu("Reset Game")]
    public void ResetGame()
    {
        onResetGame?.Invoke();
    }

    [ContextMenu("To Next Difficulty")]
    public void ToNextDifficulty()
    {
        Difficulty = (GameDifficulty)(((int) Difficulty + 1) % (int)GameDifficulty.Stop);
    }

    [ContextMenu("Change Between Game States")]
    public void ChangeState()
    {
        if (State.Value == GameState.Game)
            State.Value = GameState.Menu;

        else if (State.Value == GameState.Menu)
            State.Value = GameState.Game;
    }



    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.R))
    //    {
    //        StopGame();
    //        ResetGame();
    //    }

    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        StartGame();
    //    }

    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        ToNextDifficulty();
    //    }
    //}

}
