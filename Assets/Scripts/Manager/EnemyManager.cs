using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class EnemyManager
{
    public static EnemyManager Instance = new EnemyManager();

    private List<Enemy> enemyList;  // Enemy list

    public void LoadEnemy(GameObject go)
    {
        enemyList = new List<Enemy>();
        Enemy enemy = go.AddComponent<Enemy>();
        Debug.Log(enemy.GetType().Name);
        enemyList.Add(enemy);   // Add to list
    }

    public void DeleteEnemy(Enemy enemy)
    {
        enemyList.Remove(enemy);

        if(enemyList.Count == 0)
        {
            BattleManager.Instance.ChangeType(BattleType.Win);
        }
    }

    public IEnumerator DoAllEnemyAction()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            yield return BattleManager.Instance.StartCoroutine(enemyList[i].DoAction());
        }
        // 切换到玩家回合
        BattleManager.Instance.ChangeType(BattleType.PlayerTurn);
    }

}
