using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class BattleInit : BattleUnit
{
    public override void Init()
    {
        base.Init();
        // Switch bgm
        // AudioManager.Instance.PlayBGM("battle", true);
        // Show battle UI
        //UIManager.Instance.ShowUI<BattleUI>("FightUI");
        // Initialize level 1 enemy
        //EnemyManeger.Instance.LoadRes("10003");
        // Initialze action
        BattleActionManager.Instance.Init();
        // Initialize status
        BattleManager.Instance.Init();
        // Switch to player turn
        BattleManager.Instance.ChangeType(BattleType.PlayerTurn);



    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
