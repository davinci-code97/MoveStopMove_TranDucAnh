using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShopItemsData<T> where T : Enum {
    public List<ShopItemData<T>> itemsData;

}

[Serializable]
public class ShopItemData<T> where T : Enum
{
    public ItemConfig config;
    public Sprite sprite;
    public float price;
    public T type;
    public BuffType buffType;
    public float buffValue;
    public string buffDescription;

    public string setBuffDescription(string buffDescription) {
        switch (buffType) {
            case BuffType.AttackRange:
                buffDescription = $"+{buffValue} range";
                break;
            case BuffType.AttackSpeed:
                buffDescription = $"+{buffValue} attack speed";
                break;
            case BuffType.MoveSpeed:
                buffDescription = $"+{buffValue}% move speed";
                break;
            case BuffType.Gold:
                buffDescription = $"+{buffValue}% gold";
                break;
            default:
                buffDescription = "";
                break;
        }
        return buffDescription;
    }
}

public enum BuffType {
    None = 0,
    AttackRange = 1,
    AttackSpeed = 2,
    MoveSpeed = 3,
    Gold = 4,
}

public enum WeaponType {
    None = 0,
    Hammer = PoolType.Hammer,
    Lollipop = PoolType.Lollipop,
    Knife = PoolType.Knife,
    CandyCane = PoolType.CandyCane,
    Boomerang = PoolType.Boomerang,

}

public enum HatType {
    None = 0,
    HatA = PoolType.HatA,

}

public enum PantsType { 
    None = 0,
    PantsA = PoolType.PantsA,

}
