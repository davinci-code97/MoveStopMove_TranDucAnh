using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    float timer;
    float randomTime;
    float minRandomTime = 2f;
    float maxRandomTime = 4f;

    public void OnEnter(Bot bot) {
        bot.StopMoving();
        timer = 0;
        randomTime = Random.Range(minRandomTime, maxRandomTime);
    }

    public void OnExecute(Bot bot) {
        timer += Time.deltaTime;

        if (bot.NearestTarget != null) {
            bot.ChangeState(new AttackState());
        }

        if (timer > randomTime) {
            bot.ChangeState(new PatrolState());
        }

    }

    public void OnExit(Bot bot) {

    }

}
