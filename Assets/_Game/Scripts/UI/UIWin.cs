using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWin : UICanvas
{
    // TODO: OnWinLevel Event

    [SerializeField] private Button homeButton; 
    [SerializeField] private TMP_Text playerGoldEarnedText;

    private void OnEnable() {
        homeButton.onClick.AddListener(() => {
            BackToMainMenu();
        });
        playerGoldEarnedText.SetText(Player.Instance.GetCharacterGoldValue().ToString());
    }

    private void OnDisable() {
        homeButton.onClick.RemoveListener(BackToMainMenu);
    }

    private void BackToMainMenu() {
        SoundManager.Instance.PlayButtonClickSFX();
        UserDataManager.Instance.SetLevel(UserDataManager.Instance.GetCurrentLevel()+1);
        LevelManager.Instance.ResetLevel();
        LevelManager.Instance.OnInit();
        GameManager.Instance.SetGameState(GameState.MAIN_MENU);
    }

}
