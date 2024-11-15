using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : GameUnit
{
    [SerializeField] private WeaponConfig config;
    [SerializeField] private Rigidbody rb;

    private Character owner;
    private Character target;
    private Vector3 direction;
    private float damage;
    private float bulletSpeed;
    [SerializeField] private float rotateSpeed;

    private Vector3 shootPoint;
    private float attackRange;

    void Start()
    {
        OnInit();
    }

    private void OnInit() {
        damage = config.damage;
        rotateSpeed = config.rotateSpeed;
        bulletSpeed = config.bulletSpeed;
    }

    void Update()
    {
        TF.Rotate(rotateSpeed * Vector3.up, Space.Self);
        rb.velocity = direction * bulletSpeed;

        // despawn khi qua range
        float distance = Vector3.Distance(TF.position, shootPoint);
        if (distance > attackRange)
        {
            HBPool.Despawn(this);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        CheckCollideWithCharacter(collision);
        CheckCollideWithMap(collision);
    }

    private void CheckCollideWithCharacter(Collision collision) {
        if (collision.gameObject.CompareTag(Constants.TAG_OBSTACLE) || collision.gameObject.CompareTag(Constants.TAG_WALL)) {
            SoundManager.Instance.PlayHitSFX(TF.position);
            HBPool.Despawn(this);
        }
    }

    private void CheckCollideWithMap(Collision collision) {
        if (collision.gameObject.CompareTag(Constants.TAG_CHARACTER) && collision.gameObject != owner.gameObject) {
            Character character = collision.gameObject.GetComponent<Character>();
            character.OnHit(this, damage);
            HBPool.Despawn(this);
        }
    }

    
    public void SetTarget(Character character) {
        target = character;
        direction = target.transform.position - TF.position;
    }

    public void SetBulletOwner(Character character) {
        owner = character;
    }

    public Character GetOwner() { return owner; }

    public void SetRange(float range) {
        attackRange = range;
    }

    public void SetShootPoint(Vector3 position) {
        shootPoint = position;
    }
}
