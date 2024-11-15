using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : UICanvas
{

    [SerializeField] private Button closeShopBtn;
    [SerializeField] private TMP_Text playerGoldText;

    private void OnEnable() {
        closeShopBtn.onClick.AddListener(CloseShop);
        playerGoldText.SetText(UserDataManager.Instance.GetPlayerGold().ToString());
    }

    private void OnDisable() {
        closeShopBtn.onClick.RemoveAllListeners();
    }

    private void CloseShop() {
        SoundManager.Instance.PlayButtonClickSFX();
        GameManager.Instance.SetGameState(GameState.MAIN_MENU);
        Player.Instance.OnInit();
    }


}
