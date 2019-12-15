using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Coin : PickUp
{
    public UnityEvent onDestroy;

    private SpriteRenderer sr;

    protected override void Awake()
    {
        base.Awake();

        sr = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void OnPickUp()
    {
        base.OnPickUp();

        ScoringManager.instance.IncrementCoins();
        Kill();

        if(sr)
            sr.enabled = false;
    }

    private void OnDestroy()
    {
        onDestroy?.Invoke();
    }
}
