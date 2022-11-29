using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokemonSlot : MonoBehaviour
{
    public GameObject PokemonObject;
    public PokemonScript pokemon;

    public GameObject TestPokemon;
    public StatsScreen stats;

    private void Start()
    {
        //if (TestPokemon != null)
        //{
        //    AddPokemonToSlot(TestPokemon);
        //}
    }

    public void Update()
    {
        if (pokemon != null)
        {
            stats.StatsUpdate(pokemon.PokemonName, pokemon.lvl, pokemon.FinalHP, pokemon.currentHP);
        }
    }

    public void AddPokemonToSlot(GameObject poke)
    {
        if(PokemonObject != null)
        {
            
            pokemon = null;
            Destroy(PokemonObject);
            PokemonObject = null;
        }

        PokemonObject = poke;
        PokemonObject.transform.parent = transform;
        pokemon = PokemonObject.GetComponent<PokemonScript>();
        stats.StatsUpdate(pokemon.PokemonName, pokemon.lvl, pokemon.FinalHP, pokemon.currentHP);
    }

    public PokemonScript GetPokemon()
    {
        return pokemon;
    }

}
