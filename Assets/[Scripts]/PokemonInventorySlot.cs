using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokemonInventorySlot : MonoBehaviour
{
    public GameObject PokemonObject;
    public PokemonScript PokemonData;
    public Slider HpBar;

    public TMP_Text TextName;
    public TMP_Text TextLvl;
    public TMP_Text TextHpMax;
    public TMP_Text TextHpCurrent;

    public Image pokemonImage;

    // Start is called before the first frame update
    void Start()
    {
        //UpdatePokemonData();
    }

    // Update is called once per frame
    void Update()
    {
        //if(BattleSceneManager.Instance)
        //if(PokemonData.currentHP <= 0)
        //{
        //    pokemonButton.interactable = false;
        //}
        //else
        //{
        //    pokemonButton.interactable = true;
        //}
    }

    public void SetPokemon(GameObject _pokemon)
    {
        PokemonObject = _pokemon;
        PokemonData = PokemonObject.GetComponent<PokemonScript>();
        UpdatePokemonData();
    }

    public void UpdatePokemonData()
    {
        TextName.text = PokemonData.PokemonName;
        TextLvl.text = PokemonData.lvl.ToString();
        TextHpMax.text = PokemonData.FinalHP.ToString();
        TextHpCurrent.text = PokemonData.currentHP.ToString();

        HpBar.maxValue = PokemonData.FinalHP;
        HpBar.value = PokemonData.currentHP;

        pokemonImage.sprite = PokemonData.pokemon.poke2;
    }
}
