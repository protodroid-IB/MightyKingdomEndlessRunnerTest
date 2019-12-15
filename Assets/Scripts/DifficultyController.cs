using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class DifficultyController : MonoBehaviour
{
    [System.Serializable]
    public class DifficultyInterval
    {
        public GameDifficulty difficulty = GameDifficulty.VerySlow;
        public int meterInterval = 20;
    }

    private int intervalDistance = 0;

    [SerializeField] private List<DifficultyInterval> difficultyIntervals;

    private int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        ScoringManager.instance.distanceRun
            .Where(distance => GameManager.instance.Difficulty != GameDifficulty.InsanelyFast)
            .Subscribe(distance =>
            {
                intervalDistance++;

                if (intervalDistance == difficultyIntervals[index].meterInterval)
                {
                    index++;
                    GameManager.instance.ToNextDifficulty();
                    intervalDistance = 0;
                }
                
            });

        GameManager.instance.onStartGame.AddListener(() => 
        {
            index = 0;
            intervalDistance = 0;
        });
    }
}
