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
        //audioManager = GetComponent<AudioManager>();
        audioManager.playTrack(AudioManager.TrackID.inTown);
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

    }
    public IEnumerator GoToTownCoro()
    {
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene("MainScene1");

    }

    public IEnumerator GoToCaveWoodsCoro()
    {
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene("CaveToWoods");

    }

    //public IEnumerator GoToBattle(GameObject BattleScene)
    //{
    //    yield return new WaitForSeconds(0.4f);
    //    BattleScene.SetActive(true);
    //    fadeOut();

    //}
}
