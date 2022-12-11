using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class InteractSquare : MonoBehaviour
{

    public bool isObstacle = false;
    public NPCScript npc;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            isObstacle = true;
        }
        if (collision.gameObject.tag == "NPC")
        {
            isObstacle = true;
            npc = collision.gameObject.GetComponent<NPCScript>();
        }

        //if (MovementController.Instance.interactBox == this)
        //{
        //    if (collision.gameObject.tag == "NPC")
        //    {
        //        MovementController.Instance.activeNpc = collision.gameObject.GetComponent<NPCScript>();
        //    }
        //    else
        //    {
        //        MovementController.Instance.activeNpc = null;
        //    }
        //}


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            isObstacle = false;
        }
        if (collision.gameObject.tag == "NPC")
        {
            isObstacle = false;
            npc = null;
        }

    }
}
