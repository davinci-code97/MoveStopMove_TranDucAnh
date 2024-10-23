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
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag(Constants.TAG_CHARACTER) && collision.gameObject != owner.gameObject) {
            Character character = collision.gameObject.GetComponent<Character>();
            character.OnHit(this, damage);

            HBPool.Despawn(this);
        }
    }

    //private void OnTriggerEnter(Collider other) {
    //    if (other.CompareTag(Constants.TAG_CHARACTER)) {
    //        Character character = other.GetComponent<Character>();
    //        character.OnHit(damage);

    //        HBPool.Despawn(this);
    //    }
    //}
    
    public void SetTarget(Character character) {
        target = character;
        direction = target.transform.position - TF.position;
    }

    public void SetOwner(Character character) {
        owner = character;
    }

    public Character GetOwner() { return owner; }

}
