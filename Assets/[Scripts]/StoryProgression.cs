using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryProgression : MonoBehaviour
{
    public static StoryProgression Instance { get; private set; }

    public List<string> storyText;
    public List<int> storyTextStop;
    public ChatBox chatBox;
    public int storyIndex = 0;
    public int storyIndexStop = 0;
    public bool onStoryProgression;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        StoryProgresion();
    }

    public void Update()
    {
        if(onStoryProgression == true)
        {
            if(Input.GetKeyDown(KeyCode.Z))
            {
                StoryProgresion();
            }
        }


    }

    public void StoryProgresion()
    {
        onStoryProgression = true;
        chatBox.ChatBoxActivate(storyText[storyIndex]);
        if(storyIndex == storyTextStop[storyIndexStop])
        {
            onStoryProgression = false;
            MovementController.Instance.canMove = true;
            chatBox.ChatBoxDeActivate();
            storyIndexStop++;
        }
        storyIndex++;
        if(storyIndex == 13)
        {
            MovementController.Instance.StartColliseumBattle();
        }
        else if (storyIndex == 16)
        {
            MovementController.Instance.StartColliseumBattle();
        }
        else if (storyIndex == 17)
        {
            GlobalData.Instance.gameClear = true;
            BattleSceneManager.Instance.EndGame();
        }
    }

    public void Remove()
    {
        Instance = null;
        Destroy(gameObject);
    }
}
