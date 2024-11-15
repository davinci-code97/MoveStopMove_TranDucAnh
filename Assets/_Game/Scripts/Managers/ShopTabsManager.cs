using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class ShopTabsManager : MonoBehaviour
{
    public enum Tab {
        Weapon = 0,
        Hat = 1,
        Pants = 2,
        FullSet = 3
    }

    [SerializeField] private ShopDataSO shopData;
    [SerializeField] private Image[] tabImages;
    [SerializeField] private Image[] pages;
    [SerializeField] private Transform[] contentTFList;
    [SerializeField] private UIShopItem uiShopItemPrefab;

    [SerializeField] private TMP_Text playerGoldText;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button buyButton;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private TMP_Text buffDescText;

    [SerializeField] private UIShopItem currentShopItem;
    [SerializeField] private UIShopItem equipedShopItem;

    void Start() {
        SetupShopPages();
        ActiveTab(0);
    }

    private void OnEnable() {
        UIShopItem.OnShopItemSelected += UIShopItem_OnShopItemSelected;
        equipButton.onClick.AddListener(Equip);
        buyButton.onClick.AddListener(Purchase);
    }

    private void OnDisable() {
        UIShopItem.OnShopItemSelected -= UIShopItem_OnShopItemSelected;
        equipButton.onClick.RemoveAllListeners();
        buyButton.onClick.RemoveAllListeners();
    }

    private void UIShopItem_OnShopItemSelected(UIShopItem UIshopItem) {

        if (currentShopItem != null) {
            currentShopItem.UnSelectItem();
        }

        currentShopItem = UIshopItem;
        SetupShopItemOptions(UIshopItem);

        SoundManager.Instance.PlayButtonClickSFX();
    }

    private void SetupShopItemOptions(UIShopItem shopItemUI) {
        ShopItem shopItem = shopItemUI.shopItem;

        if (shopItemUI.isPurchased) {
            equipButton.gameObject.SetActive(true);
            buyButton.gameObject.SetActive(false);

            if (shopItemUI.isEquipped) {
                equipedShopItem = shopItemUI;
            }

        }
        else {
            equipButton.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(true);
            priceText.SetText(shopItem.price.ToString());
        }

        buffDescText.SetText(shopItem.setBuffDescription());
    }

    public void Purchase() {
        if (!UserDataManager.Instance.IsPurchaseable(currentShopItem.shopItem.price)) return;
        currentShopItem.PurchaseShopItem();
        SetupShopItemOptions(currentShopItem);
        UpdatePlayerGoldText();
        SoundManager.Instance.PlayButtonClickSFX();
    }

    public void Equip() {
        if (currentShopItem != equipedShopItem) {
            currentShopItem.EquipShopItem();
            if (equipedShopItem) {
                equipedShopItem.UnEquipShopItem();
            }
        }

        SetupShopItemOptions(currentShopItem);
        SetupShopItemOptions(equipedShopItem);

        SoundManager.Instance.PlayButtonClickSFX();
    }

    // Setup
    private void SetupShopPages() {
        SetUpWeaponShop();
        SetUpHatShop();
        SetUpPantsShop();
    }

    private void SetUpWeaponShop() {
        foreach (ShopItem<WeaponType> weaponItem in shopData.shopWeaponDataList.shopItemsDataList) {
            SetupShopItem(weaponItem, Tab.Weapon);
        }
    }

    private void SetUpHatShop() {
        foreach (ShopItem<HatType> hatItem in shopData.shopHatDataList.shopItemsDataList) {
            SetupShopItem(hatItem, Tab.Hat);
        }
    }

    private void SetUpPantsShop() {
        foreach (ShopItem<PantsType> pantsItem in shopData.shopPantsDataList.shopItemsDataList) {
            SetupShopItem(pantsItem, Tab.Pants);
        }
    }

    private void SetupShopItem(ShopItem _shopItem, Tab tab) {
        UIShopItem shopItemUIprefab = Instantiate(uiShopItemPrefab, contentTFList[(int)tab]);
        shopItemUIprefab.Setup(_shopItem);
    }

    public void ActiveTab(int tabNo) {
        for (int i = 0; i < tabImages.Length; i++) {
            pages[i].gameObject.SetActive(false);
            tabImages[i].color = Color.grey;
        }

        pages[tabNo].gameObject.SetActive(true);
        tabImages[tabNo].color = Color.white;
    }

    private void UpdatePlayerGoldText() {
        playerGoldText.SetText(UserDataManager.Instance.GetPlayerGold().ToString());
    }
}
