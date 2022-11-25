using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Fader : MonoBehaviour
{
   Animator animator;
   
   public static Fader instance;

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
}
