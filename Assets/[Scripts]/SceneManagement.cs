using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagement : MonoBehaviour
{
   
  
    public void GoToMainSceneCoroutine()
    {
        StartCoroutine(GoToMainScene());
    }
    public void GoToStartSceneCoroutine()
    {
        StartCoroutine(GoToStartScene());
    }
    public void GoToContinueCoroutine()
    {
        StartCoroutine(GoToContinue());
    }
    public void GoToCreditsCoroutine()
    {
        StartCoroutine(GoToCredits());
    }

    public void GoToQuit()
    {
        StartCoroutine(Exit());
    }

    private IEnumerator GoToMainScene()
    {
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene("MainScene1");
    }
    private IEnumerator GoToContinue()
    {
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene("MainScene1");
    }

   
    private IEnumerator Exit()
    {
        yield return new WaitForSeconds(0.2f);
        
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
         Application.OpenURL(webplayerQuitURL);
#else
         Application.Quit();
#endif
        }

    }

    private IEnumerator GoToCredits()
    {
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene("Credits");
    }

    private IEnumerator GoToStartScene()
    {
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene("StartScene");
    }

}
