using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager
{
    public static DeckManager Instance = new DeckManager();
    public List<string> cardList;//�洢ӵ�еĿ��Ƶ�id
    public void Init()
    {
        cardList = new List<string>();
        
        cardList.Add("AttackCard");
        cardList.Add("AttackCard");
        cardList.Add("AttackCard");
        cardList.Add("AttackCard");
        cardList.Add("AttackCard");

        // cardList.Add("DefendCard");

        //cardList.Add("1001");
        //cardList.Add("1001");
        //cardList.Add("1001");
        //cardList.Add("1001");

        //cardList.Add("1002");
        //cardList.Add("1002");
    }
}
