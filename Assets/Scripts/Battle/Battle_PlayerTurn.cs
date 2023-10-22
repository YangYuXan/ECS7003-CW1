using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_PlayerTurn : BattleUnit
{
    public override void Init()
    {
        base.Init();
        UIManager.Instance.ShowTip("Your turn", Color.green, delegate (){
            // Update power
            BattleManager.Instance.CurCostCount = 3;
            UIManager.Instance.GetUI<BattleUI>("BattleUI").UpdateCost();
            // No card
            if (BattleActionManager.Instance.HasCard() == false)
            {

                BattleActionManager.Instance.Init();
                
            }
            Debug.Log("draw");
            UIManager.Instance.GetUI<BattleUI>("BattleUI").CreateCardItem(3);     // Draw 3
            UIManager.Instance.GetUI<BattleUI>("BattleUI").UpdateCardItemPos();   // Update card position
            // Update card number
            UIManager.Instance.GetUI<BattleUI>("BattleUI").UpdateCardCount();
        });

        Debug.Log("player round");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
