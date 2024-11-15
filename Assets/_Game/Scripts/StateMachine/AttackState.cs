using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private float timer;
    private float randomTime;
    private float minRandomTime = 1f;
    private float maxRandomTime = 2f;

    public void OnEnter(Bot bot) {
        timer = 0;
        randomTime = Random.Range(minRandomTime, maxRandomTime);

    }

    public void OnExecute(Bot bot) {
        timer += Time.deltaTime;

        if (bot.HasCharacterInRange) {
            bot.StopMoving();
            bot.botAttack();
        }

        if (timer > randomTime) {
            bot.ChangeState(new PatrolState());
        }
    }

    public void OnExit(Bot bot) {

    }


}
