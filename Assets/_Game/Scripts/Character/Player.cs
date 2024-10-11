using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class Player : Character
{
    [SerializeField] private Joystick joystick;

    private Vector2 direction;

    protected override void Start()
    {
        OnInit();
    }

    protected override void Update()
    {
        base.Update();
        Move();

        if (rb.velocity.Equals(Vector3.zero) && nearestTarget != null) {
            Attack();
        }
    }

    protected override void OnInit() {
        base.OnInit();
    }

    protected override void SetUpCurrentWeapon() {
        base.SetUpCurrentWeapon();
        WeaponType weaponPoolType = UserDataManager.Instance.GetCurrentWeaponPoolType();
        WeaponConfig weaponConfig = LevelManager.Instance.GetWeaponByWeaponType(weaponPoolType);
        //if (weaponConfig == null) { return; }
        //Debug.Log(weaponConfig.itemType);
        weapon = HBPool.Spawn<Weapon>(weaponConfig.itemType, rightHand.position, Quaternion.Euler(0, -90, 0));
        Quaternion rotation = Quaternion.Euler(0, -90, 0);
        weapon.SetWeaponParent(rightHand);
        weapon.SetOwner(this);
    }

    public override void Move() {
        if (joystick == null) return;
        base.Move();
        direction = joystick.Direction;
        rb.velocity = new Vector3(direction.x * moveSpeed, rb.velocity.y, direction.y * moveSpeed);
    }

    //public override void Attack() { }

}
