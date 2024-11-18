using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character : GameUnit
{
    public event EventHandler<OnCharacterDeadEventArgs> OnCharacterDead;
    public class OnCharacterDeadEventArgs : EventArgs {
        public Character character;
    }

    [SerializeField] protected CharacterConfig characterConfig;

    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected Animator animator;
    protected string currentAnimName;

    [SerializeField] protected SphereCollider attackRangeCollider;
    [SerializeField] protected GameObject beingTargetedSprite;
    [SerializeField] protected Transform rightHand;
    [SerializeField] protected Transform shootPoint;
    [SerializeField] protected Weapon currentWeapon;

    [SerializeField] protected Hat currentHat;
    [SerializeField] protected Transform hatPos;
    [SerializeField] protected Material currentPants;
    [SerializeField] protected SkinnedMeshRenderer pantsMeshRenderder;
    [SerializeField] protected SkinnedMeshRenderer modelMeshRenderder;

    public bool IsDead { get; set; }
    protected bool isAttacking;
    protected bool canAttack;

    [SerializeField] protected float hp;
    protected float moveSpeed;
    protected float gold;
    protected float attackRange;

    [SerializeField] protected List<Character> charactersInRange;
    public bool HasCharacterInRange => charactersInRange.Count > 0;
    protected Character nearestTarget;
    protected float nearestDistance;

    protected float attackSpeed = 1f;
    protected float attackTimer = 0f;

    protected float throwSpeed = .2f;
    protected float throwTimer = 0f;

    protected Vector3 shootPointOffset = new Vector3(0.36f, 1.13f, 0.55f);

    protected int killCount = 0; 
    private int growthStage = 0; 
    [SerializeField] private int baseKillRequirement = 2;
    [SerializeField] private float growValue = .1f;

    private void Start() {
        OnInit();
    }

    protected virtual void Update() {
        if (IsDead || GameManager.Instance.currentState != GameState.PLAYING) {
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezePosition;
            return;
        }
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        TryResetAttack();
    }

    private void TryResetAttack() {
        if (!canAttack) {
            attackTimer += Time.deltaTime;
            if (attackTimer > attackSpeed) {
                attackTimer = 0f;
                canAttack = true;
                currentWeapon.SetWeaponActive(true);
            }
        }
    }

    protected virtual void OnTriggerEnter(Collider other) {
        CharacterTriggerEnter(other);
    }
    protected virtual void OnTriggerExit(Collider other) {
        CharacterTriggerExit(other);
    }
    private void CharacterTriggerEnter(Collider other) {
        if (other.CompareTag(Constants.TAG_CHARACTER) && other.gameObject != this.gameObject) {
            Character target = other.GetComponent<Character>();
            AddCharacterToRange(target);

            //Debug.Log("Enemy entered range: " + other.name);
            //Debug.Log($"Enemies in range: {charactersInRange.Count}");
        }
    }
    private void CharacterTriggerExit(Collider other) {
        if (other.CompareTag(Constants.TAG_CHARACTER) && other.gameObject != this.gameObject) {
            Character target = other.GetComponent<Character>();
            RemoveCharacterFromRange(target);

            //Debug.Log("Enemy exited range: " + other.name);
            //Debug.Log($"Enemies in range: {charactersInRange.Count}");
        }
    }

    public virtual void OnInit() {
        rb.constraints = RigidbodyConstraints.FreezeRotation |  RigidbodyConstraints.FreezePositionY;
        rb.detectCollisions = true;
        IsDead = false;
        canAttack = true;
        isAttacking = false;

        killCount = 0;
        moveSpeed = characterConfig.moveSpeed;
        hp = characterConfig.hp;
        attackRange = attackRangeCollider.radius;
        gold = characterConfig.gold;

        SetUpCurrentWeapon();
        SetUpCurrentHat();
        SetUpCurrentPants();

        ChangeAnim(Constants.ANIM_IDLE);
        OnCharacterDead += Character_OnCharacterDead;
    }

    // OnDeath
    protected virtual void Character_OnCharacterDead(object sender, OnCharacterDeadEventArgs e) {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezePosition;
        rb.detectCollisions = false;
        IsDead = true;
         
        SetBeingTargetedSprite(false);
        ChangeAnim(Constants.ANIM_DEAD);
        SoundManager.Instance.PlayDeadSFX(TF.position);
    }

    public void OnHit(Bullet bullet, float damage) {
        if (!IsDead) {
            hp -= damage;
            if (hp <= 0) {
                hp = 0;
                OnCharacterDead?.Invoke(this, new OnCharacterDeadEventArgs { character = this });
                bullet.GetOwner().OnKill(this);
            }
        }
    }

    public virtual void OnKill(Character victim) {
        IncreaseCharacterGoldValue(victim.GetCharacterGoldValue());
        killCount++;
        CheckGrowth();
    }

    protected IEnumerator ResetRB(float delay) {
        yield return new WaitForSeconds(delay);
        rb.isKinematic = false;
    }
    
    protected IEnumerator DespawnAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        OnDespawn();
    }

    protected virtual void OnDespawn() {
        HBPool.Despawn(this);
    }

    protected void AddCharacterToRange(Character enemy) {
        charactersInRange.Add(enemy);
        enemy.OnCharacterDead += EnemyInRange_OnCharacterDead; ;
    }

    protected void RemoveCharacterFromRange(Character target) {
        if (charactersInRange.Contains(target)) {
            charactersInRange.Remove(target);
        }
        if (target == nearestTarget) {
            target.SetBeingTargetedSprite(false);
            nearestTarget = null;
        }
        target.OnCharacterDead -= EnemyInRange_OnCharacterDead;
    }

    protected virtual void EnemyInRange_OnCharacterDead(object sender, OnCharacterDeadEventArgs e) {
        RemoveCharacterFromRange(e.character);
    }

    protected virtual void SetUpCurrentWeapon() {
        if (currentWeapon != null) {
            HBPool.Despawn(currentWeapon);
        }
    }
    public void SetUpWeapon(WeaponConfig weaponConfig) {
        if (currentWeapon != null) {
            HBPool.Despawn(currentWeapon);
        }
        currentWeapon = HBPool.Spawn<Weapon>(weaponConfig.itemType, rightHand.position, rightHand.rotation);
        currentWeapon.SetWeaponParent(rightHand);
        currentWeapon.SetWeaponOwner(this);
        currentWeapon.SetAttackRange(attackRange);
    }

    protected virtual void SetUpCurrentHat() {
        if (currentHat) {
            HBPool.Despawn(currentHat);
        }
    }
    public void SetUpHat(HatConfig hatConfig) {
        if (currentHat) {
            HBPool.Despawn(currentHat);
        }
        currentHat = HBPool.Spawn<Hat>(hatConfig.itemType, hatPos.position, hatPos.rotation);
        currentHat.SetHatParent(hatPos);
    }

    protected virtual void SetUpCurrentPants() {}
    public void SetUpPants(PantsConfig pantsConfig) {
        currentPants = pantsMeshRenderder.material = pantsConfig.material;
    }

    void CheckGrowth() {
        int requiredKillsForNextGrowth = baseKillRequirement * (int)Mathf.Pow(2, growthStage);

        if (killCount >= requiredKillsForNextGrowth) {
            GrowCharacter();
            growthStage++;
        }
    }
    void GrowCharacter() {
        transform.localScale += new Vector3(growValue, growValue, growValue);
        shootPoint.localPosition = new Vector3(shootPoint.localPosition.x, shootPointOffset.y, shootPoint.localPosition.z);

        SoundManager.Instance.PlayGrowSizeSFX(transform.position);
        //Debug.Log("Character grew! Current size: " + transform.localScale);
    }

    public void ChangeAnim(string animName) {
        if (currentAnimName != animName) {
            animator.ResetTrigger(animName);
            currentAnimName = animName;
            animator.SetTrigger(currentAnimName);
        }
    }

    protected virtual void HandleMovement() {
        if (!IsDead && rb.velocity.Equals(Vector3.zero) && !isAttacking) {
            ChangeAnim(Constants.ANIM_IDLE);
        }
        else if (!rb.velocity.Equals(Vector3.zero)) {
            isAttacking = false;
            throwTimer = 0f;
            ChangeAnim(Constants.ANIM_RUN);
            RotateCharacter(rb.velocity);
        }
    }

    protected virtual void Attack() {
        if (attackTimer < attackSpeed && canAttack) {
            isAttacking = true;
            Vector3 direction = nearestTarget.TF.position - TF.position;
            RotateCharacter(direction);
            ChangeAnim(Constants.ANIM_ATTACK);

            throwTimer += Time.deltaTime;
            if (throwTimer > throwSpeed) {
                currentWeapon.Fire(shootPoint.position, nearestTarget, attackRange);
                throwTimer = 0f;
                canAttack = false;
                currentWeapon.SetWeaponActive(false);
            }
        }
    }

    protected virtual void RotateCharacter(Vector3 direction) {
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        rb.MoveRotation(rotation);
    }

    protected virtual void FindNearestTarget() { }

    public void SetBeingTargetedSprite(bool isTargeted) => beingTargetedSprite.SetActive(isTargeted);

    public float GetCharacterGoldValue() => gold;

    public void IncreaseCharacterGoldValue(float value) {
        this.gold += value;
    }

}