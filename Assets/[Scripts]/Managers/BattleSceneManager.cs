using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneManager : MonoBehaviour
{
    public List<GameObject> PokemonSlotInBattle;
    public GameObject MainBattleMenu;
    public GameObject AttackBattleMenu;

    public bool InBattleProgresion = false;


    //public Button AttackButton;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToAttackMenu()
    {
        MainBattleMenu.SetActive(false);
        AttackBattleMenu.SetActive(true);
    }
    
    public void ToMainMenu()
    {
        MainBattleMenu.SetActive(true);
        AttackBattleMenu.SetActive(false);
    }

    public void flee()
    {
        if (gameObject.activeSelf == true)
        {
            gameObject.SetActive(false);
        }
    }

    public void BattleProgression()
    {
        if (InBattleProgresion == false)
        {
            InBattleProgresion = true;
            StartCoroutine(E_BattleProgression());
        }
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
            //
        }
        else if (Attacker.GetAttack().isHeal)
        {
            //
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

            yield return new WaitForSeconds(1);

            Attack(enemyPokemon, playerPokemon, false);
        }
        else if (isPlayerAttackingFirst == false)
        {
            Debug.Log("Enemy Attack");
            Attack(enemyPokemon, playerPokemon, false);

            yield return new WaitForSeconds(1);

            Attack(playerPokemon, enemyPokemon, true);
        }

        yield return new WaitForSeconds(1);

        ToMainMenu();
        InBattleProgresion = false;
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

}
