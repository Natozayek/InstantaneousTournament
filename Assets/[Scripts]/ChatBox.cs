using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatBox : MonoBehaviour
{
    public GameObject ChatBoxMenu;
    public TMP_Text chatBoxText;
    public GameObject ButtonSet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChatBoxActivate(string text)
    {
        ChatBoxMenu.SetActive(true);
        ButtonSet.SetActive(false);
        chatBoxText.text = text;
    }

    public void ChatBoxActivateShop(string text)
    {
        ChatBoxMenu.SetActive(true);
        ButtonSet.SetActive(true);
        chatBoxText.text = text;
    }
    public void ChatBoxDeActivate()
    {
        ChatBoxMenu.SetActive(false);
        chatBoxText.text = "";
    }

    public void Buy()
    {
        GlobalData.Instance.monney -= 200;
        MovementController.Instance.pokeballsOwned++;
        ChatBoxDeActivate();
        MovementController.Instance.canMove = true;
    }

    public void Cancel()
    {
        ChatBoxDeActivate();
        MovementController.Instance.canMove = true;
    }
}
