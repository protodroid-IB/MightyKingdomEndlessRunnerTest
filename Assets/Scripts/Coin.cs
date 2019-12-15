using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Coin : PickUp
{
    public UnityEvent onDestroy;

    protected override void OnPickUp()
    {
        base.OnPickUp();

        ScoringManager.instance.IncrementCoins();
        Kill();
    }

    private void OnDestroy()
    {
        onDestroy?.Invoke();
    }
}
