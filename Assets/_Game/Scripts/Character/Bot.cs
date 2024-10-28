using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    [SerializeField] private NavMeshAgent agent;

    private IState currentState;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (!IsDead) {
            currentState?.OnExecute(this);
        }

    }

    protected override void OnInit() {
        base.OnInit();

        ChangeState(new IdleState());
    }

    protected override void OnDeath(Character character) {
        base.OnDeath(character);
        agent.SetDestination( transform.position );
        ResetNavMeshNavigation();
        ChangeState(new DeadState());
        StartCoroutine(DespawnAfterDelay(2f));
    }

    protected override void SetUpCurrentWeapon() {
        base.SetUpCurrentWeapon();
        WeaponConfig weaponConfig = LevelManager.Instance.GetRandomWeaponType();
        //if (weaponConfig == null) { return; }
        //Debug.Log(weaponConfig.itemType);
        Quaternion rotation = Quaternion.Euler(0, -90, 0);
        weapon = HBPool.Spawn<Weapon>(weaponConfig.itemType, rightHand.position, rotation);
        weapon.SetWeaponParent(rightHand);
        weapon.SetOwner(this);
        weapon.SetAttackRange(attackRange);
    }

    public void ChangeState(IState newState) {
        if (currentState != null) {
            currentState.OnExit(this);
        }

        currentState = newState;

        if (currentState != null) {
            currentState.OnEnter(this);
        }
    }

    public void MoveNavMeshAgent(Vector3 destination) {
        agent.isStopped = false;
        agent.SetDestination(destination);
        throwTimer = 0f;
        ChangeAnim(Constants.ANIM_RUN);
    }

    public void StopMoving() {
        rb.velocity = Vector3.zero;
        agent.velocity = Vector3.zero;
        agent.SetDestination(TF.position);
        //ResetNavMeshNavigation();
        ChangeAnim(Constants.ANIM_IDLE);
    }

    protected override void Attack() {
        if (charactersInRange.Count > 0) {
            FindNearestTarget();
            base.Attack();
        }
    }

    protected override void FindNearestTarget() {
        base.FindNearestTarget();
        nearestDistance = Mathf.Infinity;
        if (charactersInRange.Count > 0) {
            foreach (Character character in charactersInRange) {
                Character target = character.GetComponent<Character>();
                float distance = Vector3.Distance(TF.position, target.TF.position);
                if (distance < nearestDistance) {
                    nearestDistance = distance;
                    nearestTarget = target;
                }
            }
        }
        else {
            nearestTarget = null;
        }
    }

    protected override void Target_OnCharacterDead(object sender, Character target) {
        base.Target_OnCharacterDead(sender, target);
        //IncreaseBotGoldValue(target);
    }

    public void ResetNavMeshNavigation() {
        agent.isStopped = true;
        agent.ResetPath();
    }

    public void botAttack() {
        Attack();
    }
}
