using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleActionManager
{
    public static BattleActionManager Instance = new BattleActionManager();
    public List<string> cardList;   // Card list
    public List<string> usedCardList;   // Discard card list
    private int total;
    // Initialize
    public void Init()
    {
        cardList = new List<string>();
        usedCardList = new List<string>();
        // Temp list ( used to keep player's cards )
        List<string> tempList = new List<string>();
        tempList.AddRange(DeckManager.Instance.cardList);
        total = tempList.Count;
        // Randomly add to card list
        while (tempList.Count > 0)
        {
            int tempIndex = Random.Range(0, tempList.Count);

            cardList.Add(tempList[tempIndex]);

            tempList.RemoveAt(tempIndex);
        }
    }

    // Has card
    public bool HasCard()
    {
        return usedCardList.Count != total;
    }
    // Draw card
    public string DrawCard()
    {
        string id = cardList[cardList.Count - 1];
        cardList.RemoveAt(cardList.Count - 1);
        return id;
    }
}
