using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Progress;

public class LevelManager : Singleton<LevelManager>
{

    public List<LevelConfig> LevelConfigList;
    [SerializeField] private LevelConfig currentLevelConfig;
    [SerializeField] private LevelPrefab currentLevelPrefab;
    

    public List<WeaponConfig> WeaponConfigList;
    public List<HatConfig> HatConfigList;
    public List<PantsConfig> PantsConfigList;

    //public List<CharacterConfig> BotConfigs;
    [SerializeField] private List<Character> currentBotList = new List<Character>();
    [SerializeField] private List<BotType> botTypeList;
    public int levelBotsCount { get; private set; }
    public int botsRemain { get; private set; }
    private int maxCurrentBotCount;
    private float spawnRadius;

    private Vector3 playerStartPoint;

    private float countdownTime;

    void Start()
    {
        OnInit();
    }

    private void OnInit() {
        SetupMap();
        SetupLevel();
        SetupBots();
    }

    private void SetupMap() {
        currentLevelConfig = LevelConfigList[UserDataManager.Instance.GetCurrentLevel()];
        if (currentLevelPrefab != null ) {
            Destroy( currentLevelPrefab );
        }
        currentLevelPrefab = Instantiate(currentLevelConfig.levelPrefab, Vector3.zero, Quaternion.identity);
        playerStartPoint = currentLevelConfig.playerStartPoint;
        Player.Instance.SetPlayerPosition(playerStartPoint);
    }

    private void SetupLevel() {
        botTypeList = currentLevelConfig.botTypeList;
        levelBotsCount = currentLevelConfig.botCount;
        maxCurrentBotCount = currentLevelConfig.maxCurrentBotCount;
        spawnRadius = currentLevelConfig.spawnRadius;
        countdownTime = currentLevelConfig.countdownTime;
    }

    private void SetupBots() {
        botsRemain = levelBotsCount;
        for (int i = 0; i < maxCurrentBotCount; i++) {
            SpawnBot();
        }
    }

    public void ResetLevel() {
        Player.Instance.SetPlayerPosition(playerStartPoint);
        Player.Instance.OnInit();
        DespawnAllBots();
        SetupBots();
    }

    public void SpawnBot() {
        if( (currentBotList.Count >= maxCurrentBotCount) 
            || (botsRemain <= currentBotList.Count) )
            return;

        Vector3 randomPosition = GetRandomBotSpawnPosition();

        System.Random random = new System.Random();
        int index = random.Next(botTypeList.Count);

        Bot bot = HBPool.Spawn<Bot>((PoolType)botTypeList[index], randomPosition, Quaternion.identity);
        currentBotList.Add(bot);
        bot.OnInit();
    }

    public void SetCharacterRemain() {
        botsRemain--;
        UIGamePlaying.Instance.UpdateBotRemainsNumber(botsRemain);
        SpawnBot();
        if (botsRemain <= 0) {
            DespawnAllBots();
            GameManager.Instance.SetGameState(GameState.WIN);
            Player.Instance.OnWinGame();
        }
    }

    public void RemoveFromCurrentBotList(Character bot) {
        if (currentBotList.Contains(bot)) {
            currentBotList.Remove(bot);
        }
    }

    public void DespawnAllBots() {
        foreach (Bot bot in currentBotList) {
            HBPool.Despawn(bot);
        }
        currentBotList.Clear();
    }

    public Vector3 GetRandomNavMeshPosition() {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * spawnRadius;
        randomDirection += playerStartPoint;
        randomDirection.y = 0;
        NavMeshHit hit;
        for (int i = 0; i < 3; i++) {
            if (NavMesh.SamplePosition(randomDirection, out hit, spawnRadius, NavMesh.AllAreas)) {
                return hit.position;
            }
        }
        return Vector3.zero;
    }

    public Vector3 GetRandomBotSpawnPosition() {
        float playerSafeRange = 10f;
        for (int i = 0; i < 10; i++) {
            Vector3 randomPos = GetRandomNavMeshPosition();
            if (Vector3.Distance(randomPos, playerStartPoint) > playerSafeRange) {
                return randomPos;
            }
        }
        return Vector3.zero;
    }

    public WeaponConfig GetWeaponConfigByType(WeaponType weaponPoolType) {
        //Debug.Log((PoolType)weaponPoolType);
        //Debug.Log(weaponPoolType);
        foreach (WeaponConfig weapon in WeaponConfigList) {
            if (weapon.itemType == (PoolType)weaponPoolType)
                return weapon;
        }
        return null;
    }

    public WeaponConfig GetRandomWeaponConfig() {
        System.Random random = new System.Random();
        int index = random.Next(WeaponConfigList.Count);

        return WeaponConfigList[index];
    }

    public HatConfig GetHatConfigByType(HatType hatType) {
        foreach (HatConfig hatConfig in HatConfigList)
        {
            if (hatConfig.itemType == (PoolType)hatType) {
                return hatConfig;
            }
        }
        return null;
    }

    public HatConfig GetRandomHatConfig() {
        System.Random random = new System.Random();
        int index = random.Next(HatConfigList.Count);

        return HatConfigList[index];
    }

    public PantsConfig GetPantsConfigByType(PantsType pantsType) {
        foreach (PantsConfig pantConfig in PantsConfigList) {
            if (pantConfig.itemType == (PoolType)pantsType) {
                return pantConfig;
            }
        }
        return null;
    }

    public PantsConfig GetRandomPantsConfig() {
        System.Random random = new System.Random();
        int index = random.Next(PantsConfigList.Count);

        return PantsConfigList[index];
    }



}
