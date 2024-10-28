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
    public float spawnRadius;

    private float countdownTime;
    private Vector3 playerStartPoint;

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
        playerStartPoint = currentLevelConfig.playerStartPoint;
        Player.Instance.SetPlayerPosition(playerStartPoint);

        for (int i = 0; i < maxCurrentBotCount; i++) {
            //SpawnBot();
        }
    }

    public void SpawnBot() {
        if (currentBotsList.Count >= maxCurrentBotCount) return;
        if (botsCount <= 0) return;

        Vector3 randomPosition = GetRandomNavMeshPosition();

        System.Random random = new System.Random();
        int index = random.Next(botTypeList.Count);

        Bot bot = HBPool.Spawn<Bot>((PoolType)botTypeList[index], randomPosition, Quaternion.identity);
        currentBotsList.Add(bot);

        // set bot weapon
    }

    public Vector3 GetRandomNavMeshPosition() {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * spawnRadius;
        randomDirection += playerStartPoint;
        randomDirection.y = 0;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, spawnRadius, NavMesh.AllAreas)) {
            return hit.position;
        } else { 
            return GetRandomNavMeshPosition(); 
        }
    }

    public void RemoveFromCurrentBotsList(Character bot) {
        currentBotsList.Remove(bot);
    }

    public void SetCharacterRemain(Character character) {
        botsCount--;
    }

    public WeaponConfig GetWeaponByWeaponType(WeaponType weaponPoolType) {
        //Debug.Log((PoolType)weaponPoolType);
        //Debug.Log(weaponPoolType);
        foreach (WeaponConfig weapon in WeaponConfigList) {
            if (weapon.itemType == (PoolType)weaponPoolType)
                return weapon;
        }
        return null;
    }

    public WeaponConfig GetRandomWeaponType() {
        System.Random random = new System.Random();
        int index = random.Next(WeaponConfigList.Count);

        return WeaponConfigList[index];
    }

}
