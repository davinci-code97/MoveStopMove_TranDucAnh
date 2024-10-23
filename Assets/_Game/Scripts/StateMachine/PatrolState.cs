using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : IState
{
    float timer;
    float randomTime;
    float minRandomTime = 2f;
    float maxRandomTime = 5f;

    public void OnEnter(Bot enemy) {
        timer = 0;
        randomTime = Random.Range(minRandomTime, maxRandomTime);

    }

    public void OnExecute(Bot enemy) {
        timer += Time.deltaTime;
        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();

        Vector3 randomPosition = LevelManager.Instance.GetRandomNavMeshPosition(LevelManager.Instance.spawnRadius);

        if (timer < randomTime) {
            //enemy.Moving();
            agent.SetDestination(randomPosition);
        }
        else {
            enemy.ChangeState(new IdleState());
        }

    }

    public void OnExit(Bot enemy) {

    }


}
