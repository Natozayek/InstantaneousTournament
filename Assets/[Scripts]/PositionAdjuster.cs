using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionAdjuster : MonoBehaviour
{
    public List<GameObject> l_Positions;
    public int positionListIndex;
    public GameObject player;
    public PositionChangeEnum positionChange;
    public bool positionChanged = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player");

        positionChange = player.GetComponent<MovementController>().positionChange;

        switch (positionChange)
        {
            case PositionChangeEnum.NONE:
                break;
            case PositionChangeEnum.MAINTOCAVE:
                positionListIndex = 0;
                break;
            case PositionChangeEnum.CAVETOMAIN:
                positionListIndex = 0;
                break;
            case PositionChangeEnum.COLISEUMTOMAIN:
                positionListIndex = 1;
                break;
            case PositionChangeEnum.CAVE2TOMAIN:
                positionListIndex = 2;
                break;
            case PositionChangeEnum.ISLANDTOMAIN:
                positionListIndex = 3;
                break;
            case PositionChangeEnum.FORESTTOCAVE2:
                positionListIndex = 1;
                break;
            case PositionChangeEnum.MAINTOCOLISEUM:
                positionListIndex = 0;
                break;
            case PositionChangeEnum.MAINTOCAVE2:
                positionListIndex = 0;
                break;
            case PositionChangeEnum.MAINTOISLAND:
                positionListIndex = 0;
                break;
            case PositionChangeEnum.CAVE2TOFOREST:
                positionListIndex = 0;
                break;
            case PositionChangeEnum.DEFEAT:
                positionListIndex = 1;
                break;
            case PositionChangeEnum.TOURNAMENT:
                positionListIndex = 2;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalData.Instance.firstCharacterGeneration == true)
        {
            positionChanged = true;
            GlobalData.Instance.firstCharacterGeneration = false;
        }
        if(positionChanged == false)
        {
            if (player.transform.position != l_Positions[positionListIndex].transform.position)
            {
                player.transform.position = l_Positions[positionListIndex].transform.position;
            }
            else
            {
                player.GetComponent<MovementController>().canMove = true;
                positionChanged = true;
            }
        }
    }
}
