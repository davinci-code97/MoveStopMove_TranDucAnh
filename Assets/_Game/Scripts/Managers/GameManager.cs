using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public enum GameState { INTRO, MAIN_MENU, SHOPPING, PLAYING, PAUSED, WIN, LOSE }

public class GameManager : Singleton<GameManager>
{
    public GameState currentState { get; private set; }

    //private float gamePlayingTimer;
    //private float gamePlayingTimerMax;

    private bool isGamePaused;

    [SerializeField] private Joystick joystick;
    [SerializeField] private Canvas indicators;

    private void Awake() {
        Input.multiTouchEnabled = false;
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        //xu tai tho
        int maxScreenHeight = 1920;
        float ratio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        if (Screen.currentResolution.height > maxScreenHeight) {
            Screen.SetResolution(Mathf.RoundToInt(ratio * (float)maxScreenHeight), maxScreenHeight, true);
        }

    }

    void Start()
    {
        SetGameState(GameState.MAIN_MENU);

    }

    public void SetGameState(GameState newState) {
        currentState = newState;

        switch (currentState) {
            case GameState.INTRO:
                //UIManager.Instance.LoadUI("Intro");
                break;
            case GameState.MAIN_MENU:
                OnMainMenu();
                break;
            case GameState.SHOPPING:
                OnShopping();
                break;
            case GameState.PLAYING:
                OnGamePlaying();
                break;
            case GameState.PAUSED:
                OnGamePaused();
                break;
            case GameState.WIN:
                OnGameWin();
                break;
            case GameState.LOSE:
                OnGameLose();
                break;
        }

        if (currentState == GameState.PLAYING) {
            joystick.gameObject.SetActive(true);
            indicators.gameObject.SetActive(true);
        } else {
            //joystick.gameObject.SetActive(false);
            indicators.gameObject.SetActive(false);
        }

        if (currentState == GameState.PAUSED) {
            Time.timeScale = 0f;
        } else {
            Time.timeScale = 1f;
        }
    }

    private void OnMainMenu() {
        CameraManager.Instance.SwitchCamera(Constants.CAM_MAINMENU_INDEX);
        Player.Instance.ChangeAnim(Constants.ANIM_IDLE);
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<UIMainMenu>();
        MusicManager.Instance.PlayMenuMusic();
    }

    private void OnShopping() {
        CameraManager.Instance.SwitchCamera(Constants.CAM_SHOP_INDEX);
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<UIShop>();
        Player.Instance.OnWinGame();
    }

    private void OnGamePlaying() {
        CameraManager.Instance.SwitchCamera(Constants.CAM_GAMEPLAY_INDEX);
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<UIGamePlaying>();
        MusicManager.Instance.PlayGameplayMusic();
    }

    private void OnGamePaused() {
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<UIGamePaused>();
    }

    private void OnGameWin() {
        CameraManager.Instance.SwitchCamera(Constants.CAM_MAINMENU_INDEX);
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<UIWin>();
        MusicManager.Instance.PlayWinMusic();
    }

    private void OnGameLose() {
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<UILose>();
        MusicManager.Instance.PlayLoseMusic();
    }


}
