using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UpdateDistance : MonoBehaviour
{
    private TextMeshProUGUI distanceUI;

    void Start()
    {
        distanceUI = GetComponent<TextMeshProUGUI>();


        // when the score is changed, update the UI
        ScoringManager.instance.distanceRun
            .Subscribe(distance => { distanceUI.text = $"{distance} meters"; });
    }


}
