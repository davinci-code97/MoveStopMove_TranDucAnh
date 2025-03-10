using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material normalMat;
    [SerializeField] private Material transparentMat;

    public void ChangeTransparentMateral() {
        meshRenderer.material = transparentMat;
    }

    public void ChangeNormalMateral() {
        meshRenderer.material = normalMat;
    }

}
