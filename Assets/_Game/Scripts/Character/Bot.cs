using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Waypoint_Indicator indicator;

    private IState currentState;

    protected override void Update()
    {
        base.Update();
        if (!IsDead) {
            currentState?.OnExecute(this);
        }
    }

    public override void OnInit() {
        base.OnInit();
        SetUpModelColor();
        indicator.offScreenSpriteColor = modelMeshRenderder.material.color;
        ChangeState(new IdleState());
    }

    private void SetUpModelColor() {
        Color randomColor = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        modelMeshRenderder.material.color = randomColor;
    }

    protected override void SetUpCurrentWeapon() {
        base .SetUpCurrentWeapon();
        WeaponConfig weaponConfig = LevelManager.Instance.GetRandomWeaponConfig();
        SetUpWeapon(weaponConfig);
    }

    protected override void SetUpCurrentHat() {
        base.SetUpCurrentHat();
        HatConfig hatConfig = LevelManager.Instance.GetRandomHatConfig();
        SetUpHat(hatConfig);
    }

    protected override void SetUpCurrentPants() {
        base.SetUpCurrentPants();
        PantsConfig pantsConfig = LevelManager.Instance.GetRandomPantsConfig();
        SetUpPants(pantsConfig);
    }

    protected override void Character_OnCharacterDead(object sender, OnCharacterDeadEventArgs e) {
        base.Character_OnCharacterDead(sender, e);
        agent.SetDestination(transform.position);
        ResetNavMeshNavigation();
        ChangeState(new DeadState());
        StartCoroutine(DespawnAfterDelay(2f));

        if (GameManager.Instance.currentState == GameState.PLAYING) {
            LevelManager.Instance.SetCharacterRemain();
        }
        LevelManager.Instance.RemoveFromCurrentBotList(e.character);
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

    public void ResetNavMeshNavigation() {
        agent.isStopped = true;
        agent.ResetPath();
    }

    public void botAttack() {
        Attack();
    }

}
