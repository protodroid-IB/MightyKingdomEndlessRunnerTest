using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class RepeatingBackground : MonoBehaviour
{
    private SpriteRenderer renderer;
    private Material mat;

    private float spriteWidth;

    [SerializeField]
    [Range(0.01f,10f)]
    private float maxSpeed = 1f;
    private float speed = 0f;

    [SerializeField]
    [Range(0.005f, 10f)]
    private float acceleration = 1f;
    private float speedLerpValue = 0;

    private IDisposable speedChangeSub;
    private IDisposable scrollingSub;




    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        mat = renderer.material;
        spriteWidth = renderer.sprite.rect.width;

        // if there is no game manager do not make subscriptions to events
        if (!GameManager.instance)
            return;

        GameManager.instance.onStartGame.AddListener(StartScroll);
        GameManager.instance.onDifficultyChange.AddListener(ChangeSpeed);
        GameManager.instance.onStopGame.AddListener(StopScroll);
    }


    private void StartScroll()
    {
        ChangeSpeed();

        scrollingSub = this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                float offset = mat.mainTextureOffset.x + Time.deltaTime * speed;

                if (offset >= spriteWidth)
                    offset = (offset - spriteWidth);

                mat.mainTextureOffset = new Vector2(offset, 0f);
            });
    }


    private void ChangeSpeed()
    {
        speedChangeSub?.Dispose();
        speedLerpValue = 0;
        float initialSpeed = speed;
        float finalSpeed = maxSpeed * GameManager.instance.GameSpeed;

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
                    speedChangeSub.Dispose();
            });

    }
}
