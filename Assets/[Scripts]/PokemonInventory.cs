using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public TMP_Text pokeballCount;
    public MovementController m_MovementController;

    // Start is called before the first frame update
    void Start()
    {
        //PlayerPokemonSlot = GameObject.Find("PlayerPokemon").GetComponent<PokemonSlot>();
        //battleSceneManager = GameObject.FindObjectOfType<BattleSceneManager>();
    }

    // Update is called once per frame
    void Update()
    {
        SelectedPokemon = PokemonInventoryList[pokemonIndex];
        if(playerMenu.active == true)
        {
            UpdatePokemonSlotMenu();
            pokeballCount.text = m_MovementController.pokeballsOwned.ToString();
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

    public int GetPokemonsAlive()
    {
        int pokemonAlive = 0;
        for (int i = 0; i < PokemonInventoryList.Count; i++)
        {
            if (PokemonInventoryList[i].GetComponent<PokemonScript>().currentHP > 0)
            {
                pokemonAlive++;
            }
        }
        return pokemonAlive;
    }

    public bool HasPokeballs()
    {
        if(m_MovementController.pokeballsOwned > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanCapture()
    {
        if(PokemonInventoryList.Count<maxPokemom)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UsePokeBall()
    {
        m_MovementController.pokeballsOwned--;
    }

    public void AddCapturedPokemon(GameObject capturedPokemon)
    {
        GameObject newPokemon = Instantiate(capturedPokemon);
        newPokemon.name = newPokemon.GetComponent<PokemonScript>().pokemon.name;
        newPokemon.GetComponent<PokemonScript>().isPlayerPokemon = true;
        PokemonInventoryList.Add(newPokemon);
        newPokemon.transform.parent = this.gameObject.transform;

    }

}
