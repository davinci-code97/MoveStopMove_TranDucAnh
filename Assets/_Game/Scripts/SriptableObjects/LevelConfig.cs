using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Scriptable Object/Level Config", order = 1)]

public class LevelConfig : ScriptableObject
{
    public int level;
    public LevelPrefab levelPrefab;

    public List<BotType> botTypeList;
    public int botCount;
    public int maxCurrentBotCount;
    public float spawnRadius;
    public float countdownTime;
    public Vector3 playerStartPoint;
}
