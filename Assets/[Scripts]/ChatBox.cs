using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatBox : MonoBehaviour
{
    public GameObject ChatBoxMenu;
    public TMP_Text chatBoxText;
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
        chatBoxText.text = text;
    }
    public void ChatBoxDeActivate()
    {
        ChatBoxMenu.SetActive(false);
        chatBoxText.text = "";
    }

}
