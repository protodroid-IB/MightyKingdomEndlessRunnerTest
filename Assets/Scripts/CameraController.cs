using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    private Camera cam;
    private Animator animator;

    public UnityEvent onEnterMenu, onEnterGame, onExitMenu, onExitGame;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        animator = GetComponent<Animator>();

        GameManager.instance.State
            .Subscribe(AnimateCamera);

       onEnterGame.AddListener(GameManager.instance.StartGame);
       onEnterMenu.AddListener(GameManager.instance.ResetGame);
    }

    private void AnimateCamera(GameState inState)
    {
        switch (inState)
        {
            case GameState.Game:
                animator.SetBool("ToGame", true);
                break;

            case GameState.Menu:
                animator.SetBool("ToGame", false);
                break;
        }
    }

    public void AtMainMenu()
    {
        onEnterMenu?.Invoke();
    }

    public void AtGame()
    {
        onEnterGame?.Invoke();
    }

    public void ExitMenu()
    {
        onExitMenu?.Invoke();
    }

    public void ExitGame()
    {
        onExitGame?.Invoke();
    }
}
