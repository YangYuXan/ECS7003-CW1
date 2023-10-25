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
        //����Χ�����з���Ҫ��Ľ�ɫ����ս������
        for(int i=0;i<FindObjectsOfType<Character>().Length;i++)
        {
            if (Vector3.Distance(position, 
                    FindObjectsOfType<Character>()[i].transform.position)<50&&
                        FindObjectsOfType<Character>()[i].camp== camp)
            {
                battleQueue.Add(FindObjectsOfType<Character>()[i]);
            }
        }

        //�����ɫ�ƶ�˳��
        CalcAttackOrder();
    }

    public void CalcAttackOrder()
    {
        //����ս���Ľ�ɫ�������ݴӸ���������
        battleQueue.Sort((a,b)=>b.speed.CompareTo(a.speed));

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
            //����UIЧ��
            turnEndButton.enabled=false;
            ColorBlock disableColor = new ColorBlock();
            disableColor.normalColor = new Color(0.75f, 0.75f, 0.75f,1);
            turnEndButton.colors = disableColor;
        }

        _currentAttackIndex++;

        //��ȡ��������һλ����ս���Ľ�ɫ
        for (int i = _currentAttackIndex; i < battleQueue.Count; i++)
        {
            //��ɫ�ǵ��أ�������������ж�
            if (!battleQueue[i].GetDeath() && !battleQueue[i].GetNearDeath())
            {

                break;
            }
        }
    }
}
