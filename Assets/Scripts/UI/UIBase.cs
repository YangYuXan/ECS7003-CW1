using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    // Event Register
    public UIEventTrigger Register(string uiName)
    {
        Transform tf = transform.Find(uiName);
        return UIEventTrigger.Get(tf.gameObject);
    }
    public virtual void Show()    // Show
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()    // Hide
    {
        gameObject.SetActive(false);
    }

    public virtual void Close()    // Close(Destroy)
    {
        Debug.Log(gameObject.name);
        UIManager.Instance.CloseUI(gameObject.name);
    }

}
