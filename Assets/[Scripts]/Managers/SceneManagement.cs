using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagement : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;

    public GameObject TimeChooseGameObject;

    

    public void OnNewGameClicked()
    {
        DisableButtons();
        StartCoroutine(GoToMainScene());

    }
    public void GoToMainMenu()
    {
        StartCoroutine(GoToMainMenuCoro());
    }
    public void OnContinueGameClicked()
    {
        DisableButtons();
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
        DataPersistenceManager.instance.isThereAnyDataSaved = false;
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadSceneAsync("Coliseo");

    }
    private IEnumerator GoToContinue()
    {
        DataPersistenceManager.instance.isThereAnyDataSaved = true;
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadSceneAsync("Coliseo");

    }

    public void timeToChoose()
    {
        TimeChooseGameObject.SetActive(true);
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
        SceneManager.LoadSceneAsync("Credits");
    }

    private IEnumerator GoToMainMenuCoro()
    {
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadSceneAsync("StartScene");
    }

    private void DisableButtons()
    {
        newGameButton.interactable = false;
        continueButton.interactable = false;
    }
}
