using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShopItemDataList<T> where T : Enum {
    public List<ShopItem<T>> shopItemsDataList;

}

[Serializable]
public class ShopItem<T> : ShopItem where T : Enum {
    public T type;

}

[Serializable]
public class ShopItem
{
    public Sprite sprite;
    public float price;
    public BuffType buffType;
    public float buffValue;
    public string buffDescription;

    public string setBuffDescription() {
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
    //CandyCane = PoolType.CandyCane,
    //Boomerang = PoolType.Boomerang,

}

public enum HatType {
    None = 0,
    Hat_Arrow = PoolType.Hat_Arrow,
    Hat_Crown = PoolType.Hat_Crown,

}

public enum PantsType { 
    None = 0,
    Pants_Batman = PoolType.Pants_Batman,
    Pants_chambi = PoolType.Pants_chambi,
    //Pants_comy = PoolType.Pants_comy,
    //Pants_dabao = PoolType.Pants_dabao,
    //Pants_onion = PoolType.Pants_onion,
    //Pants_Pokemon = PoolType.Pants_pokemon,
    //Pants_rainbow = PoolType.Pants_rainbow,
    //Pants_Skull = PoolType.Pants_Skull,
    //Pants_vantim = PoolType.Pants_vantim,

}

public enum FullSetType {
    None = 0,

}