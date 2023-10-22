using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Lose : BattleUnit
{
    public override void Init()
    {
        base.Init();
        UIManager.Instance.ShowTip("You lose", Color.black, delegate () {
            Debug.Log("game over");
            BattleManager.Instance.StopAllCoroutines();
            DestroyEnemyUI();
            TimeDelay();
            BattleManager.Instance.EndBattle();
            // TODO: switch to checkpoint
            GameObject.Find("Player").transform.position = new Vector3(0, 0.5f, 0);

        });
    }

    private void DestroyEnemyUI()
    {
        GameObject[] enemyUI = GameObject.FindGameObjectsWithTag("EnemyUI");

        // 逐个销毁它们
        foreach (GameObject ui in enemyUI)
        {
            MonoBehaviour.Destroy(ui);
        }
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
