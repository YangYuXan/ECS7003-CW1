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
        //����Χ�����з���Ҫ��Ľ�ɫ����ս������
        //TODO ���λ�ò���Ҫ���˾�ɫ��Ӫ������Ҫ���������ߵ���Ӫ
        for(int i=0;i<FindObjectsOfType<Character>().Length;i++)
        {
            if (Vector3.Distance(position, 
                    FindObjectsOfType<Character>()[i].transform.position)<50)
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
            ColorBlock disableColor = new();
            disableColor.normalColor = new Color(0.75f, 0.75f, 0.75f,1);
            turnEndButton.colors = disableColor;
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
