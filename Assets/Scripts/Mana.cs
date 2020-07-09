using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mana : MonoBehaviour
{
    [SerializeField] Text greenManaText;
    [SerializeField] Text blueManaText;

    int blueManaMax = 0;
    int blueManaAct = 0;
    int greenManaMax = 0;
    int greenManaAct = 0;

    private void Start()
    {
        //blueManaText.text = "Mana: " + blueManaAct + "/" + blueManaMax;
        //greenManaText.text = "Mana: " + greenManaAct + "/" + greenManaMax;
    }

    public void IncreaseBlueManaMax()
    {
        if (blueManaMax < 10)
            blueManaMax++;
        RefreshBlueMana();
        blueManaText.text = "Mana: " + blueManaAct + "/" + blueManaMax;
    }
    public void IncreaseGreenManaMax()
    {
        if (greenManaMax < 10)
            greenManaMax++;
        RefreshGreenMana();
        greenManaText.text = "Mana: " + greenManaAct + "/" + greenManaMax;
    }
    private void RefreshGreenMana()
    {
        greenManaAct = greenManaMax;
    }
    private void RefreshBlueMana()
    {
        blueManaAct = blueManaMax;
    }
    public bool CheckManaValue(int value, bool isCardBlue)
    {
        if(isCardBlue)
        {
            if (value <= blueManaAct)
            {
                blueManaAct -= value;
                blueManaText.text = "Mana: " + blueManaAct + "/" + blueManaMax;
                return true;
            }
            else
                return false;
        }
        else
        {
            if (value <= greenManaAct)
            {
                greenManaAct -= value;
                greenManaText.text = "Mana: " + greenManaAct + "/" + greenManaMax;
                return true;
            }
            else
                return false;
        }
    }

}
