using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsScreen : MonoBehaviour
{
    public TMP_Text TextName;
    public TMP_Text TextLvl;
    public TMP_Text TextHpMax;
    public TMP_Text TextHpCurrent;

    public Slider HpBar;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StatsUpdate(string pokemonName, int pokemonLVL, int pokemonMaxHp, int pokemonCurrentHP)
    {
        TextName.text = pokemonName;
        TextLvl.text = pokemonLVL.ToString();
        TextHpMax.text = pokemonMaxHp.ToString();
        TextHpCurrent.text = pokemonCurrentHP.ToString();

        HpBar.maxValue = pokemonMaxHp;
        HpBar.value = pokemonCurrentHP;
    }

}
