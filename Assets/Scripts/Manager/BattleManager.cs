using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;


public enum BattleType
{
    None,
    Init,
    PlayerTurn,
    EnemyTurn,
    Win,
    Lose
}

public class BattleManager : MonoBehaviour
{
    public int MaxHp;   // Max Health
    public int CurHp;   // Current health
    public int MaxCostCount;   // Max power
    public int CurCostCount;   // Current power
    public BattleUnit battleUnit;
    public static BattleManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void EnterBattle(GameObject go)
    {
        Debug.Log("Enter battle");
        EncounterEnemy.Instance.outBattle = false;
        UIManager.Instance.ShowUI<BattleUI>("BattleUI");
        DeckManager.Instance.Init();
        EnemyManager.Instance.LoadEnemy(go);
        ChangeType(BattleType.Init);
    }

    public void EndBattle()
    {
        Debug.Log("Battle end");
        EncounterEnemy.Instance.outBattle = true;
        UIManager.Instance.CloseUI("BattleUI");
    }

    // Initialize player status
    public void Init()
    {
        MaxHp = 10;
        CurHp = 10;
        MaxCostCount = 3;
    }

    public void ChangeType(BattleType type)
    {
        switch (type)
        {
            case BattleType.None:
                break;
            case BattleType.Init:
                battleUnit = new BattleInit();
                break;
            case BattleType.PlayerTurn:
                battleUnit = new Battle_PlayerTurn();
                break;
            case BattleType.EnemyTurn:
                battleUnit = new Battle_EnemyTurn();
                break;
            case BattleType.Win:
                battleUnit = new Battle_Win();
                break;
            case BattleType.Lose:
                battleUnit = new Battle_Lose();
                break;
        }
        battleUnit.Init();// ≥ı ºªØ
    }

    public void GetPlayerHit(int hit)
    {

            CurHp -= hit;
        // Update UI
        UIManager.Instance.GetUI<BattleUI>("BattleUI").UpdateHp();

        if (CurHp <= 0)
        {
            CurHp = 0;
            //«–ªªµΩ”Œœ∑ ß∞‹◊¥Ã¨
            ChangeType(BattleType.Lose);
        }
    }
}
