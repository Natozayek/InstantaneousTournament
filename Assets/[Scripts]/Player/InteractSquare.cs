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
        if(npc != null)
        { isObstacle = true; }    
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            isObstacle = false;
        }
        else if (collision.gameObject.tag == "NPC")
        {
            isObstacle = false;
            npc = null;
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            isObstacle = true;
        }
        else if (collision.gameObject.tag == "NPC")
        {
            isObstacle = true;
            npc = collision.gameObject.GetComponent<NPCScript>();
        }
    }
}
