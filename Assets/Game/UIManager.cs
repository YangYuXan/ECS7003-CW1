using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI UI_Action;
    public void Action_0()
    {
        UI_Action.text = "Action: 0";
        Debug.Log("take action 0");
    }

    public void Action_1()
    {
        UI_Action.text = "Action: 1";
        Debug.Log("take action 1");
    }

    public void Action_2()
    {
        UI_Action.text = "Action: 2";
        Debug.Log("take action 2");
    }

    public void Action_3()
    {
        UI_Action.text = "Action: 3";
        Debug.Log("take action 3");
    }
}
