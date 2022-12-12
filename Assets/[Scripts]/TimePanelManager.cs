using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimePanelManager : MonoBehaviour
{
    public TMP_Text min;
    public TMP_Text sec;
    float f_Sec;
    public GameObject panelGameObject;
    // Start is called before the first frame update
    void Start()
    {
        f_Sec = 0;
        GlobalData.Instance.Min = GlobalData.Instance.TimeChoosed;
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalData.Instance.TournamentTime == false)
        {
            PassTime();
        }

        min.text = GlobalData.Instance.Min.ToString();
        sec.text = GlobalData.Instance.Sec.ToString();
    }

    void PassTime()
    {
        if(f_Sec <= -1)
        {
            f_Sec = 59;
            GlobalData.Instance.Min--;
        }

        f_Sec -= Time.deltaTime;
        //f_Sec -= Time.deltaTime;//
        //f_Sec -= Time.deltaTime;//
        GlobalData.Instance.Sec = (int)f_Sec;
    }

    public void ToogleTimePanel()
    {
        if(panelGameObject.active == true)
        {
            panelGameObject.SetActive(false);
        }
        else
        {
            panelGameObject.SetActive(true);
        }
    }
}
