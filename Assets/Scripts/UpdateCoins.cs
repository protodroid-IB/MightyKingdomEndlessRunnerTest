using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class UpdateCoins : MonoBehaviour
{

    private TextMeshProUGUI coinsUI;

    void Start()
    {
        coinsUI = GetComponent<TextMeshProUGUI>();

        // when the score is changed, update the UI
        ScoringManager.instance.totalCoins
            .Subscribe(coins => { coinsUI.text = $"{coins}"; });
    }
}
