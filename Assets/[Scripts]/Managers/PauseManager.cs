using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    public static bool GameIsPause = false;
    public GameObject PauseMenuUI;

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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void SaveGame()
    {
        DataPersistenceManager.instance.SaveGame();
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPause = false;
    }

    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPause = true;
    }
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        StartCoroutine(GameToMain());
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        StartCoroutine(Quit());
    }

    public IEnumerator GameToMain()
    {
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene("StartScene");
    }
    public IEnumerator Quit()
    {
        yield return new WaitForSeconds(0.3f);
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

    public void Remove()
    {
        Instance = null;
        Destroy(gameObject);
    }

}
