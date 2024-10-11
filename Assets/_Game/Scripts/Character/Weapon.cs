using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : GameUnit
{
    [SerializeField] private WeaponConfig config;
    [SerializeField] private BulletType bulletType;
    public BuffType buffType;
    public int buffValue;
    public Character owner;

    private void Start() {
        OnInit();
    }

    private void OnInit() {
        bulletType = config.bulletType;
        buffType = config.buffType;
        buffValue = config.buffValue;
        SetOwner(owner);
    }

    public void SetOwner(Character character) {
        owner = character;
    }

    public void SetWeaponParent(Transform parent) {
        transform.SetParent(parent);
    }

    public void Fire(Vector3 shootPoint, Character target) {
        Bullet bullet = HBPool.Spawn<Bullet>((PoolType)bulletType, shootPoint, Quaternion.identity);
        bullet.SetTarget(target);
        bullet.SetOwner(owner);
    }

}
