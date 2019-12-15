using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using UniRx.Triggers;

public class PickUp : InteractableObject
{
    [SerializeField]
    protected float activeTime = 20f;
    public float ActiveTime { get => activeTime; protected set => activeTime = value; }

    [SerializeField]
    private Animator animator;

    private IDisposable endAnimSub;

    [Space(10)]
    public UnityEvent onPickUp;

    private float deathTime = 3f;

    private Collider2D collider;

    protected virtual void Awake()
    {
        collider = GetComponentInChildren<Collider2D>();
    }



    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);

        if (collider.tag == "Player")
        {
               OnPickUp();
        }
    }

    protected virtual void OnPickUp()
    {
        if(animator)
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Bob"))
            {
                animator.SetBool("Collected", true);
                endAnimSub = this.UpdateAsObservable()
                    .Subscribe(_ =>
                    {
                        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Collected") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                        {
                            Kill();
                        }
                    });
            }
        }

        onPickUp?.Invoke();
    }

    protected virtual void Kill()
    {
        endAnimSub?.Dispose();

        if(collider)
            collider.enabled = false;

        Destroy(gameObject, deathTime);
    }

}
