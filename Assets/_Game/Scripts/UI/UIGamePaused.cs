using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePaused : UICanvas
{
    [SerializeField] private Button toggleMusicBtn;
    [SerializeField] private Button toggleSFXBtn;
    [SerializeField] private Button toggleVibrationBtn;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button continueButton;

    private void OnEnable() {
        homeButton.onClick.AddListener(() => {
            BackToMainMenu();
        });

        continueButton.onClick.AddListener(() => {
            ContinuePlaying();
        });

    }

    private void OnDisable() {
        homeButton.onClick.RemoveAllListeners();
        continueButton.onClick.RemoveAllListeners();
    }

    private void BackToMainMenu() {
        SoundManager.Instance.PlayButtonClickSFX();
        Close(0);
        UIManager.Instance.OpenUI<UIMainMenu>();
        LevelManager.Instance.ResetLevel();
        GameManager.Instance.SetGameState(GameState.MAIN_MENU);
    }

    private void ContinuePlaying() {
        SoundManager.Instance.PlayButtonClickSFX();
        Close(0);
        UIManager.Instance.OpenUI<UIGamePlaying>();
        GameManager.Instance.SetGameState(GameState.PLAYING);
    }


}
