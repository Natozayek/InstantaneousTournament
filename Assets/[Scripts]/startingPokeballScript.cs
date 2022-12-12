using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startingPokeballScript : MonoBehaviour
{
    public GameObject pokemon;
    public PokemonInventory PokemonInventory;
    
    public void Choose()
    {
        PokemonInventory.AddCapturedPokemon(pokemon);
    }

    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
