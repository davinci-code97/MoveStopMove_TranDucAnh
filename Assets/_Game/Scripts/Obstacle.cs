using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material normalMat;
    [SerializeField] private Material transparentMat;

    void Start()
    {
        Player.Instance.OnEnterObstacle += Player_OnEnterObstacle;
        Player.Instance.OnExitObstacle += Player_OnExitObstacle;
    }

    private void Player_OnEnterObstacle(object sender, System.EventArgs e) {
        meshRenderer.material = transparentMat;
    }

    private void Player_OnExitObstacle(object sender, System.EventArgs e) {
        meshRenderer.material = normalMat;
    }

}
