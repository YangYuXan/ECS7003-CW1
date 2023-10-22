using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Event Listener
public class UIEventTrigger : MonoBehaviour, IPointerClickHandler
{
    public Action<GameObject, PointerEventData> onClick;

    public static UIEventTrigger Get(GameObject go)
    {
        UIEventTrigger trigger = go.GetComponent<UIEventTrigger>();
        if(trigger == null)
        {
            trigger = go.AddComponent<UIEventTrigger>();
            Debug.Log("get");
        }
        return trigger;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(onClick != null)
        {
            onClick(gameObject, eventData);
            Debug.Log("click");
        }
    }

}
