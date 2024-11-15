using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePlaying : UICanvas
{
    public static UIGamePlaying Instance { get; private set; }

    [SerializeField] private Button settingButton;
    [SerializeField] private TMP_Text botRemainNumber;
    [SerializeField] private TMP_Text playerScore;

    private void Awake() {
        Instance = this;
    }

    private void OnEnable() {
        settingButton.onClick.AddListener(OpenSetting);
        UpdatePlayerScore(0);
        botRemainNumber.SetText($"Alive: {LevelManager.Instance.botsRemain}");
    }

    private void OnDisable() {
        settingButton.onClick.RemoveListener(OpenSetting);
    }

    public void UpdateBotRemainsNumber(int number) {
        botRemainNumber.SetText($"Alive: {number}");
    }

    public void UpdatePlayerScore(int number) {
        playerScore.SetText(number.ToString());
    }

    private void OpenSetting() {
        SoundManager.Instance.PlayButtonClickSFX();
        Close(0);
        UIManager.Instance.OpenUI<UIGamePaused>();
        GameManager.Instance.SetGameState(GameState.PAUSED);
    }

}
