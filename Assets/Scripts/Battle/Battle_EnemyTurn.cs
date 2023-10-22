using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_EnemyTurn : BattleUnit
{
    public override void Init()
    {
        base.Init();
        // Remove all cards
        UIManager.Instance.GetUI<BattleUI>("BattleUI").RemoveOverHoldCards();
        //显示敌人回合提示
        UIManager.Instance.ShowTip("Enemy turn", Color.red, delegate ()
        {
            BattleManager.Instance.StartCoroutine(EnemyManager.Instance.DoAllEnemyAction());
        });
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
