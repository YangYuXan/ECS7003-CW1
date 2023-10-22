using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Win : BattleUnit
{
    public override void Init()
    {
        base.Init();
        UIManager.Instance.ShowTip("You win", Color.black, delegate () {
            Debug.Log("game over");
            BattleManager.Instance.StopAllCoroutines();
            TimeDelay();
            BattleManager.Instance.EndBattle();

        });
    }

    private IEnumerator TimeDelay()
    {
        yield return new WaitForSecondsRealtime(3);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
