using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }



}
