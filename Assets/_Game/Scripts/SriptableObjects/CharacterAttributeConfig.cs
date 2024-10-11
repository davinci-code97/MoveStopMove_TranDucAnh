using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Atrribute", menuName = "Scriptable Object/Character Attribute", order = 1)]
public class CharacterAttributeConfig : ScriptableObject
{
    public float moveSpeed;
    public float hp;
    public float gold;
}
