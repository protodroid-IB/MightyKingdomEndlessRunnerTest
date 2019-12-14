using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    #region  singleton

    public static InputManager instance;

    private void Awake()
    {
        if (FindObjectsOfType<InputManager>().Length > 1)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    #endregion

    public UnityEvent onTouchDown, onTouching, onTouchUp;

    // stores subscriptions for input detection
    private IDisposable touchDown, touching, touchUp;

    private void Start()
    {
        // if the game has started, enable controls
        GameManager.instance.onStartGame.AddListener(EnableGameControls);

        // if the game has ended, disable controls
        GameManager.instance.onStopGame.AddListener(DisableGameControls);
    }

    public void EnableGameControls()
    {
        // subscribe input detections, on update

        // is pressed down
        if(touchDown == null)
            touchDown = this.UpdateAsObservable()
                .Subscribe(_ => DetectTouchDown());

        // is pressed and held down
        if(touching == null)
            touching = this.UpdateAsObservable()
                .Subscribe(_ => DetectTouching());

        // is lifted up after pressing down
        if(touchUp == null)
            touchUp = this.UpdateAsObservable()
                .Subscribe(_ => DetectTouchUp());
    }

    public void DisableGameControls()
    {
        // dispose of all touch subscriptions

        touchDown?.Dispose();
        touching?.Dispose();
        touchUp?.Dispose();

        touchDown = null;
        touching = null;
        touchUp = null;
    }

    private void DetectTouchDown()
    {
        if(Input.GetMouseButtonDown(0))
            onTouchDown?.Invoke();
    }

    private void DetectTouching()
    {
        if (Input.GetMouseButton(0))
            onTouching?.Invoke();
    }

    private void DetectTouchUp()
    {
        if (Input.GetMouseButtonUp(0))
            onTouchUp?.Invoke();
    }
}
