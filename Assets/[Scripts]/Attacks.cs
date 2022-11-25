using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attacks/New Attack")]
public class Attacks : ScriptableObject
{
    [Header("AttackStats")]
    public int Damage;
    public int Accuaracy;
    public int MaxPP;
    public PokemonTypes Type;

    [Header("AttackOthers")]
    public bool isOffensive = false;
    public bool isHeal = false;
    public bool isBuff = false;
    public BuffType buffType = BuffType.NONE;
    //public int TimesUsedInARow = 0;
    public bool isEmpty = false; //Test


    [Header("AttackAnimString")]
    public string animationNamePlayer = "";
    public string animationNameEnemy = "";
}
