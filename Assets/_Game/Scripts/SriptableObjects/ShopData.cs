using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopData", menuName = "Scriptable Object/Shop Data")]
public class ShopData : ScriptableObject {
    public ShopItemDataList<WeaponType> weaponsData;
    public ShopItemDataList<HatType> hatsData;
    public ShopItemDataList<PantsType> pantsData;

    public ShopItemData<WeaponType> GetWeaponsData(WeaponType weaponType) {
        for (int i = 0; i < weaponsData.itemsData.Count; i++) {
            if (weaponsData.itemsData[i].type == weaponType) {
                return weaponsData.itemsData[i];
            }
        }
        return null;
    }

    public ShopItemData<PantsType> GetPantsData(PantsType pantsType) {
        for (int i = 0; i < pantsData.itemsData.Count; i++) {
            if (pantsData.itemsData[i].type == pantsType) {
                return pantsData.itemsData[i];
            }
        }
        return null;
    }
    
    public ShopItemData<HatType> GetHatData(HatType hatType) {
        for (int i = 0; i < hatsData.itemsData.Count; i++) {
            if (hatsData.itemsData[i].type == hatType) {
                return hatsData.itemsData[i];
            }
        }
        return null;
    }


}
