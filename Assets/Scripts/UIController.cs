using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [Serializable]
    public class ScreenUI
    {
        public GameObject parentGO;
        public GameState gameState;
        public Animator animator;
    }

    [SerializeField]
    private List<ScreenUI> screens;

    private CameraController camController;


    private void Start()
    {
        camController = FindObjectOfType<CameraController>();

        ScreenUI initialScreen = screens
            .First(inScreen => inScreen.gameState == GameManager.instance.State.Value);

        initialScreen.animator.SetBool("Enter", true);

        camController.onEnterGame.AddListener(() =>
        {
            ScreenUI screen = screens
                .First(inScreen => inScreen.gameState == GameState.Game);

            screen?.animator.SetBool("Enter", true);
        });

        camController.onExitGame.AddListener(() =>
        {
            ScreenUI screen = screens
                .First(inScreen => inScreen.gameState == GameState.Game);

            screen?.animator.SetBool("Enter", false);
        });


        camController.onEnterMenu.AddListener(() =>
        {
            ScreenUI screen = screens
                .First(inScreen => inScreen.gameState == GameState.Menu);

            screen?.animator.SetBool("Enter", true);
        });

        camController.onExitMenu.AddListener(() =>
        {
            ScreenUI screen = screens
                .First(inScreen => inScreen.gameState == GameState.Menu);

            screen?.animator.SetBool("Enter", false);
        });
    }
}
