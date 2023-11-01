using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
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
        idile,
        action
    }

    public enum CharacterStatus
    {
        normal,
        attack
    }

    public enum DamageType
    {
        normal,
        fire,
        freeze
    }

    private NavMeshAgent agent;
    public CharacterCamp camp;
    public string CharacterName;

    private MoveStatus moveStatus = MoveStatus.idile;
    private CharacterStatus _characterStatus = CharacterStatus.normal;
    public DamageType _damageType = DamageType.normal;

    //Character Property
    public float maxhealth;
    public float currentHealth;
    public float attackValue;
    public float attackRadius;
    public float attackCostPoint;
    public float speed;
    public float defenceValue;
    public int attackOrder=-1;
    public bool _canOperate = true;
    private bool _isNeardeath = false;
    private bool _isDeath = false;
    private bool _isFallDown = false;
    public bool _isPlayPawn;
    public float maxOperatePoint;
    public float currentOperatePoint;
    public TextMeshProUGUI operatePoint;

    //AI Status
    public bool actionFinish=true;

    //Abnormal Status
    public bool isBurned=false;
    public bool isFreeze = false;


    private GameObject target;
    private Vector3 targetPosition;
    public GameObject hateTarget;
    private PathFound pathFound;

    //
    [SerializedDictionary("Status", "result")]
    public SerializedDictionary<string, bool> status;

    // Start is called before the first frame update
    void Start()
    {
        currentOperatePoint = maxOperatePoint;
        pathFound = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PathFound>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!_isNeardeath && !_isDeath||_canOperate)
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
                        ApplyDamage(target,_damageType);
                        moveStatus = MoveStatus.idile;

                        //玩家不应该触发这个,玩家由Button触发SwitchNextCharacter()
                        if (!_isPlayPawn)
                        {
                            FindAnyObjectByType<GameMode>().SwitchNextCharacter();
                        }
                    }
                    break;
                case MoveStatus.action:
                    if (_damageType == DamageType.fire)
                    {
                        ApplyDamage(target, _damageType);
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

    public void AI_UsingSkill(GameObject enermy)
    {
        GetAgent();
        //TODO
        target = enermy;
        moveStatus = MoveStatus.action;
    }

    void GetAgent()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void SetHP(float dealValue)
    {
        currentHealth += dealValue;
    }

    void ApplyDamage(GameObject target, DamageType damageType)
    {
        if (currentOperatePoint>attackCostPoint)
        {
            currentOperatePoint -= attackCostPoint;
            switch (damageType)
            {
                case DamageType.normal:
                    break;

                case DamageType.fire:
                    target.GetComponent<Character>().isBurned = true;
                    break;

                case DamageType.freeze:
                    target.GetComponent<Character>().isFreeze = true;
                    break;
            }

            //处理受伤事件
            target.GetComponent<Character>().RecvDamage(attackValue, this.gameObject);
        }
    }

    void RecvDamage(float hurtValue,GameObject enermy)
    {
        //通知伙伴加入战斗,处于战斗状态了则不需要通知
        GameMode gameMode = FindFirstObjectByType<GameMode>();
        if (!gameMode.GetBattleStatus())
        {
            gameMode.GenerateQueue(transform.position,camp,gameObject);
            gameMode.GenerateHateTarget(this.camp,enermy);
            gameMode.CalcAttackOrder();

            gameMode.SetBattle(true);
            pathFound.moveMode = PathFound.MoveMode.TurnMode;

        } 

        //roll点判定是否会造成伤害
        if (defenceValue > hurtValue)
        {
            float randomValue = Random.Range(0f, 100f);
            if (randomValue <= 60)
            {
                SetHP(hurtValue*-1);
                //变色模拟受伤
                GetComponent<Renderer>().material.color=Color.red;

                if (currentHealth <= 0)
                {
                    _isNeardeath = true;
                }
            }
        }
        else
        {
            SetHP(hurtValue * -1);
            //变色模拟受伤
            GetComponent<Renderer>().material.color = Color.red;
            if (currentHealth <= 0)
            {
                _isNeardeath = true;
            }
        }


        

    }

    public void SetCharacterAttackStatus()
    {
        _characterStatus = CharacterStatus.attack;
    }

    public void SetCharacterNormalStatus()
    {
        _characterStatus = CharacterStatus.normal;
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
        if (currentHealth <= 0)
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

    //角色开始前计算附加伤害，例如冰冻、火焰
    public void CalcExtraHurt()
    {
        if (isBurned)
        {
            SetHP(-5);
        }

        if (isFreeze)
        {
            SetHP(-3);
        }
    }

    public void AiStrategy()
    {
        //如果不是玩家控制的，则进行策略
        if (!_isPlayPawn)
        {
            CalcExtraHurt();
            AI_Attack(hateTarget);
            print("计算策略"+attackOrder);
            //FindAnyObjectByType<GameMode>().SwitchNextCharacter();

        }
    }

    public void FireBomb()
    {
        SetCharacterAttackStatus();
        _damageType = DamageType.fire;
    }
}
