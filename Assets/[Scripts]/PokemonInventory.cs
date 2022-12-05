using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonInventory : MonoBehaviour
{
    public PokemonSlot PlayerPokemonSlot;

    int maxPokemom = 3;

    public GameObject playerMenu;

    public List<GameObject> PokemonInventoryList;
    public List<PokemonInventorySlot> PokemonInventoryListMenu;

    public GameObject SelectedPokemon;
    public int pokemonIndex = 0;
    public BattleSceneManager battleSceneManager;
    public bool inMenu;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPokemonSlot = GameObject.Find("PlayerPokemon").GetComponent<PokemonSlot>();
        battleSceneManager = GameObject.FindObjectOfType<BattleSceneManager>();

    }

    // Update is called once per frame
    void Update()
    {
        SelectedPokemon = PokemonInventoryList[pokemonIndex];
        if(playerMenu.active == true)
        {
            UpdatePokemonSlotMenu();
        }

        inMenu = playerMenu.active;
    }

    public void TooglePlayerMenu()
    {
        if (inMenu == true)
        {
            playerMenu.SetActive(false);
        }
        else
        {
            playerMenu.SetActive(true);
        }
    }

    public void UpdatePokemonSlotMenu()
    {
        for (int i = 0; i < PokemonInventoryList.Count; i++)
        {
            PokemonInventoryListMenu[i].SetPokemon(PokemonInventoryList[i]);
        }
    }

    public void ChoosePokemon()
    {
        battleSceneManager.PokemonSlotInBattle[0].GetComponent<PokemonSlot>().AddPokemonToSlot(SelectedPokemon);
    }

}
