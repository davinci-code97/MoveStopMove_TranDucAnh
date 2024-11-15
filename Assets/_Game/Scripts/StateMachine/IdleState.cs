using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : IState
{
    private float timer;
    private float randomTime;
    private float minRandomTime = 0f;
    private float maxRandomTime = 4f;

    public void OnEnter(Bot bot) {
        bot.StopMoving();
        timer = 0;
        randomTime = Random.Range(minRandomTime, maxRandomTime);
    }

    public void OnExecute(Bot bot) {
        timer += Time.deltaTime;

        if (GameManager.Instance.currentState == GameState.PLAYING) {
            if (bot.HasCharacterInRange) {
                bot.ChangeState(new AttackState());
            }

            if (timer > randomTime) {
                if (bot.HasCharacterInRange) {
                    bot.ChangeState(new AttackState());
                }
                else {
                    bot.ChangeState(new PatrolState());
                }
            }
        }

    }

    public void OnExit(Bot bot) {

    }


}
