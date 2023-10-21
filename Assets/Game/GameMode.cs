using System.Collections;
using System.Collections.Generic;
using CharacterCampSpace;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    private List<Character> battleQueue = new List<Character>();
    private Character determineRole;

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
        battleQueue.Sort((a,b)=>b.speed.CompareTo(a.speed));
        for (int i = 0; i < battleQueue.Count; i++)
        {
            //赋予角色攻击顺序
            battleQueue[i].attackOrder = i;
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
}
