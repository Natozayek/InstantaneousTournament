using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Fader : MonoBehaviour
{
   Animator animator;
   
   public static Fader instance;
   public AudioManager audioManager;

   private void Awake()
    {
        animator = GetComponent<Animator>();
        instance = this;

    }

   private void Start()
   {
        fadeOut();
   }


    public void fadeOut()
    {
        animator.SetTrigger("FadeOut");
    }

    public void fadeIn()
    {
        animator.SetTrigger("FadeIn");
    }

    public void fadeInBattle()
    {
        animator.SetTrigger("FadeInBattle");
    }

    public IEnumerator ResetAnim()
    {
        yield return new WaitForSeconds(0.1f);
        animator.SetTrigger("Reset");
    }

    public IEnumerator GoToCaveCoro()
    {
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene("Cave");
        fadeOut();
    }
    public IEnumerator GoToTownCoro()
    {
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene("MainScene1");
        fadeOut();
    }

    public IEnumerator GoToCaveWoodsCoro()
    {
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene("CaveToWoods");
        fadeOut();
    }
    public IEnumerator GoToWoodsCoro()
    {
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene("Woods");
        fadeOut();
    }
    public IEnumerator GoToColiseoCoro()
    {
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene("Coliseo");
        fadeOut();  
    }
    public IEnumerator GoToIslandCoro()
    {
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene("Island");
        fadeOut();
    }


}
