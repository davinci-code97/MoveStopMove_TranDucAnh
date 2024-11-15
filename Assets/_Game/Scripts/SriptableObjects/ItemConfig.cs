using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemConfig", menuName = "Scriptable Object/Item Config", order = 1)]
public class ItemConfig : ScriptableObject {

    public PoolType itemType;
    public BuffType buffType;
    public int buffValue;

}
