// using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector2 initPos;    // Initial card position
    bool init = true;

    private void Start()
    {
        // initPos = transform.GetComponent<RectTransform>().anchoredPosition;
    }

    // Start drag
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (init)
        {
            initPos = transform.GetComponent<RectTransform>().anchoredPosition;
            init = false;
        }
        // Audio active
        //AudioManager.Instance.PlayEffect("Cards/draw");
    }

    // On drag
    public virtual void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent.GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out pos
        ))
        {
            transform.GetComponent<RectTransform>().anchoredPosition = pos;
        }
    }

    // End drag
    public virtual void OnEndDrag(PointerEventData eventData)
    {
        Vector2 endPosition = Input.mousePosition;
        if(endPosition.y < Screen.height / 3)
        {
            StopAllCoroutines();
            transform.GetComponent<RectTransform>().anchoredPosition = initPos;
            Debug.Log("return card");
        }else
        {
            Debug.Log("play card");
        }
        // transform.GetComponent<RectTransform>().anchoredPosition = initPos;
        // transform.SetSiblingIndex(index);
    }

    // Try use card
    public virtual bool TryUse()
    {
        // Cost need
        int cost = 1;
        if (cost > BattleManager.Instance.CurCostCount)
        {
            // Cost lack
            UIManager.Instance.ShowTip("·ÑÓÃ²»×ã", Color.white);
            return false;
        }
        else
        {
            // Cut cost
            BattleManager.Instance.CurCostCount -= cost;
            // Refresh text
            UIManager.Instance.GetUI<BattleUI>("BattleUI").UpdateCost();
            // Remove card
            UIManager.Instance.GetUI<BattleUI>("BattleUI").RemoveCard(this);
            return true;
        }
    }
}
