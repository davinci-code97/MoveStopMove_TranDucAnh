using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkinShop : MonoBehaviour
{
    public enum UISkinShopStatus {
        None = 0,
        Purchased = 1,
        NotPurchased = 2,
    }
    [SerializeField] private Button btnSelect;
    [SerializeField] private Image imageIcon;
    [SerializeField] private Image imageLock;
    [SerializeField] protected UISkinShopStatus status;

    private void Start() {
        btnSelect.onClick.AddListener(OnClickPreview);
    }

    private void OnClickPreview() {
        
    }

    public void SetUp(ShopItemData<PoolType> itemData) {
        if (UserDataManager.Instance.CheckPurchasedItem(itemData.type)) {
            SetStatus(UISkinShopStatus.Purchased);
        }
    }

    private void SetStatus(UISkinShopStatus status) {
        switch (status) {
            case UISkinShopStatus.None:
                break;
            case UISkinShopStatus.Purchased:
                SetUISkinShopStatusPurchased();
                break;
            case UISkinShopStatus.NotPurchased:
                break;
        }
    }

    private void SetUISkinShopStatusPurchased() {
        imageLock.enabled = true;
    }


}
