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
    public float attackRange;

    private void Start() {
        OnInit();
    }

    private void OnInit() {
        bulletType = config.bulletType;
        buffType = config.buffType;
        buffValue = config.buffValue;
        //SetOwner(owner);
    }

    public void SetWeaponOwner(Character character) {
        owner = character;
    }

    public void SetAttackRange(float range) {
        attackRange = range;
    }

    public void SetWeaponParent(Transform parent) {
        TF.SetParent(parent);
    }

    public void SetWeaponActive(bool active) {
        gameObject.SetActive(active);
    }

    public void Fire(Vector3 shootPoint, Character target, float range) {
        SoundManager.Instance.PlayShootSFX(TF.position);
        Bullet bullet = HBPool.Spawn<Bullet>((PoolType)bulletType, shootPoint, Quaternion.identity);
        bullet.SetBulletOwner(owner);
        bullet.SetTarget(target);
        bullet.SetShootPoint(shootPoint);
        bullet.SetRange(range);
    }

}
