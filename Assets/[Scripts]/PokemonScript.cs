using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokemonScript : MonoBehaviour
{
    public BattleSceneManager battleSceneManager;

    public Pokemon pokemon;
    //public Image PokemonSprite;
    public List<Attacks> ListAttacks;
    public bool isPlayerPokemon = false;

    public string PokemonName;

    [Header("StatsFinal")]
    public int FinalHP;
    public int FinalAtk;
    public int FinalDef;
    public int FinalSpeed;

    [Header("StatsBuff")]
    public int BuffAtk;
    public int BuffDef;
    public int BuffSpeed;

    [Header("StatsOthers")]
    public int lvl = 1;
    public int currentHP;
    public int currentXP;

    [Header("AttackRelated")]
    public List<int> CurrentPP;
    public List<int> TimesUsedInARow;


    public Animator PokemonAnimations;

    public int attackIndex = -1;
    public bool pokemonUpdated = false;

    // Start is called before the first frame update
    private void Awake()
    {

    }
    void Start()
    {
        battleSceneManager = FindObjectOfType<BattleSceneManager>();
    }

    // Update is called once per frame
    void Update()
    {
        PokemonAnimations.SetBool("isPlayerPokemon", isPlayerPokemon);

        if (pokemon != null)
        {
            if (pokemonUpdated == false)
            {
                PokemonBattleStartUpdate();
            }
            else
            {
                UpdatePokemonStats();
            }
        }
    }

    public void PokemonBattleStartUpdate()
    {
        ListAttacks.Add(pokemon.attack0);
        CurrentPP.Add(pokemon.attack0.MaxPP);
        TimesUsedInARow.Add(0);
        ListAttacks.Add(pokemon.attack1);
        CurrentPP.Add(pokemon.attack1.MaxPP);
        TimesUsedInARow.Add(0);
        ListAttacks.Add(pokemon.attack2);
        CurrentPP.Add(pokemon.attack2.MaxPP);
        TimesUsedInARow.Add(0);
        ListAttacks.Add(pokemon.attack3);
        CurrentPP.Add(pokemon.attack3.MaxPP);
        TimesUsedInARow.Add(0);

        SetHPToMax();
        SetPPToMax();

        pokemonUpdated = true;
    }

    public void InputAttackCommand(int i)
    {

        attackIndex = i;
        battleSceneManager.BattleProgression();
    }

    public void AttackCommand()
    {
        if (isPlayerPokemon)
        {
            PokemonAnimations.Play(ListAttacks[attackIndex].animationNamePlayer);
            CurrentPP[attackIndex]--;
        }
        else
        {
            PokemonAnimations.Play(ListAttacks[attackIndex].animationNameEnemy);
            CurrentPP[attackIndex]--;
            TimesUsedInARow[attackIndex]++;
            for (int i = 0; i < ListAttacks.Count; i++)
            {
                if (i != attackIndex)
                {
                    TimesUsedInARow[i] = 0;
                }
            }
        }
    }

    public void SetHPToMax()
    {
        currentHP = GetFinalHp();
    }

    public void SetPPToMax()
    {
        for (int cPP = 0; cPP < CurrentPP.Count; cPP++)
        {
            CurrentPP[cPP] = ListAttacks[cPP].MaxPP;
        }
    }

    public void UpdatePokemonStats()
    {
        PokemonName = pokemon.name;
        FinalHP = pokemon.BaseHP + (pokemon.MultHP * lvl);
        FinalAtk = pokemon.BaseAtk + BuffAtk + (pokemon.MultAtk * lvl);
        FinalDef = pokemon.BaseDef + BuffDef + (pokemon.MultDef * lvl);
        FinalSpeed = pokemon.BaseSpeed + BuffSpeed + (pokemon.MultSpeed * lvl);
    }

    public int GetFinalHp()
    {
        FinalHP = pokemon.BaseHP + (pokemon.MultHP * lvl);
        return FinalHP;
    }

    public int GetFinalAtk()
    {
        FinalAtk = pokemon.BaseAtk + BuffAtk + (pokemon.MultAtk * lvl);
        return FinalAtk;
    }

    public int GetFinalDef()
    {
        FinalDef = pokemon.BaseDef + BuffDef + (pokemon.MultDef * lvl);
        return FinalDef;
    }

    public int GetFinalSpeed()
    {
        FinalSpeed = pokemon.BaseSpeed + BuffSpeed + (pokemon.MultSpeed * lvl);
        return FinalSpeed;
    }

    public Attacks GetAttack()
    {
        return ListAttacks[attackIndex];
    }

    public void Initiate(Pokemon pkmn, bool isPlayer, int _lvl)
    {
        pokemon = pkmn;
        isPlayerPokemon = isPlayer;
        GameObject position;
        if (isPlayerPokemon == true)
        {
            gameObject.GetComponent<Image>().sprite = pkmn.poke1;
            position = GameObject.Find("PlayerPokemon");
            transform.parent = position.transform;
            transform.position = position.transform.position;

        }
        else
        {
            gameObject.GetComponent<Image>().sprite = pkmn.poke2;
            position = GameObject.Find("EnemyPokemon");
            transform.parent = position.transform;
            transform.position = position.transform.position;
        }
        lvl = _lvl; 
    }

    public void Initiate(PokemonScript pkmScript)
    {
        pokemon = pkmScript.pokemon;
        isPlayerPokemon = pkmScript.isPlayerPokemon;
        GameObject position;
        if (isPlayerPokemon == true)
        {
            gameObject.GetComponent<Image>().sprite = pkmScript.pokemon.poke1;
            position = GameObject.Find("PlayerPokemon");
            transform.parent = position.transform;
            transform.position = position.transform.position;

        }
        else
        {
            gameObject.GetComponent<Image>().sprite = pkmScript.pokemon.poke2;
            position = GameObject.Find("EnemyPokemon");
            transform.parent = position.transform;
            transform.position = position.transform.position;
        }
        lvl = pkmScript.lvl;
    }

}