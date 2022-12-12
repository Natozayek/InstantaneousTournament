using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trainer : MonoBehaviour
{
    public List<GameObject> pokemonList;
    public BattleSceneManager battleSceneManager;
    public int levelDesired;
    public int activePokemon = 2;
    public GameObject SelectedPokemon;
    // Start is called before the first frame update
    void Start()
    {
        SelectedPokemon = pokemonList[activePokemon];

        battleSceneManager = FindObjectOfType<BattleSceneManager>();

        if (GlobalData.Instance.TimeChoosed == 5)
        {
            levelDesired = 5;
        }
        else if(GlobalData.Instance.TimeChoosed == 10)
        {
            levelDesired = 8;
        }
        else
        {
            levelDesired = 10;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(SelectedPokemon != pokemonList[activePokemon])
        {
            SelectedPokemon = pokemonList[activePokemon];
            ChoosePokemon();
        }

        foreach (GameObject pokemon in pokemonList)
        {
            pokemon.GetComponent<PokemonScript>().lvl = levelDesired;
        }
    }

    public void DeletePokemon()
    {
        foreach (GameObject p in pokemonList)
        {
            Destroy(p);
        }
    }

    public void SwitchPokemon()
    {
        Destroy(SelectedPokemon);
        SelectedPokemon = null;
        pokemonList.RemoveAt(activePokemon);
        activePokemon--;
    }

    public void ChoosePokemon()
    {
        SelectedPokemon.GetComponent<PokemonScript>().SetHPToMax();
        battleSceneManager.PokemonSlotInBattle[1].GetComponent<PokemonSlot>().AddPokemonToSlot(SelectedPokemon);
    }
}
