using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : UICanvas
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private TMP_Text playerGoldText;
    [SerializeField] private TMP_Text playerProgressText;

    private void OnEnable() {
        playButton.onClick.AddListener(() => {
            PlayGame();
        });

        shopButton.onClick.AddListener(() => {
            OpenShop();
        });

        playerGoldText.text = UserDataManager.Instance.GetPlayerGold().ToString();
        playerProgressText.text = $"ZONE:{UserDataManager.Instance.GetCurrentLevel()}";
        //  - BEST:#{UserDataManager.Instance.GetCurrentLevelRank()}
        Player.Instance.OnInit();
    }

    private void OnDisable() {
        playButton.onClick.RemoveAllListeners();
        shopButton.onClick.RemoveAllListeners();
    }

    private void PlayGame() {
        SoundManager.Instance.PlayButtonClickSFX();
        GameManager.Instance.SetGameState(GameState.PLAYING);

    }

    private void OpenShop() {
        SoundManager.Instance.PlayButtonClickSFX();
        GameManager.Instance.SetGameState(GameState.SHOPPING);

    }
}
