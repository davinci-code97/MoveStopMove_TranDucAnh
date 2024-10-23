using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public static Player Instance { get; private set; }

    [SerializeField] private Joystick joystick;

    private Vector2 direction;

    private int killCount = 0;

    private void Awake() {
        Instance = this;
    }

    protected override void Start()
    {
        OnInit();
    }

    protected override void Update()
    {
        base.Update();
        HandleMovement();

        if (charactersInRange.Count > 0) {
            FindNearestTarget();
        }

        if (rb.velocity.Equals(Vector3.zero) && nearestTarget != null) {
            Attack();
        }
    }

    protected override void OnTriggerEnter(Collider other) {
        base.OnTriggerEnter(other);
        if (other.CompareTag(Constants.TAG_OBSTACLE)) {
            //Debug.Log("Obstacle in range");
            MeshRenderer obstacleRenderer = other.GetComponentInChildren<MeshRenderer>();
            Color color = obstacleRenderer.material.color;
            color.a = 0.5f;
            obstacleRenderer.material.color = color;
        }
    }

    protected override void OnTriggerExit(Collider other) {
        base.OnTriggerExit(other);
        if (other.CompareTag(Constants.TAG_OBSTACLE)) {
            Debug.Log("Obstacle out of range");
            MeshRenderer obstacleRenderer = other.GetComponentInChildren<MeshRenderer>();
            Color color = obstacleRenderer.material.color;
            color.a = 1f;
            obstacleRenderer.material.color = color;
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
        Quaternion rotation = Quaternion.Euler(0, -90, 0);
        weapon = HBPool.Spawn<Weapon>(weaponConfig.itemType, rightHand.position, rotation);
        weapon.SetWeaponParent(rightHand);
        weapon.SetOwner(this);
    }

    protected override void Target_OnCharacterDead(object sender, Character target) {
        base.Target_OnCharacterDead(sender, target);
        killCount++;
        //UserDataManager.Instance.IncreasePlayerGold(target.GetCharacterGoldValue());
    }

    public override void HandleMovement() {
        if (joystick == null) return;
        base.HandleMovement();
        direction = joystick.Direction;
        rb.velocity = new Vector3(direction.x * moveSpeed, rb.velocity.y, direction.y * moveSpeed);
    }



    //public override void Attack() { }

}
