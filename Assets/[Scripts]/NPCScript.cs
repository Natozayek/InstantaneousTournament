using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    public bool isProfesor = false;
    public bool isShop = false;
    public bool isPokeball = false;

    public GameObject pokemon;


    public string mainChat = "";
    public string chat1 = "";
    public string chat2 = "";
    void Start()
    {
        mainChat = chat1;
        if(isPokeball == false)
        {
            pokemon = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isShop == true)
        {
            if(GlobalData.Instance.monney >= 200)
            {
                mainChat = chat1;
            }    
            else
            {
                mainChat = chat2;
            }
        }
        if(isPokeball == true)
        {
            if(MovementController.Instance.hasStartingPokemon == true)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
