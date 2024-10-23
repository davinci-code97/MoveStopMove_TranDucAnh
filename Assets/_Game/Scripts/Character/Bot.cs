using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : Character
{
    private IState currentState;

    

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (currentState != null && !IsDead) {
            currentState.OnExecute(this);
        }

    }

    protected override void OnInit() {
        base.OnInit();

        ChangeState(new IdleState());
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

    public void StopMoving() {
        rb.velocity = Vector3.zero;
        ChangeAnim(Constants.ANIM_IDLE);
    }

    public override void Attack() {
        base.Attack();

    }

    protected override void Target_OnCharacterDead(object sender, Character target) {
        base.Target_OnCharacterDead(sender, target);
        //IncreaseBotGoldValue(target);
    }

    

}
