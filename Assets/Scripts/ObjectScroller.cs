using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class ObjectScroller : MonoBehaviour
{
    private IDisposable scrollingSub;
    private IDisposable speedChangeSub;
    private IDisposable killSub;

    [SerializeField]
    private Rigidbody2D rb;

    public float MaxSpeed { get; set; }
    private float speed;

    [SerializeField]
    [Range(0.005f, 10f)]
    private float acceleration = 2f;
    private float speedLerpValue = 0;

    private Collider2D thisCollider;
    private Collider2D spawnerCollider;

    private void Start()
    {
        // if there is no game manager do not make subscriptions to events
        if (!GameManager.instance)
            return;

        if(!rb)
            rb = GetComponent<Rigidbody2D>();

        thisCollider = GetComponent<Collider2D>();

        GameManager.instance.onStartGame.AddListener(StartScroll);
        GameManager.instance.onDifficultyChange.AddListener(ChangeSpeed);
        GameManager.instance.onStopGame.AddListener(StopScroll);
    }

    public void StartScroll()
    {
        ChangeSpeed();

        if (!rb)
        {
            // translate the gameobject via transform
            scrollingSub = this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    transform.position -= new Vector3(speed * Time.deltaTime, 0f, 0f);
                });
        }
        else
        {
            // translate the gameobject via rigidbody
            scrollingSub = this.FixedUpdateAsObservable()
                .Subscribe(_ =>
                {
                    rb.velocity = new Vector2(-speed, 0f);
                });
        }

        killSub = this.UpdateAsObservable()
            .Subscribe(_ => CheckInBounds());

    }

    private void ChangeSpeed()
    {
        speedChangeSub?.Dispose();
        speedLerpValue = 0;
        float initialSpeed = speed;
        float finalSpeed = MaxSpeed * GameManager.instance.GameSpeed;

        // lerp the speed and dispose of this subscription when speed has been reached
        speedChangeSub = this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                speedLerpValue += Time.deltaTime * acceleration;
                speedLerpValue = Mathf.Clamp01(speedLerpValue);
                speed = Mathf.Lerp(initialSpeed, finalSpeed, speedLerpValue);

                if (speedLerpValue >= 1)
                    speedChangeSub.Dispose();
            });
    }

    private void StopScroll()
    {
        speedChangeSub?.Dispose();
        speedLerpValue = 0;

        float initialSpeed = speed;
        float finalSpeed = 0;

        speedChangeSub = this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                speedLerpValue += Time.deltaTime * acceleration;
                speedLerpValue = Mathf.Clamp01(speedLerpValue);
                speed = Mathf.Lerp(initialSpeed, finalSpeed, speedLerpValue);

                if (speedLerpValue >= 1)
                {
                    if(rb)
                        rb.velocity = new Vector2(0f, 0f);

                    speedChangeSub.Dispose();
                    scrollingSub?.Dispose();
                }
            });
    }

    private void CheckInBounds()
    {
        if (spawnerCollider.bounds.Intersects(thisCollider.bounds))
            return;

        killSub?.Dispose();
        speedChangeSub?.Dispose();
        scrollingSub?.Dispose();
        Destroy(gameObject);
    }

    public void SetScrollingObject(float maxSpeed, Collider2D lifespanCollider)
    {
        MaxSpeed = maxSpeed;
        spawnerCollider = lifespanCollider;
    }
}
