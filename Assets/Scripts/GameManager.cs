using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;


public enum GameDifficulty
{
    VerySlow,
    Slow,
    Normal,
    Fast,
    VeryFast,
    Stop
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

    // difficulty events and variables
    public UnityEvent onStartGame, onDifficultyChange, onStopGame;

    private GameDifficulty difficulty = GameDifficulty.VerySlow;
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
                GameSpeed = 0.2f;
                break;

            case GameDifficulty.Slow:
                GameSpeed = 0.4f;
                break;

            case GameDifficulty.Normal:
                GameSpeed = 0.6f;
                break;

            case GameDifficulty.Fast:
                GameSpeed = 0.8f;
                break;

            case GameDifficulty.VeryFast:
                GameSpeed = 1.0f;
                break;

            case GameDifficulty.Stop:
                GameSpeed = 0.0f;
                break;
        }

        onDifficultyChange?.Invoke();
    }


    // starts the game
    [ContextMenu("Start Game")]
    private void StartGame()
    {
        Difficulty = GameDifficulty.VerySlow;

        onStartGame?.Invoke();
    }

    [ContextMenu("Stop Game")]
    private void StopGame()
    {
        Difficulty = GameDifficulty.Stop;

        onStopGame?.Invoke();
    }

    [ContextMenu("To Next Difficulty")]
    public void ToNextDifficulty()
    {
        Difficulty = (GameDifficulty)(((int) Difficulty + 1) % (int)GameDifficulty.Stop);
    }
}
