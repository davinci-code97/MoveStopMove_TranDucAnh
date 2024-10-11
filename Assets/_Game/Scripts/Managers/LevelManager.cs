using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager Instance;

    public List<WeaponConfig> WeaponConfigs;
    public List<ItemConfig> HatConfigs;

    public int level;

    private void Awake() {
        Instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    internal static void SetCharacterRemain(Character character) {
        
    }

    internal WeaponConfig GetWeaponByWeaponType(WeaponType weaponPoolType) {
        //Debug.Log((PoolType)weaponPoolType);    
        //Debug.Log(weaponPoolType);
        foreach (WeaponConfig weapon in WeaponConfigs) {
            if (weapon.itemType == (PoolType)weaponPoolType)
                return weapon;
        }
        return null;
    }

}
