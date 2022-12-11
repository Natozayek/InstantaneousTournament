using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public bool inBattle = false;

    public TMP_Text pokeballCount;
    public MovementController m_MovementController;

    public GameObject SwitchPanel;

    public Button GoBack;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(battleSceneManager.hasToChangePokemon == true)
        {
            GoBack.gameObject.SetActive(false);   
        }
        else
        {
            GoBack.gameObject.SetActive(true);
        }

        SelectedPokemon = PokemonInventoryList[pokemonIndex];
        if(playerMenu.active == true)
        {
            UpdatePokemonSlotMenu();
            pokeballCount.text = m_MovementController.pokeballsOwned.ToString();
        }

        inMenu = playerMenu.active;
    }

    public void TooglePlayerMenu(bool _inBattle)
    {
        inBattle = _inBattle;
        if (inMenu == true)
        {
            playerMenu.SetActive(false);
            MovementController.Instance.canMove = true;
        }
        else
        {
            playerMenu.SetActive(true);
            MovementController.Instance.canMove = false;
        }
    }

    public void TooglePlayerMenuInBattle()
    {
        if(battleSceneManager.InBattleProgresion == false)
        {
            inBattle = true;
            if (inMenu == true)
            {
                playerMenu.SetActive(false);
                MovementController.Instance.canMove = true;
            }
            else
            {
                playerMenu.SetActive(true);
                MovementController.Instance.canMove = false;
            }
        }
    }

    public void ToogleSwitchPanelMenu(int i)
    {
        if (SwitchPanel.active == true)
        {
            SwitchPanel.SetActive(false);
        }
        else
        {
            SwitchPanel.SetActive(true);
            SwitchPanel.GetComponent<SwitchPanelScript>().UpdatePokemonData(i);
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

    public void ChangeActivePokemon(int newPokemonIndex)
    {
        pokemonIndex = newPokemonIndex; //
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
        newPokemon.GetComponent<Image>().sprite = newPokemon.GetComponent<PokemonScript>().pokemon.poke1;   
        newPokemon.name = newPokemon.GetComponent<PokemonScript>().pokemon.name;
        newPokemon.GetComponent<PokemonScript>().isPlayerPokemon = true;
        PokemonInventoryListMenu[PokemonInventoryList.Count].gameObject.SetActive(true);
        PokemonInventoryList.Add(newPokemon);
        newPokemon.transform.parent = this.gameObject.transform;
    }

}
