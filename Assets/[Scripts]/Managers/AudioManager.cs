using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public enum TrackID
    {
        inColiseo,
        inTown,
        inCave,
        inCave2,
        inWoods,
        inIsland,
        inBattle,
        inVictory,
        inStart,
        inCredits,

 
    }

    //AUDIO SOURCES
    [Tooltip("Indices to line up tiwh TrackID enum")]
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioSource musicAudioSource2;
    [SerializeField] AudioClip[] musicTracks;



    //Hidden constructor
    private AudioManager() { }
    private static AudioManager instance = null; //Singleton instance
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                SceneManager.sceneLoaded += instance.OnSceneLoadedAction;
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }

        private set { instance = value; }
    }
    

    void Start()
    {

        AudioManager originalManager = Instance;

        AudioManager[] managerClones = FindObjectsOfType<AudioManager>();

        foreach (AudioManager managers in managerClones)
        {
            if (managers != originalManager)
            {
                Destroy(managers.gameObject);
            }
        }

        if (SceneManager.GetActiveScene().name == "Coliseo")
        {
            playTrack(TrackID.inColiseo);
        }
        if (SceneManager.GetActiveScene().name == "StartScene")
        {
            playTrack(TrackID.inStart);
        }

    }

    //Functions 
    //Play a track
    public void playTrack(TrackID track)
    {
        musicAudioSource.Stop();
        musicAudioSource2.Stop();

       musicAudioSource.clip = musicTracks[(int)track];
       musicAudioSource.volume = 1;
       musicAudioSource.Play();
    }


    /// <summary>
    /// Fade from whatever is currently playing to the goalTrack over time /// </summary>
    /// <param name="goalTrack">the new track to play</param>
    /// <param name="transitionInSeconds">total crossfade duration</param>
    public void CrossFadeTO(TrackID goalTrack, float transitionInSeconds = 1.0f)
    {
        AudioSource oldTrack = musicAudioSource;
        AudioSource newTrack = musicAudioSource2;

        if (musicAudioSource.isPlaying)
        {

        }
        else if (musicAudioSource2.isPlaying)
        {
            oldTrack = musicAudioSource2;
            newTrack = musicAudioSource;

        }
        newTrack.clip = musicTracks[(int)goalTrack];
        newTrack.Play();

        StartCoroutine(CrossFadeCoroutine(oldTrack, newTrack, transitionInSeconds));
    }

    private IEnumerator CrossFadeCoroutine(AudioSource oldTrack, AudioSource newTrack, float transitionInSeconds)
    {
        float time = 0.0f;
        while (time < transitionInSeconds)
        {
            float tValue = Mathf.Min(time / transitionInSeconds, 1.0f) ;
        
            newTrack.volume = tValue;
            oldTrack.volume = 1.0f - tValue;

            time += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        oldTrack.Stop();
        oldTrack.volume = 1.0f;
    }


    void OnSceneLoadedAction(Scene newScene, LoadSceneMode mode)
    {
        if (newScene.name == "Coliseo")
        {
            CrossFadeTO(TrackID.inColiseo);
        }
        if (newScene.name == "Cave")
        {
            CrossFadeTO(TrackID.inCave);
        }
        if(newScene.name == "MainScene1")
        {
            CrossFadeTO(TrackID.inTown);
        }
        if (newScene.name == "CaveToWoods")
        {
            CrossFadeTO(TrackID.inCave2);
        }
        if (newScene.name == "Island")
        {
            CrossFadeTO(TrackID.inIsland);
        }
        if (newScene.name == "Woods")
        {
            CrossFadeTO(TrackID.inWoods);
        }

        if (newScene.name == "StartScene")
        {
            CrossFadeTO(TrackID.inStart);
        }
        if (newScene.name == "Credits")
        {
            CrossFadeTO(TrackID.inCredits);
        }

    }

}
