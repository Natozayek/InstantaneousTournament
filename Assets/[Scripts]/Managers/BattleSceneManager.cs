using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneManager : MonoBehaviour
{
    public static BattleSceneManager Instance { get; private set; }

    public List<GameObject> PokemonSlotInBattle;
    public GameObject BattleMenuScreen;
    public GameObject MainBattleMenu;
    public GameObject AttackBattleMenu;
    public MovementController playerGameObject;

    public PokemonInventory _pokemonInventory;

    public bool InBattleProgresion = false;
    bool battleEnds = false;
    public bool hasToChangePokemon = false;

    //public Button AttackButton;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _pokemonInventory = GameObject.FindObjectOfType<PokemonInventory>();
        playerGameObject = GameObject.FindObjectOfType<MovementController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToogleBattleMenu()
    {
        if(BattleMenuScreen.activeInHierarchy == true)
        {
            BattleMenuScreen.gameObject.SetActive(false);
        }
        else
        {
            BattleMenuScreen.gameObject.SetActive(true);
        }
    }

    public void ToAttackMenu()
    {
        if (InBattleProgresion == false)
        {
            MainBattleMenu.SetActive(false);
            AttackBattleMenu.SetActive(true);
        }
    }
    
    public void ToMainMenu()
    {
        if (InBattleProgresion == false)
        {
            MainBattleMenu.SetActive(true);
            AttackBattleMenu.SetActive(false);
        }
    }

    public void flee()
    {
        if (InBattleProgresion == false)
        {
            if (gameObject.activeSelf == true)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void BattleProgression()
    {
        if (InBattleProgresion == false)
        {
            ToMainMenu();
            InBattleProgresion = true;
            StartCoroutine(E_BattleProgression());
        }
    }

    public void CaptureProgression()
    {
        if(_pokemonInventory.HasPokeballs() && _pokemonInventory.CanCapture() == true)
        {
            if (InBattleProgresion == false)
            {
                PokemonScript enemyPokemon = PokemonSlotInBattle[1].GetComponent<PokemonSlot>().GetPokemon();
                InBattleProgresion = true;
                _pokemonInventory.UsePokeBall();
                StartCoroutine(CaptureAttempt(enemyPokemon));
            }
        }

    }

    bool Capture(PokemonScript wildPokemon)
    {
        float captureChance = (((1 + ((float)wildPokemon.FinalHP * 3 - (float)wildPokemon.currentHP * 2) * (float)wildPokemon.pokemon.CatchRate ) / ((float)wildPokemon.FinalHP * 3)) / 256) * 100;
        int finalCapturePercent = (int)captureChance;

        int chances = Random.Range(1, 101);

        Debug.Log("CAPTURE CHANCES = " + finalCapturePercent);
        if(finalCapturePercent > chances)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void changePokemonBattle()
    {
        if(InBattleProgresion == false)
        {
            InBattleProgresion = true;
            StartCoroutine(changePokemon());
        }
    }

    public void changePokemonKockOut()
    {
        _pokemonInventory.TooglePlayerMenuInBattle();
    }

    public void Attack(PokemonScript Attacker, PokemonScript Defender, bool isPlayer)
    {
        if (isPlayer == false)
        {
            Attacker.attackIndex = EnemyAttackAI(Attacker, Defender);
        }
        Attacker.AttackCommand();
        if (Attacker.GetAttack().isOffensive)
        {
            DamageCalculation(Defender, Attacker);
        }
        else if (Attacker.GetAttack().isBuff)
        {
            Buff(Attacker);
        }
        else if (Attacker.GetAttack().isHeal)
        {
            Heal(Attacker);
        }
    }

    IEnumerator E_BattleProgression()
    {
        bool isPlayerAttackingFirst;
        PokemonScript playerPokemon = PokemonSlotInBattle[0].GetComponent<PokemonSlot>().GetPokemon();
        PokemonScript enemyPokemon = PokemonSlotInBattle[1].GetComponent<PokemonSlot>().GetPokemon();

        //Check Who Is Going First
        if (PokemonSlotInBattle[0].GetComponent<PokemonSlot>().GetPokemon().FinalSpeed > PokemonSlotInBattle[1].GetComponent<PokemonSlot>().GetPokemon().FinalSpeed)
        {
            isPlayerAttackingFirst = true;
        }
        else if (PokemonSlotInBattle[1].GetComponent<PokemonSlot>().GetPokemon().FinalSpeed > PokemonSlotInBattle[0].GetComponent<PokemonSlot>().GetPokemon().FinalSpeed)
        {
            isPlayerAttackingFirst = false;
        }
        else
        {
            int rand = Random.Range(1, 3);
            if (rand == 1)
            {
                isPlayerAttackingFirst = true;
            }
            else
            {
                isPlayerAttackingFirst = false;
            }
        }

        if (isPlayerAttackingFirst == true)
        {
            Debug.Log("Defender Attack");

            Attack(playerPokemon, enemyPokemon, true);

            if (enemyPokemon.currentHP <= 0)
            {
                //Check if it is a Trainer Battle
                StartCoroutine(WildBattleTermination(playerPokemon, enemyPokemon));
            }
            else
            {
                yield return new WaitForSeconds(1);
                Attack(enemyPokemon, playerPokemon, false);
                if (playerPokemon.currentHP <= 0)
                {
                    if (_pokemonInventory.GetPokemonsAlive() == 0)
                    {
                        //wiped
                    }
                    else
                    {
                        hasToChangePokemon = true;
                        InBattleProgresion = false;
                        changePokemonKockOut();
                    }
                }
            }
        }
        else if (isPlayerAttackingFirst == false)
        {
            Debug.Log("Enemy Attack");
            Attack(enemyPokemon, playerPokemon, false);

            if (playerPokemon.currentHP <= 0)
            {
                if (_pokemonInventory.GetPokemonsAlive() == 0)
                {
                    //wiped
                }
                else
                {
                    hasToChangePokemon = true;
                    InBattleProgresion = false;
                    changePokemonKockOut();
                }
            }
            else
            {
                yield return new WaitForSeconds(1);
                Attack(playerPokemon, enemyPokemon, true);
                if (enemyPokemon.currentHP <= 0)
                {
                    //Check if it is a Trainer Battle
                    StartCoroutine(WildBattleTermination(playerPokemon, enemyPokemon));
                }
            }    
        }

        if (battleEnds == false)
        {
            yield return new WaitForSeconds(1);
            InBattleProgresion = false;
        }
    }

    int EnemyAttackAI(PokemonScript Enemy, PokemonScript Player)
    {
        List<int> AttackValues = new List<int>();
        List<int> AttackDamage = new List<int>();

        for (int AttackList = 0; AttackList < 4; AttackList++)
        {
            int attackValue = 0;

            if (Enemy.ListAttacks[AttackList].isEmpty == false)
            {
                if (Enemy.CurrentPP[AttackList] > 0)
                {
                    if (Enemy.currentHP >= Enemy.FinalHP * 0.8f)
                    {
                        if (Enemy.ListAttacks[AttackList].isHeal == true) { attackValue += -1; }
                        if (Enemy.ListAttacks[AttackList].isBuff == true) { attackValue += 3; }
                    }
                    else if (Enemy.currentHP <= Enemy.FinalHP * 0.2f)
                    {
                        if (Enemy.ListAttacks[AttackList].isHeal == true) { attackValue += 3; }
                        if (Enemy.ListAttacks[AttackList].isBuff == true) { attackValue += -1; }
                    }

                    if (Enemy.ListAttacks[AttackList].isOffensive)
                    {
                        if (Player.currentHP <= Player.currentHP * 0.3f) { attackValue += 1; }
                        attackValue += isEffective(Enemy.ListAttacks[AttackList], Player.pokemon);
                    }
                    attackValue -= Enemy.TimesUsedInARow[AttackList] * 2;
                }
                else
                {
                    attackValue = -100;
                }
            }
            else
            {
                attackValue = -100; //Just in case
            }
            AttackDamage.Add(Enemy.ListAttacks[AttackList].Damage);
            AttackValues.Add(attackValue);
        }

        for (int attackI = 0; attackI < AttackValues.Count; attackI++)
        {
            for (int othersAttacks = 0; othersAttacks < AttackValues.Count; othersAttacks++)
            {
                if (attackI != othersAttacks)
                {
                    if (AttackDamage[attackI] != -1 || AttackDamage[othersAttacks] != -1)
                    {
                        if (AttackDamage[attackI] > AttackDamage[othersAttacks]) { AttackValues[attackI] += 1; }
                    }
                }
            }

        }
        int finalValue = 0;
        int choosenAttack = -1;
        for (int i = 0; i < AttackValues.Count; i++)
        {
            Debug.Log(AttackValues[i]);
            if (AttackValues[i] > finalValue)
            {
                finalValue = AttackValues[i];
                choosenAttack = i;
            }
            else if (AttackValues[i] == finalValue)
            {
                int random = Random.Range(0, 2);
                if (random == 1)
                {
                    finalValue = AttackValues[i];
                    choosenAttack = i;
                }
            }
        }

        return choosenAttack;
    }

    int isEffective(Attacks attack, Pokemon Defender)
    {
        int effective = 3;
        int normal = 0;
        int nonEffective = -3;


        if (attack.Type == PokemonTypes.FIRE && Defender.Type == PokemonTypes.GRASS)
        {
            return effective;
        }
        else if (attack.Type == PokemonTypes.GRASS && Defender.Type == PokemonTypes.FIRE)
        {
            return effective;
        }
        else if (attack.Type == PokemonTypes.WATER && Defender.Type == PokemonTypes.FIRE)
        {
            return effective;
        }
        else if (attack.Type == PokemonTypes.FIRE && Defender.Type == PokemonTypes.WATER)
        {
            return nonEffective;
        }
        else if (attack.Type == PokemonTypes.GRASS && Defender.Type == PokemonTypes.FIRE)
        {
            return nonEffective;
        }
        else if (attack.Type == PokemonTypes.WATER && Defender.Type == PokemonTypes.GRASS)
        {
            return nonEffective;
        }
        else
        {
            return normal;
        }
    }

    float isEffectiveMultiplier(Attacks attack, Pokemon Defender)
    {
        float effective = 2;
        float normal = 1;
        float nonEffective = 0.5f;


        if (attack.Type == PokemonTypes.FIRE && Defender.Type == PokemonTypes.GRASS)
        {
            return effective;
        }
        else if (attack.Type == PokemonTypes.GRASS && Defender.Type == PokemonTypes.FIRE)
        {
            return effective;
        }
        else if (attack.Type == PokemonTypes.WATER && Defender.Type == PokemonTypes.FIRE)
        {
            return effective;
        }
        else if (attack.Type == PokemonTypes.FIRE && Defender.Type == PokemonTypes.WATER)
        {
            return nonEffective;
        }
        else if (attack.Type == PokemonTypes.GRASS && Defender.Type == PokemonTypes.FIRE)
        {
            return nonEffective;
        }
        else if (attack.Type == PokemonTypes.WATER && Defender.Type == PokemonTypes.GRASS)
        {
            return nonEffective;
        }
        else
        {
            return normal;
        }
    }

    private void DamageCalculation(PokemonScript Defender, PokemonScript Attacker)
    {

        float Damage = ((2* Attacker.GetFinalAtk()) / 5 + 2 ) * Attacker.GetAttack().Damage * Attacker.GetFinalAtk() / Defender.GetFinalDef();
        Damage = Damage / 50 + 2;
        Damage = Damage * isEffectiveMultiplier(Attacker.GetAttack(), Defender.pokemon);
        int finalDamage = (int)Damage;

        Defender.currentHP -= finalDamage;
    }

    private void Buff(PokemonScript user)
    {
        int buff = user.GetAttack().Damage;

        switch (user.GetAttack().buffType)
        {
            case BuffType.NONE:
                break;
            case BuffType.BuffAtk:
                user.BuffAtk += buff;
                break;
            case BuffType.BuffDef:
                user.BuffDef += buff;
                break;
            case BuffType.BuffSpeed:
                user.BuffSpeed += buff;
                break;
        }
        user.UpdatePokemonStats();
    }

    private void Heal(PokemonScript user)
    {
        user.currentHP += user.GetAttack().Damage;
        if(user.currentHP >= user.FinalHP)
        {
            user.currentHP = user.FinalHP;
        }
    }

    public GameObject ReturnPokemon()
    {
        return PokemonSlotInBattle[0].GetComponent<PokemonSlot>().GetPokemonObject();
    }

    public void PokemonPlayerFleeSupport()
    {
        PokemonSlotInBattle[0].GetComponent<PokemonSlot>().PlayerFleeSupport();
    }

    IEnumerator WildBattleTermination(PokemonScript player, PokemonScript wildPokemon)
    {
        battleEnds = true;

        yield return new WaitForSeconds(2);


        int expGain = (wildPokemon.pokemon.ExpWorth * wildPokemon.lvl) / 7;
        player.currentXP += expGain;
        ToMainMenu();
        battleEnds = false;
        InBattleProgresion = false;
        playerGameObject.BattleEnds();
        StopAllCoroutines();
        GlobalData.Instance.monney += 50;
    }

    IEnumerator CaptureAttempt(PokemonScript wildPokemon)
    {
        yield return new WaitForSeconds(2);

        bool captured = Capture(wildPokemon);

        if (captured == true)
        {
            _pokemonInventory.AddCapturedPokemon(wildPokemon.gameObject);
            battleEnds = false;
            InBattleProgresion = false;
            ToMainMenu();
            playerGameObject.BattleEnds();
            StopAllCoroutines();
        }
        else
        {
            PokemonScript playerPokemon = PokemonSlotInBattle[0].GetComponent<PokemonSlot>().GetPokemon();
            Debug.Log("Enemy Attack");
            Attack(wildPokemon, playerPokemon, false);

            if (playerPokemon.currentHP < 0)
            {
                //Check if it is a Trainer Battle
                //endBattle
                if (_pokemonInventory.GetPokemonsAlive() == 0)
                {
                    //wiped
                }
                else
                {
                    hasToChangePokemon = true;
                    InBattleProgresion = false;
                    changePokemonKockOut();
                }
            }

            InBattleProgresion = false;
        }
    }

    IEnumerator changePokemon()
    {
        yield return new WaitForSeconds(2);

        GameObject newPokemon = _pokemonInventory.SelectedPokemon;
        PokemonSlotInBattle[0].GetComponent<PokemonSlot>().AddPokemonToSlotPlayer(newPokemon);


        yield return new WaitForSeconds(2);

        if (hasToChangePokemon == true)
        {
            hasToChangePokemon = false;
            InBattleProgresion = false;
        }
        else
        {
            PokemonScript playerPokemon = PokemonSlotInBattle[0].GetComponent<PokemonSlot>().GetPokemon();
            PokemonScript wildPokemon = PokemonSlotInBattle[1].GetComponent<PokemonSlot>().GetPokemon();

            Debug.Log("Enemy Attack");
            Attack(wildPokemon, playerPokemon, false);

            if (playerPokemon.currentHP < 0)
            {
                //Check if it is a Trainer Battle
                //endBattle
                if (_pokemonInventory.GetPokemonsAlive() == 0)
                {
                    //wiped
                }
                else
                {
                    hasToChangePokemon = true;
                    InBattleProgresion = false;
                    changePokemonKockOut();
                }
            }

            InBattleProgresion = false;
        }
    }
}
