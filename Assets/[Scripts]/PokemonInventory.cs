using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonInventory : MonoBehaviour
{
    public PokemonSlot PlayerPokemonSlot;

    int maxPokemom = 3;

    public List<GameObject> PokemonInventoryList;
    public GameObject SelectedPokemon;
    public int pokemonIndex = 0;
    public BattleSceneManager battleSceneManager;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPokemonSlot = GameObject.Find("PlayerPokemon").GetComponent<PokemonSlot>();
        battleSceneManager = GameObject.FindObjectOfType<BattleSceneManager>();
        //SelectedPokemon = PokemonInventoryList[0];
    }

    // Update is called once per frame
    void Update()
    {
        SelectedPokemon = PokemonInventoryList[pokemonIndex];
    }

    public void ChoosePokemon()
    {
        battleSceneManager.PokemonSlotInBattle[0].GetComponent<PokemonSlot>().AddPokemonToSlot(SelectedPokemon);
    }

}
