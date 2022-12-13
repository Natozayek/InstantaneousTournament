using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public void GoBackToStart()
    {
        MovementController.Instance.Remove();
        StoryProgression.Instance.Remove();
        BattleSceneManager.Instance.Remove();
        GlobalData.Instance.Remove();
        PauseManager.Instance.Remove();
        SceneManager.LoadSceneAsync("StartScene");
    }
}
