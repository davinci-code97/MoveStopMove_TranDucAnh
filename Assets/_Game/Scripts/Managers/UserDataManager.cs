using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    public static string KeySaveData = "KeySaveData";

    public static UserDataManager Instance { get; private set; }

    public ShopDataSO shopData;
    public UserData userData;

    private void Awake() {
        Instance = this;
        if (string.IsNullOrEmpty(PlayerPrefs.GetString(KeySaveData))) {
            userData = new();
        } else {
            userData = JsonUtility.FromJson<UserData>(PlayerPrefs.GetString(KeySaveData));
        }

    }

    public void SaveData() {
        string saveUserData = JsonUtility.ToJson(userData);
        PlayerPrefs.SetString(KeySaveData, saveUserData);
    }

    public void SetPurchasedWeapons(WeaponType weaponType) {
        if (userData.weaponData.purchasedWeaponsList.Contains(weaponType)) return;
        userData.weaponData.purchasedWeaponsList.Add(weaponType);
        SaveData();
    }

    public void SetPurchasedHats(HatType hatType) {
        if (userData.hatData.purchasedHatsList.Contains(hatType)) return;
        userData.hatData.purchasedHatsList.Add(hatType);
        SaveData();
    }

    public void SetLevel(int lvl) {
        if (lvl >= LevelManager.Instance.LevelConfigList.Count) return;
        userData.level = lvl;
        SaveData();
    }

    public int GetCurrentLevel() {
        return userData.level;
    }
    
    public int GetCurrentLevelRank() {
        return userData.rank;
    }

    public WeaponType GetCurrentWeaponPoolType() {
        return userData.weaponData.equippedWeapon;
    }

    public void SetCurrentWeaponPoolType(WeaponType weaponType) {
        userData.weaponData.equippedWeapon = weaponType;
        SaveData();
    }

    public HatType GetCurrentHatPoolType() {
        return userData.hatData.equippedHat;
    }
    public void SetCurrentHatPoolType(HatType hatType) {
        userData.hatData.equippedHat = hatType;
        SaveData();

    }

    public PantsType GetCurrentPantsType() {
        return userData.pantsData.equippedPants;
    }

    public void SetCurrentPantsType(PantsType pantsType) {
        userData.pantsData.equippedPants = pantsType;
        SaveData();

    }

    public float GetPlayerGold() {
        return userData.gold;
    }

    public void SetPlayerGold(float changeNumber) {
        userData.gold += changeNumber;
        SaveData();
    }

    public bool IsPurchaseable(float cost) {
        return cost > userData.gold;
    }

    public bool CheckPurchasedItem(ShopItem item) {
        return item switch {
            ShopItem<WeaponType> weapon => IsWeaponPurchased(weapon.type),
            ShopItem<HatType> hat => IsHatPurchased(hat.type),
            ShopItem<PantsType> pant => IsPantPurchased(pant.type),
            //ShopItemData<FullSetType> fullSet => IsFullSetPurchased(fullSet.type),
            _ => false
        };
       
    }

    public bool IsItemEquipped(ShopItem item) {
        return item switch {
            ShopItem<WeaponType> weapon => IsWeaponEquipped(weapon.type),
            ShopItem<HatType> hat => IsHatEquipped(hat.type),
            ShopItem<PantsType> pant => IsPantEquipped(pant.type),
            //ShopItemData<FullSetType> fullSet => IsFullSetEquipped(fullSet.type),
            _ => false
        };
    }

    public void EquipShopItem(ShopItem item) {
        switch (item) {
            case ShopItem<WeaponType> weapon:
                userData.weaponData.equippedWeapon = weapon.type;
                break;
            case ShopItem<HatType> hat:
                userData.hatData.equippedHat = hat.type;
                break;
            case ShopItem<PantsType> pant:
                userData.pantsData.equippedPants = pant.type;
                break;
            //case ShopItemData<FullSetType> fullSet:
            //    userData.fullSetEquipped = fullSet.type;
            //    break;
            default:
                return;
        }

        SaveData();
    }

    public void PurchaseShopItem(ShopItem item) {
        userData.gold -= item.price;

        switch (item) {
            case ShopItem<WeaponType> weapon:
                userData.weaponData.purchasedWeaponsList.Add(weapon.type);
                break;
            case ShopItem<HatType> hat:
                userData.hatData.purchasedHatsList.Add(hat.type);
                break;
            case ShopItem<PantsType> pant:
                userData.pantsData.purchasedPantsList.Add(pant.type);
                break;
            //case ShopItemData<FullSetType> fullSet:
            //    userData.fullSetPurchasedList.Add(fullSet.type);
            //    break;
            default:
                return;
        }

        SaveData();
    }

    private bool IsWeaponPurchased(WeaponType type) {
        return userData.weaponData.purchasedWeaponsList.Contains(type);
    }

    private bool IsWeaponEquipped(WeaponType type) {
        return userData.weaponData.equippedWeapon == type;
    }

    private bool IsHatPurchased(HatType type) {
        return userData.hatData.purchasedHatsList.Contains(type);
    }

    private bool IsHatEquipped(HatType type) {
        return userData.hatData.equippedHat == type;
    }

    private bool IsPantPurchased(PantsType type) {
        return userData.pantsData.purchasedPantsList.Contains(type);
    }

    private bool IsPantEquipped(PantsType type) {
        return userData.pantsData.equippedPants == type;
    }

}

[Serializable]
public class UserData {
    public SaveItemWeaponData weaponData;
    public SaveItemHatData hatData;
    public SaveItemPantsData pantsData;

    public int level;
    public int rank;
    public float gold;

    public float musicVolume = 0.3f;
    public float sfxVolume = 1f;

    public UserData() {
        level = 1;
        rank = 50;
        gold = 100;
        weaponData = new();
        hatData = new();
        pantsData = new();
    } 
}

[Serializable]
public class SaveItemWeaponData {
    public List<WeaponType> purchasedWeaponsList = new();
    public WeaponType equippedWeapon;

    public SaveItemWeaponData() {
        purchasedWeaponsList.Add(WeaponType.Hammer);
        equippedWeapon = WeaponType.Hammer;
    }
}

[Serializable]
public class SaveItemHatData 
{
    public List<HatType> purchasedHatsList = new();
    public HatType equippedHat;

    public SaveItemHatData() {
        purchasedHatsList.Add(HatType.Hat_Arrow);
        equippedHat = HatType.None;
    }
}

[Serializable]
public class SaveItemPantsData 
{
    public List<PantsType> purchasedPantsList = new();
    public PantsType equippedPants;

    public SaveItemPantsData() {
        purchasedPantsList.Add(PantsType.Pants_Batman);
        equippedPants = PantsType.None;
    }
}
