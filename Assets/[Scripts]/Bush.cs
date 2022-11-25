using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    //Get the Player
    public GameObject Player;
    public GameObject PokemonPrefab;
    //To get the Player MovementController Script
    MovementController PlayerController;
    [Header("TypesOfPokemon \n ThatMayAppear")]
    public List<Pokemon> PokemonListFromBush;
    [Header("% of Appearing \n Element = PokemonListFromBush Element \n Must be 100 in total")]
    public List<int> PokemonListFromBushChancesOfAppearing;
    [Header("Final Pokemon List, Leave Empty")]
    public List<Pokemon> pokemonListFinal;
    public BattleSceneManager battleSceneManager;

    // Start is called before the first frame update
    void Start()
    {
        //Getting the Script
        PlayerController = Player.GetComponent<MovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") //if the object colliding with the trigger has Player tag
        {
            PlayerController.inBush = true; //Change inBush value from player to true
            Debug.Log("On Bush: " + PlayerController.inBush);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") //if it is no longer on bush
        {
            PlayerController.inBush = false; //Change inBush value from player to false
            Debug.Log("On Bush: " + PlayerController.inBush);
        }
    }

    public void Encounter()
    {
        if(pokemonListFinal.Count != 0)
        {
            pokemonListFinal.Clear();
        }

        int random = Random.Range(1, 101);
        int maxChance = 100;
        int i = 0;

        while (pokemonListFinal.Count < maxChance)
        {
            for (int x = 0; x < PokemonListFromBushChancesOfAppearing[i]; x++)
            {
                pokemonListFinal.Add(PokemonListFromBush[i]);
            }
            i++;
        }
        Pokemon selectedPokemon = pokemonListFinal[random];
        GameObject newPokemon = Instantiate(PokemonPrefab);
        newPokemon.GetComponent<PokemonScript>().Initiate(selectedPokemon, false, 1);
        battleSceneManager.GetComponent<BattleSceneManager>().PokemonSlotInBattle[1].GetComponent<PokemonSlot>().AddPokemonToSlot(newPokemon);

        //battleSceneManager.PokemonBattle(selectedPokemon);

    }
}
