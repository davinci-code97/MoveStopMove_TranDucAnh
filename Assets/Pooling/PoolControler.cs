using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class PoolControler : MonoBehaviour
{
    [Space]
    [Header("Pool")]
    public List<GameUnit> PoolNoneRoot;

    [Header("Pool")]
    public List<PoolAmount> PoolWithRoot;

    [Header("Particle")]
    public ParticleAmount[] Particle;


    public void Awake()
    {
        for (int i = 0; i < PoolNoneRoot.Count; i++)
        {
            HBPool.Preload(PoolNoneRoot[i], 0, transform);
        }       
        
        for (int i = 0; i < PoolWithRoot.Count; i++)
        {
            HBPool.Preload(PoolWithRoot[i].prefab, PoolWithRoot[i].amount, PoolWithRoot[i].root);
        }

        for (int i = 0; i < Particle.Length; i++)
        {
            ParticlePool.Preload(Particle[i].prefab, Particle[i].amount, Particle[i].root);
            ParticlePool.Shortcut(Particle[i].particleType, Particle[i].prefab);
        }
    }
}

[System.Serializable]
public class PoolAmount
{
    [Header("-- Pool Amount --")]
    public Transform root;
    public GameUnit prefab;
    public int amount;

    public PoolAmount (Transform root, GameUnit prefab, int amount)
    {
        this.root = root;
        this.prefab = prefab;
        this.amount = amount;
    }
}


[System.Serializable]
public class ParticleAmount
{
    public Transform root;
    public ParticleType particleType;
    public ParticleSystem prefab;
    public int amount;
}


public enum ParticleType
{
    BloodExplosionRound = 0,
    SingleThunder = 10,
}

public enum PoolType
{
    None = 0,

    // Weapon
    Hammer,
    Lollipop,
    Knife,
    CandyCane,
    Boomerang,
    SwirlyPop,

    //Bullets
    Hammer_Bullet,
    Lollipop_Bullet,
    Knife_Bullet,
    CandyCane_Bullet,
    Boomerang_Bullet,
    SwirlyPop_Bullet,

    //Hats
    Hat_Arrow,
    Hat_Crown,

    //Pants
    Pants_Batman,
    Pants_chambi,
    Pants_comy,
    Pants_dabao,
    Pants_onion,
    Pants_pokemon,
    Pants_rainbow,
    Pants_Skull,
    Pants_vantim,

    // Bots
    Bot_Default,

    
}

public enum BulletType {
    None = 0,
    Hammer_Bullet = PoolType.Hammer_Bullet,
    Lollipop_Bullet = PoolType.Lollipop_Bullet,
    Knife_Bullet = PoolType.Knife_Bullet,
    CandyCane_Bullet = PoolType.CandyCane_Bullet,
    Boomerang_Bullet = PoolType.Boomerang_Bullet,
    SwirlyPop_Bullet = PoolType.SwirlyPop_Bullet,

}

public enum BotType {
    None = 0,
    Bot_Default = PoolType.Bot_Default,
}