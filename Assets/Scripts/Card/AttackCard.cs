using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttackCard : CardItem//, IPointerDownHandler
{
    // Start is called before the first frame update
    void Start()
    {
        transform.Find("bg").GetComponent<Image>().sprite = Resources.Load<Sprite>("Icon/vampire empty card section 1");
        transform.Find("bg/bgFrame").GetComponent<Image>().sprite = Resources.Load<Sprite>("Icon/card background frame");
        transform.Find("bg/attack").GetComponent<Image>().sprite = Resources.Load<Sprite>("Icon/attack");
        transform.Find("bg/msgTxt").GetComponent<TextMeshProUGUI>().text = string.Format("test");
        transform.Find("bg/nameTxt").GetComponent<TextMeshProUGUI>().text = "Attack";
        transform.Find("bg/useTxt").GetComponent<TextMeshProUGUI>().text = "1";

    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        
        // Stop all coroutine
        StopAllCoroutines();
        // Start mouse coroutine
        StartCoroutine(OnMouseDownRight(eventData));
        base.OnEndDrag(eventData);
    }

    // Mouse key press
    /*public void OnPointerDown(PointerEventData eventData)
    {
        // Set line UI
        // UIManager.Instance.ShowUI<LineUI>("LineUI");
        // Set start position
        // UIManager.Instance.GetUI<LineUI>("LineUI").SetStartPos(transform.GetComponent<RectTransform>().anchoredPosition);
        // Mouse invisible
        //Cursor.visible = false;
        // Stop all coroutine
        StopAllCoroutines();
        // Start mouse coroutine
        StartCoroutine(OnMouseDownRight(eventData));

    }*/
    IEnumerator OnMouseDownRight(PointerEventData pData)
    {
        while (true)
        {
            // Mouse key right down to out loop
            if (Input.GetMouseButton(1)) { Debug.Log("right"); break; }
            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform.parent.GetComponent<RectTransform>(),
                pData.position,
                pData.pressEventCamera,
                out pos
            ))
            {
                // Set line position
                // UIManager.Instance.GetUI<LineUI>("LineUI").SetEndPos(pos);
                Debug.Log("on Coroutine");
                // Check enemy
                CheckRayToEnemy();
            }

            yield return null;
        }
        Cursor.visible = true;
        // UIManager.Instance.CloseUI("LineUI");
    }

    Enemy hitEnemy;// enemy casted
    private void CheckRayToEnemy()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10000, LayerMask.GetMask("Enemy")))
        {
            hitEnemy = hit.transform.GetComponent<Enemy>();
            hitEnemy.OnSelect();
            if (Input.GetMouseButtonDown(0))
            {
                // Stop all routine
                StopAllCoroutines();
                // Mouse visible
                //Cursor.visible = true;
                // Close line UI
                //UIManager.Instance.CloseUI("LineUI");
                if (TryUse() == true)
                {
                    // Hit enemy
                    int val = 1;
                    hitEnemy.Hit(val);
                }
                // Enemy on unselect
                hitEnemy.OnUnSelect();

                hitEnemy = null;
            }
        }
        else
        {
            //Œ¥…‰µΩπ÷ŒÔ
            if (hitEnemy != null)
            {
                hitEnemy.OnUnSelect();
                hitEnemy = null;
            }

        }

    }
}
