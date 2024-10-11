using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    public static string KeySaveData = "KeySaveData";

    public static UserDataManager Instance;

    public ShopData shopData;
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
        if (userData.weaponData.purchasedWeapons.Contains(weaponType)) return;
        userData.weaponData.purchasedWeapons.Add(weaponType);
    }

    public void SetPurchasedHats(HatType hatType) {
        if (userData.itemHatData.purchasedHats.Contains(hatType)) return;
        userData.itemHatData.purchasedHats.Add(hatType);
    }

    public void SetLevel(int lvl) {
        userData.level = lvl;
    }

    public int GetCurrentLevel() {
        return userData.level;
    }

    internal WeaponType GetCurrentWeaponPoolType() {
        return userData.weaponData.equippedWeapon;
    }

    public void SetCurrentWeaponPoolType(WeaponType weaponType) {
        userData.weaponData.equippedWeapon = weaponType;
    }

    public float GetPlayerGold() {
        return userData.gold;
    }

    public void IncreasePlayerGold(float number) {
        userData.gold += number;
    }
}

[Serializable]
public class UserData {
    public SaveItemWeaponData weaponData;
    public SaveItemHatData itemHatData;

    public int level;
    public float gold;

    public UserData() {
        level = 0;
        gold = 0;
        weaponData = new SaveItemWeaponData();
        itemHatData = new SaveItemHatData();
       
    } 
}

[Serializable]
public class SaveItemWeaponData {
    public List<WeaponType> purchasedWeapons = new();
    public WeaponType equippedWeapon;

    public SaveItemWeaponData() {
        purchasedWeapons.Add(WeaponType.Hammer);
        equippedWeapon = WeaponType.Hammer;
    }
}

[Serializable]
public class SaveItemHatData 
{
    public List<HatType> purchasedHats = new();
    public HatType equippedHat;

    public SaveItemHatData() {
        
    }
}
