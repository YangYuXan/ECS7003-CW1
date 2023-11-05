using System.Collections;
using System.Collections.Generic;
using CharacterCampSpace;
using UnityEngine;
using UnityEngine.UI;

public class GameMode : MonoBehaviour
{
    public Button turnEndButton;

    private List<Character> battleQueue = new();
    //private Character[] canOperateRoles;
    private int _currentAttackIndex=0;
    public bool _isPlayerTurn=false;
    public bool _isBattle;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateQueue(Vector3 position, CharacterCamp camp,GameObject inital )
    {
        //将范围内所有符合要求的角色拉入战斗序列
        //TODO 这个位置不仅要受伤绝色阵营，还需要攻击发起者的阵营
        for(int i=0;i<FindObjectsOfType<Character>().Length;i++)
        {
            if (Vector3.Distance(position, 
                    FindObjectsOfType<Character>()[i].transform.position)<50)
            {
                battleQueue.Add(FindObjectsOfType<Character>()[i]);
            }
        }
    }

    //生成目标队列
    public void GenerateHateTarget(CharacterCamp selfCamp,GameObject Enermy)
    {
        for (int i = 0; i < battleQueue.Count; i++)
        {
            if (battleQueue[i].camp == selfCamp)
            {
                battleQueue[i].hateTarget = Enermy;
            }
        }
    }


    public void CalcAttackOrder()
    {
        //将可战斗的角色按照敏捷从高往低排序
        battleQueue.Sort((a,b)=>b.speed.CompareTo(a.speed));

        //判断起始回合的角色是否为玩家的回合
        if (battleQueue[0]._isPlayPawn)
        {
            _isPlayerTurn = true;
        }

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
        //当一号机不是玩家控制时，让一号机开始行动
        if (!battleQueue[0]._isPlayPawn)
        {
            battleQueue[0].AiStrategy();
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
        //处理UI效果
        if (_isPlayerTurn)
        {
            turnEndButton.interactable = false;
            ColorBlock disableColor = new();
            disableColor.normalColor = new Color(0.75f, 0.75f, 0.75f,1);
            turnEndButton.colors = disableColor;
        }

        //设定为当前结算角色不可移动
        battleQueue[_currentAttackIndex]._canOperate = false;


        _currentAttackIndex++;
        if (_currentAttackIndex > battleQueue.Count-1)
        {
            _currentAttackIndex = 0;
        }

        //获取队列中下一位可以战斗的角色
        for (int i = _currentAttackIndex; i < battleQueue.Count; i++)
        {
            //角色非倒地，非死亡则符合行动
            if (!battleQueue[i].GetDeath() && !battleQueue[i].GetNearDeath())
            {
                if (battleQueue[i]._isPlayPawn)
                {
                    _isPlayerTurn = true;
                    turnEndButton.interactable=true;
                }
                else
                {
                    _isPlayerTurn = false;


                }
                battleQueue[i]._canOperate = true;
                battleQueue[i].AiStrategy();

                break;
            }
        }
    }
}
