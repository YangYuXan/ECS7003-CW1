using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CharacterCampSpace;
using UnityEngine;
using UnityEngine.UI;

public class GameMode : MonoBehaviour
{
    public Button turnEndButton;

    public List<Character> battleQueue = new();
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
        for(int i=0;i<FindObjectsOfType<Character>().Length;i++)
        {
            if (Vector3.Distance(position, 
                    FindObjectsOfType<Character>()[i].transform.position)<50)
            {
                battleQueue.Add(FindObjectsOfType<Character>()[i]);
            }
            if (FindObjectsOfType<Character>()[i]._isPlayPawn&& Vector3.Distance(position,
                    FindObjectsOfType<Character>()[i].transform.position) >= 50)
            {
                battleQueue.Add(FindObjectsOfType<Character>()[i]);
            }
        }
    }

    //����Ŀ�����
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
        //����ս���Ľ�ɫ�������ݴӸ���������
        battleQueue.Sort((a,b)=>b.speed.CompareTo(a.speed));

        //�ж���ʼ�غϵĽ�ɫ�Ƿ�Ϊ��ҵĻغ�
        if (battleQueue[0]._isPlayPawn)
        {
            _isPlayerTurn = true;
            turnEndButton.interactable = true;
        }

        for (int i = 0; i < battleQueue.Count; i++)
        {

            //�����ɫ����˳��
            battleQueue[i].attackOrder = i;

            //��һ��λ��ɫ��ȡ�ж�Ȩ
            if (i != 0)
            {
                battleQueue[i].SetCanOperate(false);
                
            }
            
        }
        //��һ�Ż�������ҿ���ʱ����һ�Ż���ʼ�ж�
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
        //����UIЧ��
        if (_isPlayerTurn)
        {
            turnEndButton.interactable = false;
            battleQueue[_currentAttackIndex].AttackButton.interactable = false;
        }

        //�趨Ϊ��ǰ�����ɫ�����ƶ�
        battleQueue[_currentAttackIndex]._canOperate = false;


        _currentAttackIndex++;
        if (_currentAttackIndex > battleQueue.Count-1)
        {
            _currentAttackIndex = 0;
        }

        //��ȡ��������һλ����ս���Ľ�ɫ
        for (int i = _currentAttackIndex; i < battleQueue.Count; i++)
        {
            //��ɫ�ǵ��أ�������������ж�
            if (!battleQueue[i].GetDeath() && !battleQueue[i].GetNearDeath())
            {
                if (battleQueue[i]._isPlayPawn)
                {
                    _isPlayerTurn = true;
                    turnEndButton.interactable=true;
                    battleQueue[i].AttackButton.interactable = true;
                }
                else
                {
                    _isPlayerTurn = false;


                }

                battleQueue[i].MaxMoveDistance = 20;
                battleQueue[i].RemainMoveDistance = 20;
                battleQueue[i]._canOperate = true;
                if (battleQueue[i].fireBomb != null && battleQueue[i].freezeBomb != null &&
                    battleQueue[i].AddHp != null)
                {
                    battleQueue[i].fireBomb.interactable = true;
                    battleQueue[i].freezeBomb.interactable = true;
                    battleQueue[i].AddHp.interactable = true;
                    battleQueue[i].cardLimited = 2;
                }
                
                battleQueue[i].AiStrategy();

                break;
            }
        }
    }

    public void LeaveBattle()
    {
        if (battleQueue.Count == 1)
        {
            if (battleQueue[0]._isPlayPawn)
            {
                _isBattle = false;
                battleQueue[0].attackOrder = -1;
                battleQueue[0]._canOperate = true;
                battleQueue[0].UI_RemainMoveDistance.text = "";
                battleQueue[0].UI_MaxMoveDistance.text = "";
                battleQueue[0].fireBomb.interactable = true;
                battleQueue[0].freezeBomb.interactable = true;
                battleQueue[0].AddHp.interactable = true;
                battleQueue[0].cardLimited = 2;
                turnEndButton.interactable = false;
                battleQueue[0].AttackButton.interactable = true;
                print("end battle");
                PathFound pathFound = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PathFound>();
                pathFound.moveMode = PathFound.MoveMode.Normal;
            }
            
        }
    }
}
