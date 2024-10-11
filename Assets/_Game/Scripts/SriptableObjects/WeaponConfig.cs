using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "Scriptable Object/Weapon Config", order = 2)]
public class WeaponConfig : ItemConfig
{
    //public Weapon weaponPrefab;
    //public Bullet bulletPrefab;

    public BulletType bulletType;
    public float damage;
    public float bulletSpeed;
    public float rotateSpeed;


}
