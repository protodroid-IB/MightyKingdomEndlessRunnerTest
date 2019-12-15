using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class MenuScoresView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI topScore, lastScore;

    private void Start()
    {
        ScoringManager.instance.topScore
            .Subscribe(score => { topScore.text = score.ToString(); });

        ScoringManager.instance.currentScore
            .Subscribe(score => { lastScore.text = score.ToString(); });
    }
}
