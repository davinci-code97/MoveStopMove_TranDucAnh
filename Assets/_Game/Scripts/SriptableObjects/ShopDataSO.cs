using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopDataSO", menuName = "Scriptable Object/Shop Data")]
public class ShopDataSO : ScriptableObject {
    // data list chua data
    public ShopItemDataList<WeaponType> shopWeaponDataList;
    public ShopItemDataList<HatType> shopHatDataList;
    public ShopItemDataList<PantsType> shopPantsDataList;

    // General method to get a specific item from a list based on its type
    public ShopItem<T> GetItem<T>(ShopItemDataList<T> list, T type) where T : Enum {
        return list.shopItemsDataList.FirstOrDefault(item => item.type.Equals(type));
    }

    public ShopItem<WeaponType> GetWeaponsData(WeaponType weaponType) {
        for (int i = 0; i < shopWeaponDataList.shopItemsDataList.Count; i++) {
            if (shopWeaponDataList.shopItemsDataList[i].type == weaponType) {
                return shopWeaponDataList.shopItemsDataList[i];
            }
        }
        return null;
    }

    public ShopItem<PantsType> GetPantsData(PantsType pantsType) {
        for (int i = 0; i < shopPantsDataList.shopItemsDataList.Count; i++) {
            if (shopPantsDataList.shopItemsDataList[i].type == pantsType) {
                return shopPantsDataList.shopItemsDataList[i];
            }
        }
        return null;
    }
    
    public ShopItem<HatType> GetHatData(HatType hatType) {
        for (int i = 0; i < shopHatDataList.shopItemsDataList.Count; i++) {
            if (shopHatDataList.shopItemsDataList[i].type == hatType) {
                return shopHatDataList.shopItemsDataList[i];
            }
        }
        return null;
    }


}
