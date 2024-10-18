using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Config", menuName = "Scriptable Object/Character Config", order = 1)]
public class CharacterConfig : ScriptableObject
{
    public BotType BotType;
    public float moveSpeed;
    public float hp;
    public float gold;
}
