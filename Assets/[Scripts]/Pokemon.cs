using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[CreateAssetMenu(fileName = "New Pokemon", menuName = "Pokemon/New Pokemon")]
public class Pokemon : ScriptableObject 
{
    [Header("Sprites (1: PlayerView - 2:EnemyView)")]
    public Sprite poke1;
    public Sprite poke2;

    [Header("Attacks")]
    public Attacks attack0;
    public Attacks attack1;
    public Attacks attack2;
    public Attacks attack3;

    [Header("StatsBase")]
    public int BaseHP;
    public int BaseAtk;
    public int BaseDef;
    public int BaseSpeed;

    [Header("StatsMult")]
    public int MultHP;
    public int MultAtk;
    public int MultDef;
    public int MultSpeed;

    //[Header("StatsFinal")]
    //public int FinalHP;
    //public int FinalAtk;
    //public int FinalDef;
    //public int FinalSpeed;

    //[Header("StatsBuff")]
    //public int BuffAtk;
    //public int BuffDef;
    //public int BuffSpeed;

    [Header("StatsOthers")]
    //public int lvl = 1;
    //public int currentHP;
    public PokemonTypes Type;

    //public void SetHPToMax()
    //{
    //    currentHP = GetFinalHp();
    //}
    //public void UpdatePokemonStats()
    //{
    //    FinalHP = BaseHP+ (MultHP * lvl);
    //    FinalAtk = BaseAtk + BuffAtk + (MultAtk * lvl);
    //    FinalDef = BaseDef + BuffDef + (MultDef * lvl);
    //    FinalSpeed = BaseSpeed + BuffSpeed + (MultSpeed * lvl);
    //}

    //public int GetFinalHp()
    //{
    //    FinalHP = BaseHP + (MultHP * lvl);
    //    return FinalHP;
    //}

    //public int GetFinalAtk()
    //{
    //    FinalAtk = BaseAtk + BuffAtk + (MultAtk * lvl);
    //    return FinalAtk;
    //}

    //public int GetFinalDef()
    //{
    //    FinalDef = BaseDef + BuffDef + (MultDef * lvl);
    //    return FinalDef;
    //}

    //public int GetFinalSpeed()
    //{
    //    FinalSpeed = BaseSpeed + BuffSpeed + (MultSpeed * lvl);
    //    return FinalSpeed;
    //}

}
