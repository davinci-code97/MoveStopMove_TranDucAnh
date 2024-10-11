using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Scriptable Object/Level Config", order = 1)]

public class LevelConfig : ScriptableObject
{
    public int maxBots;
    public float countdownTime;
}
