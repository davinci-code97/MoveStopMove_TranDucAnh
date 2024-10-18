using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : GameUnit
{
    public event EventHandler<Character> OnCharacterDead;

    [SerializeField] protected CharacterConfig characterConfig;
    //[SerializeField] protected WeaponConfig weaponConfig;

    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected Animator animator;

    [SerializeField] protected Weapon weapon;
    [SerializeField] protected GameObject hat;
    [SerializeField] protected GameObject pants;

    [SerializeField] protected GameObject beingTargetedSprite;
    [SerializeField] protected Transform rightHand;
    [SerializeField] protected Transform shootPoint;

    public bool IsDead { get; set; }
    //public bool IsTargeted { get; set; }
    protected bool isAttacking;
    protected bool canAttack;

    [SerializeField] protected float hp;
    protected float moveSpeed;
    protected float gold;

    [SerializeField] protected List<Character> charactersInRange;
    protected Character nearestTarget;
    public Character NearestTarget => nearestTarget;
    protected float nearestDistance;

    private string currentAnimName;

    private float attackSpeed = 1f;
    private float attackTimer = 0f;

    private float throwSpeed = .2f;
    private float throwTimer = 0f;

    protected virtual void Start() {
        OnInit();
    }

    protected virtual void Update() {
        if (IsDead) return;

        if (charactersInRange.Count > 0) {
            FindNearestTarget();
        }
    }

    protected virtual void OnTriggerEnter(Collider other) {
        if (other.CompareTag(Constants.TAG_CHARACTER) && other.gameObject != this.gameObject) {
            Character target = other.GetComponent<Character>();
            charactersInRange.Add(target);
            target.OnCharacterDead += Target_OnCharacterDead;
            FindNearestTarget();

            Debug.Log("Enemy entered range: " + other.name);
            Debug.Log($"Enemies in range: {charactersInRange.Count}");
        }
    }

    protected virtual void OnTriggerExit(Collider other) {
        if (other.CompareTag(Constants.TAG_CHARACTER) && other.gameObject != this.gameObject) {
            Character target = other.GetComponent<Character>();
            if (target == nearestTarget) {
                target.SetBeingTargetedSprite(false);
                nearestTarget = null;
            }
            RemoveFromRange(target);
            target.OnCharacterDead -= Target_OnCharacterDead;
            FindNearestTarget();

            Debug.Log("Enemy exited range: " + other.name);
            Debug.Log($"Enemies in range: {charactersInRange.Count}");
        }
    }

    protected virtual void OnInit() {
        IsDead = false;
        canAttack = true;
        moveSpeed = characterConfig.moveSpeed;
        hp = characterConfig.hp;
        gold = characterConfig.gold;
        SetUpCurrentWeapon();
    }

    public void OnHit(Bullet bullet, float damage) {
        if (!IsDead) {
            hp -= damage;

            if (hp <= 0) {
                hp = 0;
                OnDeath(bullet.GetOwner());
            }
        }
    }

    protected virtual void OnDeath(Character character) {
        rb.velocity = Vector3.zero;
        IsDead = true;
        SetBeingTargetedSprite(false);
        ChangeAnim(Constants.ANIM_DEAD);
        StartCoroutine(DespawnAfterDelay(2f));
        OnCharacterDead?.Invoke(this, this);

        Character killer = character.GetComponent<Character>();
        killer.IncreaseCharacterGoldValue(GetCharacterGoldValue());

        LevelManager.Instance.SetCharacterRemain(this);
        UIManager.Instance.ShowNoti(this);
    }

    private IEnumerator DespawnAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        OnDespawn();
    }

    protected virtual void OnDespawn() {
        HBPool.Despawn(this);
    }

    protected virtual void SetUpCurrentWeapon() {

    }
    
    protected virtual void Target_OnCharacterDead(object sender, Character target) {
        RemoveFromRange(target);
        LevelManager.Instance.RemoveFromCurrentBotsList(target);
        LevelManager.Instance.SpawnBot();
    }

    private void RemoveFromRange(Character character) {
        if (charactersInRange.Contains(character)) {
            charactersInRange.Remove(character);
        }
        if (character == nearestTarget) {
            nearestTarget = null;
        }
    }

    protected void ChangeAnim(string animName) {
        if (currentAnimName != animName) {
            animator.ResetTrigger(animName);
            currentAnimName = animName;
            animator.SetTrigger(currentAnimName);
        }
    }

    public virtual void HandleMovement() {
        if (rb.velocity.Equals(Vector3.zero) && !isAttacking) {
            ChangeAnim(Constants.ANIM_IDLE);
        }
        else if (rb.velocity != Vector3.zero) {
            isAttacking = false;
            throwTimer = 0f;
            ChangeAnim(Constants.ANIM_RUN);
            Quaternion rotation = Quaternion.LookRotation(rb.velocity, Vector3.up);
            rb.rotation = rotation;
        }
    }

    public virtual void Attack() {
        if (attackTimer < attackSpeed && canAttack) {
            isAttacking = true;
            Vector3 direction = nearestTarget.TF.position - TF.position;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            rb.rotation = rotation;
            ChangeAnim(Constants.ANIM_ATTACK);

            throwTimer += Time.deltaTime;
            if (throwTimer > throwSpeed) {
                weapon.Fire(shootPoint.position, nearestTarget);
                throwTimer = 0f;
                canAttack = false;
            }

        }

        attackTimer += Time.deltaTime;
        if (attackTimer > attackSpeed) {
            attackTimer = 0f;
            canAttack = true;
        }

    }

    protected virtual void FindNearestTarget() {
        nearestDistance = Mathf.Infinity;
        if (charactersInRange.Count > 0) {
            foreach (Character character in charactersInRange) {
                Character target = character.GetComponent<Character>();
                float distance = Vector3.Distance(TF.position, target.TF.position);
                if (distance < nearestDistance) {
                    if (nearestTarget != null) {
                        nearestTarget.SetBeingTargetedSprite(false);
                    }
                    nearestDistance = distance;
                    nearestTarget = target;
                    nearestTarget.SetBeingTargetedSprite(true);
                }
                if (target != nearestTarget) {
                    target.SetBeingTargetedSprite(false);
                }
            }
        }
        else {
            nearestTarget = null;
        }
    }

    public void SetBeingTargetedSprite(bool isTargeted) => beingTargetedSprite.SetActive(isTargeted);

    public float GetCharacterGoldValue() {
        return gold;
    }

    public void IncreaseCharacterGoldValue(float value) {
        this.gold += value;
    }

}