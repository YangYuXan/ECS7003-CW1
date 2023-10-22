using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class BattleUI : UIBase
{
    private Image hpImg;
    private TextMeshProUGUI cardCountTxt;      // Cards number count
    private TextMeshProUGUI costTxt;
    private List<CardItem> cardItemList;    // Card list
    private void Awake()
    {
        hpImg = transform.Find("hp/fill").GetComponent<Image>();
        cardCountTxt = transform.Find("cardCount/count/countText").GetComponent<TextMeshProUGUI>();
        costTxt = transform.Find("cost/costText").GetComponent<TextMeshProUGUI>();

        // Turn end button
        transform.Find("endTurnBtn").GetComponent<Button>().onClick.AddListener(onChangeTurnBtn);

        cardItemList = new List<CardItem>();
    }

    private void Start()
    {
        UpdateHp();
    }
    public void UpdateHp()
    {
        hpImg.fillAmount = (float)BattleManager.Instance.CurHp / (float)BattleManager.Instance.MaxHp;
    }

    public void UpdateCost()
    {
        costTxt.text = BattleManager.Instance.CurCostCount + "/" + BattleManager.Instance.MaxCostCount;
    }

    public void UpdateCardCount()
    {

        cardCountTxt.text = BattleActionManager.Instance.cardList.Count.ToString();
    }

    public void UpdateCardItemPos()
    {

        float offset = 100f / cardItemList.Count;
        Vector2 startPos = new Vector2(-cardItemList.Count / 2f * offset + offset * 0.5f, -120);
        for (int i = 0; i < cardItemList.Count; i++)
        {
            cardItemList[i].GetComponent<RectTransform>().DOAnchorPos(startPos, 0.5f);
            startPos.x = startPos.x + offset;
        }
    }

    // Remove card
    public void RemoveCard(CardItem item)
    {
        // AudioManager.Instance.PlayEffect("Cards/cardShove");    // Remove audio
        item.enabled = false;

        BattleActionManager.Instance.usedCardList.Add(item.name);
        // Remove from list
        cardItemList.Remove(item);
        // Refresh card position
        UpdateCardItemPos();
        // Discard card
        item.GetComponent<RectTransform>().DOAnchorPos(new Vector2(1000, -700), 0.25f);
        item.transform.DOScale(0, 0.25f);
        Destroy(item.gameObject, 1);
    }

    // Create card item
    public void CreateCardItem(int count)
    {
        if (count > BattleActionManager.Instance.cardList.Count)
        {
            count = BattleActionManager.Instance.cardList.Count;
            Debug.Log("create count:" +  count);
        }

        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(Resources.Load("UI/CardItem"), transform) as GameObject;
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100, -70);
            // var item = obj.AddComponent<CardItem>();
            string cardType = BattleActionManager.Instance.DrawCard();
            Debug.Log(cardType);
            CardItem item = obj.AddComponent(System.Type.GetType(cardType)) as CardItem;

            cardItemList.Add(item);
        }
    }

    // Turn change
    private void onChangeTurnBtn()
    {
        
        if (BattleManager.Instance.battleUnit is Battle_PlayerTurn) BattleManager.Instance.ChangeType(BattleType.EnemyTurn);
    }

    public void RemoveOverHoldCards()
    {
        for (int i = cardItemList.Count - 2; i >= 0; i--)
        {
            RemoveCard(cardItemList[i]);
        }
    }

}
