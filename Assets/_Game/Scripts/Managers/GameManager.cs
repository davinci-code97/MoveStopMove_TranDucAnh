using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { INTRO, MAIN_MENU, PLAYING, PAUSED, GAME_OVER }
    public GameState currentState { get; private set; }

    private float gamePlayingTimer;
    //private float gamePlayingTimerMax = 10f;

    private void Awake() {
        Instance = this;
        currentState = GameState.INTRO;
    }

    void Start()
    {
        //UIManager.Instance.LoadUI("MainMenu");
    }

    void Update()
    {
        switch (currentState) {
            case GameState.INTRO:
                //UIManager.Instance.LoadUI("Intro");
                break;
            case GameState.MAIN_MENU:
                break;
            case GameState.PLAYING:
                break;
            case GameState.PAUSED:
                break;
            case GameState.GAME_OVER:
                break;
        }
    }

    public void SetGameState(GameState newState) {
        currentState = newState;
        
    }

}
