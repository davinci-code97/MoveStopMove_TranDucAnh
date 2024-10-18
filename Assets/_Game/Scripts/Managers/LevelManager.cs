using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Progress;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public List<LevelConfig> LevelConfigList;
    public LevelConfig currentLevelConfig;

    public List<WeaponConfig> WeaponConfigList;
    public List<ItemConfig> HatConfigList;

    //public List<CharacterConfig> BotConfigs;
    public List<Character> currentBotsList = new List<Character>();
    public List<BotType> botTypeList;
    public int botsCount;
    private int maxCurrentBotCount;
    private float spawnRadius;

    private float countdownTime;

    private void Awake() {
        Instance = this;
    }

    void Start()
    {
        OnInit();

    }

    void Update()
    {
        
    }

    private void OnInit() {
        currentLevelConfig = LevelConfigList[UserDataManager.Instance.GetCurrentLevel()];
        botTypeList = currentLevelConfig.botTypeList;
        botsCount = currentLevelConfig.botCount;
        maxCurrentBotCount = currentLevelConfig.maxCurrentBotCount;
        spawnRadius = currentLevelConfig.spawnRadius;
        countdownTime = currentLevelConfig.countdownTime;

        for (int i = 0; i < maxCurrentBotCount; i++) {
            SpawnBot();
        }
    }

    public void SpawnBot() {
        if (currentBotsList.Count >= maxCurrentBotCount) return;
        if (botsCount <= 0) return;
        // Ensure we don't exceed the active bots limit
        //int activeBots = currentBotsList.FindAll(bot => bot.activeSelf).Count;
        //if (activeBots >= activeBotsLimit) return;

        Vector3 randomPosition = GetRandomNavMeshPosition(spawnRadius);
        System.Random random = new System.Random();
        int index = random.Next(botTypeList.Count);
        Bot bot = HBPool.Spawn<Bot>((PoolType)botTypeList[index], randomPosition, Quaternion.identity);
        currentBotsList.Add(bot);
    }

    Vector3 GetRandomNavMeshPosition(float radius) {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, radius, 1);
        return hit.position;
    }

    public void RemoveFromCurrentBotsList(Character bot) {
        currentBotsList.Remove(bot);
    }

    internal void SetCharacterRemain(Character character) {
        botsCount--;
    }

    internal WeaponConfig GetWeaponByWeaponType(WeaponType weaponPoolType) {
        //Debug.Log((PoolType)weaponPoolType);    
        //Debug.Log(weaponPoolType);
        foreach (WeaponConfig weapon in WeaponConfigList) {
            if (weapon.itemType == (PoolType)weaponPoolType)
                return weapon;
        }
        return null;
    }

}
