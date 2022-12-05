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

    [Header("StatsOthers")]
    public PokemonTypes Type;
    public int ExpWorth;
    public int CatchRate;

}
