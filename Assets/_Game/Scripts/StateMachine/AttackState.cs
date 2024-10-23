using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    float timer;
    float randomTime;
    float minRandomTime = 2f;
    float maxRandomTime = 4f;

    public void OnEnter(Bot bot) {
        timer = 0;
        randomTime = Random.Range(minRandomTime, maxRandomTime);

        if (bot.NearestTarget != null) {
            bot.StopMoving();
            bot.Attack();
        }
    }

    public void OnExecute(Bot bot) {
        timer += Time.deltaTime;
        if (timer > randomTime) {
            bot.ChangeState(new PatrolState());
        }
    }

    public void OnExit(Bot bot) {

    }


}
