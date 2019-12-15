using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class ScoringManager : MonoBehaviour
{
    #region  singleton

    public static ScoringManager instance;

    private void Awake()
    {
        if (FindObjectsOfType<ScoringManager>().Length > 1)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    #endregion

    // scores
    private float topScore = 0f;
    private float currentScore = 0f;

    // distance and coins
    public IntReactiveProperty distanceRun = new IntReactiveProperty(0);
    public IntReactiveProperty totalCoins = new IntReactiveProperty(0);

    // disposables
    private IDisposable distanceTracker;

    // timing for distance increments
    [SerializeField]
    private float oneMeterTime = 0.5f;
    private float meterTimer = 0f;

    // the scriptable object asset to store the top score data in
    [SerializeField]
    private TopScore topScoreData;



    void Start()
    {
        // grab the top score data
        topScore = topScoreData.topScore;

        // apply subscritions
        GameManager.instance.onStartGame.AddListener(StartTracking);
        GameManager.instance.onStopGame.AddListener(StopTracking);
        GameManager.instance.onResetGame.AddListener(ResetScore);
    }


    // resets all the scoring variables
    private void ResetScore()
    {
        currentScore = 0f;
        distanceRun.Value = 0;
        totalCoins.Value = 0;
    }



    // when the game starts, start tracking distance
    private void StartTracking()
    {
        distanceTracker?.Dispose();

        distanceTracker = this.UpdateAsObservable()
            .Subscribe(_ => TrackDistance());

        meterTimer = 0f;
    }



    // when the game ends stop tracking distance
    private void StopTracking()
    {
        distanceTracker?.Dispose();

        CalculateScore(distanceRun.Value, totalCoins.Value);

        if (currentScore > topScore)
        {
            topScore = currentScore;
            topScoreData.topScore = topScore;
        }
    }


    // handles the distance tracking, tied to the game speed, increments faster when gamespeed is faster
    private void TrackDistance()
    {
        if (meterTimer >= (oneMeterTime / GameManager.instance.GameSpeed))
        {
            meterTimer = 0f;
            distanceRun.Value++;
        }

        meterTimer += Time.deltaTime;
    }


    // calculates the score from the distance and the coins
    private void CalculateScore(int inDistance, int inCoins)
    {
        currentScore = inDistance + inCoins;
    }


    public void IncrementCoins()
    {
        totalCoins.Value++;
    }
}
