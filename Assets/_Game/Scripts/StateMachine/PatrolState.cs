using System.Collections;
using UnityEngine;

public class PatrolState : IState
{
    private float timer;
    private float randomTime;
    private float minRandomTime = 2f;
    private float maxRandomTime = 5f;

    public void OnEnter(Bot enemy) {
        timer = 0;
        randomTime = Random.Range(minRandomTime, maxRandomTime);
        Vector3 randomPosition = LevelManager.Instance.GetRandomNavMeshPosition();
        enemy.MoveNavMeshAgent(randomPosition);

    }

    public void OnExecute(Bot enemy) {
        timer += Time.deltaTime;

        if (timer > randomTime) {
            enemy.ChangeState(new IdleState());
        }
    }

    public void OnExit(Bot enemy) {

    }


}
