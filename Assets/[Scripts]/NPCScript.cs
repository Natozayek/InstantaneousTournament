using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    public bool isProfesor = false;
    public bool isShop = false;
    public string mainChat = "";
    public string chat1 = "";
    public string chat2 = "";
    void Start()
    {
        mainChat = chat1;
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
    }
}
