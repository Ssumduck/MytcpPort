using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stat", menuName = "Stat", order = 0)]
public class Stat : ScriptableObject
{
    public string objectName;
    public int hp, maxHp, defense, dmg;
    public float atkTime, critical, criticaldmg;
}
