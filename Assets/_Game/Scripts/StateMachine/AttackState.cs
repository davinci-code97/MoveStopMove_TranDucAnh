using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    float timer;

    public void OnEnter(Bot bot) {
        timer = 0;

        if (bot.NearestTarget != null) {
            bot.StopMoving();
            bot.Attack();
        }
    }

    public void OnExecute(Bot bot) {
        timer += Time.deltaTime;
        if (timer > 1.5f) {
            bot.ChangeState(new PatrolState());
        }
    }

    public void OnExit(Bot bot) {

    }


}
