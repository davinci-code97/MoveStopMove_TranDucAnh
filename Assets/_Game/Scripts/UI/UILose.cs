using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILose : UICanvas
{
    [SerializeField] private Button reviveButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button retryButton;
    [SerializeField] private TMP_Text playerRankText;
    [SerializeField] private TMP_Text playerGoldEarnedText;


    private void OnEnable() {
        reviveButton.onClick.AddListener(() => {
            ContinuePlaying();
        });

        homeButton.onClick.AddListener(() => {
            BackToMainMenu();
        });

        retryButton.onClick.AddListener(RetryLevel);

        int playerRank = LevelManager.Instance.botsRemain + 1;
        playerRankText.SetText($"#{playerRank}");

        playerGoldEarnedText.SetText(Player.Instance.GetCharacterGoldValue().ToString());

    }


    private void OnDisable() {
        homeButton.onClick.RemoveListener(BackToMainMenu);
        reviveButton.onClick.RemoveListener(ContinuePlaying);
        retryButton.onClick.RemoveListener(RetryLevel);
    }

    private void BackToMainMenu() {
        UserDataManager.Instance.SetPlayerGold(Player.Instance.GetCharacterGoldValue());
        LevelManager.Instance.ResetLevel();
        GameManager.Instance.SetGameState(GameState.MAIN_MENU);
    }

    private void ContinuePlaying() {
        GameManager.Instance.SetGameState(GameState.PLAYING);
        Player.Instance.OnInit();
    }

    private void RetryLevel() {
        LevelManager.Instance.ResetLevel();
        GameManager.Instance.SetGameState(GameState.PLAYING);
    }

}
