using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScript : MonoBehaviour
{
    public void ToMainMenu()
    {
        StartCoroutine(ToMain());
    }

    public IEnumerator ToMain()
    {
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene("StartScene");
    }
}
