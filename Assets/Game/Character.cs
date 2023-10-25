using System.Collections;
using System.Collections.Generic;
using CharacterCampSpace;
using NUnit.Framework.Constraints;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace CharacterCampSpace
{
    public enum CharacterCamp
    {
        human,
        monster
    }
}

public class GOAP
{
    public string goapname;
}

public class Character : MonoBehaviour
{

    public enum Interaction
    {
        talk,
        Melee
    }

    public enum MoveStatus
    {
        moveAndAttack,
        move,
        idile
    }

    public enum CharacterStatus
    {
        normal,
        battle
    }

    private NavMeshAgent agent;
    public CharacterCamp camp;
    public string CharacterName;

    private MoveStatus moveStatus = MoveStatus.idile;
    private CharacterStatus _characterStatus = CharacterStatus.normal;

    //Character Property
    public float health;
    public float attackValue;
    public float attackRadius;
    public float attackCostPoint;
    public float speed;
    public float defenceValue;
    public int attackOrder=-1;
    private bool _canOperate = true;
    private bool _isNeardeath = false;
    private bool _isDeath = false;
    private bool _isFallDown = false;
    public bool _isPlayPawn;
    public float maxOperatePoint;
    public float currentOperatePoint;
    public TextMeshProUGUI operatePoint;

    private GameObject target;
    private Vector3 targetPosition;

    //

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        operatePoint.text = "OperatePoint:" + currentOperatePoint;

        if (!_isNeardeath && !_isDeath)
        {
            switch (moveStatus)
            {
                case MoveStatus.idile:
                    break;

                case MoveStatus.move:
                    GetAgent();
                    agent.SetDestination(targetPosition);
                    agent.isStopped = false;
                    break;

                case MoveStatus.moveAndAttack:
                    agent.isStopped = false;
                    agent.SetDestination(target.transform.position);
                    if (Vector3.Distance(transform.position, target.transform.position) < attackRadius)
                    {
                        agent.isStopped = true;
                        ApplyDamage(target);
                        moveStatus = MoveStatus.idile;
                    }
                    break;
            }
        }
        
    }

    public void AI_MovetoPoint(Vector3 position)
    {
        GetAgent();
        targetPosition = position;
        moveStatus = MoveStatus.move;
    }

    public void AI_Attack(GameObject enermy)
    {
        GetAgent();
        target = enermy;
        moveStatus = MoveStatus.moveAndAttack;
    }

    void GetAgent()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void SetHP(float dealValue)
    {
        health += dealValue;
    }

    void ApplyDamage(GameObject target)
    {
        print("attack");
        target.GetComponent<Character>().RecvDamage(attackValue);
    }

    void RecvDamage(float hurtValue)
    {
        //通知伙伴加入战斗
        GameMode gameMode = FindFirstObjectByType<GameMode>();
        if (!gameMode.GetBattleStatus())
        {
            gameMode.GenerateQueue(transform.position,camp);
            gameMode.SetBattle(true);
        } 
        //roll点判定是否会造成伤害
        if (defenceValue > hurtValue)
        {
            float randomValue = Random.Range(0f, 100f);
            if (randomValue <= 60)
            {
                SetHP(hurtValue*-1);
                if (health <= 0)
                {
                    _isNeardeath = true;
                }
            }
        }
        else
        {
            SetHP(hurtValue * -1);
            if (health <= 0)
            {
                _isNeardeath = true;
            }
        }


        

    }

    public void SetCharacterStatus(CharacterStatus status)
    {
        _characterStatus = status;
    }

    public CharacterStatus GetCharacterStatus()
    {
        return _characterStatus;
    }

    public void SetCanOperate(bool can)
    {
        _canOperate = can;
    }

    public bool GetCanOperate()
    {
        return _canOperate;
    }

    public void SetNeardeath()
    {
        if (health <= 0)
        {
            _isNeardeath = true;
        }
    }

    public void SetDeath()
    {
        if (_isNeardeath)
        {
            _isDeath = true;
        }
    }

    public bool GetDeath()
    {
        return _isDeath;
    }

    public bool GetNearDeath()
    {
        return _isNeardeath;
    }

    public void AiStrategy()
    {
        if (!_isPlayPawn)
        {
            
        }
    }
}
