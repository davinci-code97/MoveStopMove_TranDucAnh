using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopItem : MonoBehaviour
{
    public static event Action<UIShopItem> OnShopItemSelected;

    public ShopItem shopItem;

    public bool isPurchased = false;
    public bool isEquipped = false;

    [SerializeField] private Button btnSelect;
    [SerializeField] private Image imageIcon;
    [SerializeField] private Image imageLock;
    [SerializeField] private Image imageHighlight;

    private void OnEnable() {
        btnSelect.onClick.AddListener(OnClickPreview);
    }

    private void OnDisable() {
        btnSelect.onClick.RemoveListener(OnClickPreview);
    }

    public void Setup(ShopItem _shopItem) {
        shopItem = _shopItem;
        if (UserDataManager.Instance.CheckPurchasedItem(shopItem)) {
            isPurchased = true;
            imageLock.gameObject.SetActive(false);

            if (UserDataManager.Instance.IsItemEquipped(shopItem)) {
                OnClickPreview();
                EquipShopItem();
            }

        }
        imageIcon.sprite = shopItem.sprite;
    }

    private void OnClickPreview() {
        switch (shopItem) {
            case ShopItem<WeaponType> weapon:
                WeaponConfig weaponConfig = LevelManager.Instance.GetWeaponConfigByType(weapon.type);
                Player.Instance.SetUpWeapon(weaponConfig);
                break;

            case ShopItem<HatType> hat:
                HatConfig hatConfig = LevelManager.Instance.GetHatConfigByType(hat.type);
                Player.Instance.SetUpHat(hatConfig);
                break;

            case ShopItem<PantsType> pant:
                PantsConfig pantsConfig = LevelManager.Instance.GetPantsConfigByType(pant.type);
                Player.Instance.SetUpPants(pantsConfig);
                break;

            case ShopItem<FullSetType> fullSet:
                
                break;

            default:
                Debug.LogWarning("Missing item type.");
                break;
        }

        imageHighlight.gameObject.SetActive(true);
        btnSelect.enabled = false;
        SoundManager.Instance.PlayButtonClickSFX();
        OnShopItemSelected?.Invoke(this);
    }

    public void PurchaseShopItem() {
        UserDataManager.Instance.PurchaseShopItem(shopItem);
        isPurchased = true;
        imageLock.gameObject.SetActive(false);
    }

    public void EquipShopItem() {
        isEquipped = true;
        UserDataManager.Instance.EquipShopItem(shopItem);
    }

    public void UnEquipShopItem() {
        isEquipped = false;
    }

    private void SetLockIcon(bool locked) {
        imageLock.gameObject.SetActive(locked);
    }

    public void UnSelectItem() {
        imageHighlight.gameObject.SetActive(false);
        btnSelect.enabled = true;
    }
}
