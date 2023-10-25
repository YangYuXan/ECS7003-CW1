using System.Collections;
using System.Collections.Generic;
using CharacterCampSpace;
using UnityEngine;
using UnityEngine.UI;

public class GameMode : MonoBehaviour
{
    public Button turnEndButton;

    private List<Character> battleQueue = new List<Character>();
    private Character determineRole;
    private int _currentAttackIndex=0;
    private bool _isPlayerTurn=true;
    private bool _isBattle;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateQueue(Vector3 position, CharacterCamp camp )
    {
        //将范围内所有符合要求的角色拉入战斗序列
        for(int i=0;i<FindObjectsOfType<Character>().Length;i++)
        {
            if (Vector3.Distance(position, 
                    FindObjectsOfType<Character>()[i].transform.position)<50&&
                        FindObjectsOfType<Character>()[i].camp== camp)
            {
                battleQueue.Add(FindObjectsOfType<Character>()[i]);
            }
        }

        //计算角色移动顺序
        CalcAttackOrder();
    }

    public void CalcAttackOrder()
    {
        //将可战斗的角色按照敏捷从高往低排序
        battleQueue.Sort((a,b)=>b.speed.CompareTo(a.speed));

        for (int i = 0; i < battleQueue.Count; i++)
        {
            //赋予角色攻击顺序
            battleQueue[i].attackOrder = i;

            //让一号位角色获取行动权
            if (i != 0)
            {
                battleQueue[i].SetCanOperate(false);
            }
        }
    }

    public void SetBattle(bool battle)
    {
        _isBattle = true;
    }

    public bool GetBattleStatus()
    {
        return _isBattle;
    }

    public void SetAttackIndex(int index)
    {
        _currentAttackIndex = index;
    }

    public int GetCurrentAttackIndex()
    {
        return _currentAttackIndex;
    }

    public void SwitchNextCharacter()
    {
        if (_isPlayerTurn)
        {
            //处理UI效果
            turnEndButton.enabled=false;
            ColorBlock disableColor = new ColorBlock();
            disableColor.normalColor = new Color(0.75f, 0.75f, 0.75f,1);
            turnEndButton.colors = disableColor;
        }

        _currentAttackIndex++;

        //获取队列中下一位可以战斗的角色
        for (int i = _currentAttackIndex; i < battleQueue.Count; i++)
        {
            //角色非倒地，非死亡则符合行动
            if (!battleQueue[i].GetDeath() && !battleQueue[i].GetNearDeath())
            {

                break;
            }
        }
    }
}
