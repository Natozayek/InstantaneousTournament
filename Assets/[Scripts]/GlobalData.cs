using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public static GlobalData Instance { get; private set; }

    public List<Trainer> trainerList;
    public GameObject trainer1;
    public GameObject trainer2;

    public bool firstCharacterGeneration = true;
    public int monney = 200;

    public int TimeChoosed;

    public int Min = 0;
    public int Sec = 0;

    public int randomPlayerLeftOut = -0;
    public bool beatTrainer1 = false;
    public bool TournamentTime = false;

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
        Randomize();
    }

    public void Randomize()
    {
        randomPlayerLeftOut = Random.Range(0, 3);
        trainerList[randomPlayerLeftOut].DeletePokemon();
        Destroy(trainerList[randomPlayerLeftOut].gameObject);
        trainerList.RemoveAt(randomPlayerLeftOut);
        int randomN = Random.Range(0, 2);
        if(randomN == 0)
        {
            trainer1 = trainerList[0].gameObject;
            trainer2 = trainerList[1].gameObject;
        }
        else
        {
            trainer1 = trainerList[1].gameObject;
            trainer2 = trainerList[0].gameObject;
        }
    }

    public void Update()
    {
        if(MovementController.Instance.canMove == true && Min <= 0 && Sec <= 0)
        {
            TournamentTime = true;
        }
    }
}
