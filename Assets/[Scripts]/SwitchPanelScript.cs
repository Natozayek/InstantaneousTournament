using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchPanelScript : MonoBehaviour
{
    public PokemonInventory inventory;

    public Image pokemonSprite;

    public TMP_Text DataName;
    public TMP_Text DataType;
    public TMP_Text DataHpTotal;
    public TMP_Text DataHpCurrent;
    public TMP_Text DataAtk;
    public TMP_Text DataDef;
    public TMP_Text DataSpeed;

    public List<TMP_Text> AttackName;
    public List<TMP_Text> AttackPower;
    public List<TMP_Text> AttackPPTotal;
    public List<TMP_Text> AttackPPCurrent;
    public List<TMP_Text> AttackType;

    public Button yesButton;
    public PokemonScript SelectedPokemon;

    public int screeningPokemonIndex;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(SelectedPokemon.currentHP <= 0)
        {
            yesButton.interactable = false;
        }
        else
        {
            yesButton.interactable = true;
        }
    }

    public void ChangePokemon()
    {
        if (inventory.inBattle == false)
        {
            inventory.ChangeActivePokemon(screeningPokemonIndex);
            inventory.ToogleSwitchPanelMenu(screeningPokemonIndex);
        }
        else
        {
            inventory.ChangeActivePokemon(screeningPokemonIndex);
            inventory.ToogleSwitchPanelMenu(screeningPokemonIndex);
            BattleSceneManager.Instance.changePokemonBattle();
            inventory.TooglePlayerMenu(true);
        }

    }

    public void UpdatePokemonData(int pokemonListIndex)
    {
        screeningPokemonIndex = pokemonListIndex;

        SelectedPokemon = inventory.PokemonInventoryList[pokemonListIndex].GetComponent<PokemonScript>();

        pokemonSprite.sprite = SelectedPokemon.pokemon.poke2;
        DataName.text = SelectedPokemon.PokemonName;
        DataType.text = SelectedPokemon.pokemon.Type.ToString();
        DataHpTotal.text = SelectedPokemon.GetFinalHp().ToString();
        DataHpCurrent.text = SelectedPokemon.currentHP.ToString();
        DataAtk.text = SelectedPokemon.GetFinalAtk().ToString();
        DataDef.text = SelectedPokemon.GetFinalDef().ToString();
        DataSpeed.text = SelectedPokemon.GetFinalSpeed().ToString();

        for (int i = 0; i < 4; i++)
        {
            AttackName[i].text = SelectedPokemon.ListAttacks[i].name;
            AttackPower[i].text = SelectedPokemon.ListAttacks[i].Damage.ToString();
            AttackPPTotal[i].text = SelectedPokemon.ListAttacks[i].MaxPP.ToString();
            AttackPPCurrent[i].text = SelectedPokemon.CurrentPP[i].ToString();
            AttackType[i].text = SelectedPokemon.ListAttacks[i].Type.ToString();
        }
    }
}
