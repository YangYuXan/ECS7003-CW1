using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartInterface : UIBase
{
    private void Awake()
    {
        // Game start
        Register("Button").onClick = onStartGameBtn;
    }

    private void onStartGameBtn(GameObject go, PointerEventData pointerEventData)
    {
        Close();
    }

}
