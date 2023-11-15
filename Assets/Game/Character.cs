using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using CharacterCampSpace;
using NUnit.Framework.Constraints;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

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
        freeze,
        addHP
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
    public float speed;
    public float defenceValue;
    public int attackOrder=-1;
    public bool _canOperate = true;
    private bool _isNeardeath = false;
    private bool _isDeath = false;
    public bool _isPlayPawn;
    public float MaxMoveDistance;
    public float RemainMoveDistance;
    public int cardLimited = 2;

    //UI element
    public TextMeshProUGUI UI_MaxMoveDistance;
    public TextMeshProUGUI UI_RemainMoveDistance;
    public TextMeshProUGUI UI_CardLimited;
    public Button fireBomb;
    public Button freezeBomb;
    public Button AddHp;
    public Button AttackButton;


    //Abnormal Status
    public bool isBurned=false;
    public bool isFreeze = false;

    //For Game Develop
    public float totalLength = 0f;

    private GameObject target;
    private Vector3 targetPosition;
    public GameObject hateTarget;
    public PathFound pathFound;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        pathFound = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PathFound>();
        FindFirstObjectByType<GameMode>().turnEndButton.interactable = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (UI_CardLimited != null)
        {
            UI_CardLimited.text = "Card Limited: "+cardLimited.ToString();
        }

        if (_isPlayPawn)
        {
            if (cardLimited == 0)
            {
                fireBomb.interactable = false;
                freezeBomb.interactable = false;
                AddHp.interactable = false;
            }
        }

        if (UI_MaxMoveDistance != null && UI_RemainMoveDistance != null)
        {
            if (FindFirstObjectByType<GameMode>()._isBattle)
            {
                UI_MaxMoveDistance.text = "MaxMaxMoveDistance: " + MaxMoveDistance+"m";
                UI_RemainMoveDistance.text = "UI_RemainMoveDistance: " + RemainMoveDistance+"m";
            }
            else
            {
                UI_MaxMoveDistance.text = "";
                UI_RemainMoveDistance.text = "";
            }
        }

        if (!_isNeardeath && !_isDeath && _canOperate)
        {
            switch (moveStatus)
            {
                case MoveStatus.idile:
                    break;

                case MoveStatus.move:
                    GetAgent();
                    agent.isStopped = false;
                    if (FindFirstObjectByType<GameMode>()._isBattle)
                    {
                        RemainMoveDistance = MaxMoveDistance - (totalLength - agent.remainingDistance);
                    }
                    else
                    {
                        agent.stoppingDistance = 0;
                    }

                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        MaxMoveDistance = RemainMoveDistance;
                        moveStatus = MoveStatus.idile;

                        //After the npc completes the behavior, execute SwitchNextCharacter()
                        if (!_isPlayPawn)
                        {
                            agent.isStopped = true;
                            FindAnyObjectByType<GameMode>().SwitchNextCharacter();
                        }
                    }
                    agent.isStopped = false;
                    break;

                case MoveStatus.moveAndAttack:
                    GetAgent();
                    agent.isStopped = false;
                    if (FindFirstObjectByType<GameMode>()._isBattle)
                    {
                        RemainMoveDistance = MaxMoveDistance - (totalLength - agent.remainingDistance);
                        agent.stoppingDistance = attackRadius;
                    }
                    else
                    {
                        agent.stoppingDistance = 0;
                    }

                    if (Vector3.Distance(transform.position,target.transform.position) <= attackRadius)
                    {
                        MaxMoveDistance = RemainMoveDistance;
                        ApplyDamage(target, _damageType);
                        moveStatus = MoveStatus.idile;
                        agent.isStopped = true;
                        _characterStatus=CharacterStatus.normal;
                        //After the npc completes the behavior, execute SwitchNextCharacter()
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
                    if(_damageType == DamageType.freeze)
                    {
                        ApplyDamage(target, _damageType);
                    }
                    moveStatus = MoveStatus.idile;
                    break;
            }
        }
        
    }
    /*
     *  The character will move closer to the target point. In combat mode,
     *  the distance moved each round is fixed. When the remaining moving distance
     *  is greater than the total distance, the character will move to the target point.
     *  If it is not enough, it will stop halfway.
     *
     *  @param : Target location
     */
    public void AI_MovetoPoint(Vector3 position)
    {
        GetAgent();
        targetPosition = position;

        if (FindFirstObjectByType<GameMode>()._isBattle)
        {
            if (totalLength <= RemainMoveDistance)
            {
                agent.stoppingDistance = 0;
            }
            //There is not enough distance left to complete the journey
            else
            {
                agent.stoppingDistance = totalLength - RemainMoveDistance;
            }
        }
        else
        {
            agent.stoppingDistance = 0;
        }

        agent.SetDestination(targetPosition);
        moveStatus = MoveStatus.move;
    }

    /*
     * The character will move closer to the target point and then launch an attack.
     * In combat mode, the distance moved each round is fixed. When the remaining movement distance
     * is greater than the total distance, the character will move to the target point and then attack.
     * If it is not enough, it will stop halfway.
     *
     *  @param : Attack Target
     */

    public void AI_Attack(GameObject enermy)
    {
        GetAgent();
        target = enermy;

        if (FindFirstObjectByType<GameMode>()._isBattle)
        {
            if (totalLength <= RemainMoveDistance)
            {
                agent.stoppingDistance = attackRadius;
            }
            //There is not enough distance left to complete the journey
            else
            {
                agent.stoppingDistance = totalLength - RemainMoveDistance;
            }
        }
        else
        {
            agent.stoppingDistance = 0;
        }

        agent.SetDestination(target.gameObject.transform.position);
        moveStatus = MoveStatus.moveAndAttack;
    }
    /*
     * This function is called when the character attempts to use a card with damage
     *
     * @param : Attack Target
     */
    public void AI_UsingSkill(GameObject enermy)
    {
        GetAgent();
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
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Destroy(this.gameObject);
            FindFirstObjectByType<GameMode>().battleQueue.Remove(this);
            FindFirstObjectByType<GameMode>().LeaveBattle();
        }
    }

    /*
     *  This function is called when the character tries to cause damage to the enemy.
     *  Different DamageType will give different debuffs to the enemy.
     *  If you need to extend the damage type, add an enum in enum DamageType
     *
     *@param target: Attack Target
     *@param damageType : damage Type inlude normal attack，fire, freeze.
     */
    void ApplyDamage(GameObject target, DamageType damageType)
    {
        
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

            target.GetComponent<Character>().RecvDamage(attackValue, this.gameObject);

    }

    /*
     *  This function will be called when the character is injured and attacked,
     *  but the character may not be injured. When the attacker's attack power is higher than the target character's defense,
     *  he will definitely be injured. When the attack power is less than the defense,
     *  the injury is a probability. event. In addition, if the person being attacked is not in combat,
     *  he, his companions, and the initiator of the attack will all be dragged into the battle,
     *  and the order of attacks will be determined based on everyone's speed.
     *
     *  @param hurtValue : hurt Value
     *  @param enermy : Attacker
     */
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

        //Roll points determine whether damage will be caused
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
            //Discoloration simulates injury
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
        _damageType = DamageType.normal;
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

    //Calculate additional damage, such as freezing, fire, before the character starts taking action
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

    /*
     *  The NPC generates a strategy when acting.
     * If it can come within the attack range,
     * it will attack, otherwise it will move towards the target.
     */

    public void AiStrategy()
    {
        //Planning if not player controlled
        if (!_isPlayPawn)
        {
            CalcExtraHurt();

            //Calculate whether the attack distance is enough to meet the remaining movement distance of this round
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(hateTarget.transform.position, path);
            totalLength = 0f;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                Vector3 pointA = path.corners[i];
                Vector3 pointB = path.corners[i + 1];
                totalLength += Vector3.Distance(pointA, pointB);
            }

            if (totalLength <= RemainMoveDistance)
            {
                AI_Attack(hateTarget);
            }
            else
            {
                AI_MovetoPoint(hateTarget.transform.position);
            }
        }

    }

    public void FireBomb()
    {
        SetCharacterAttackStatus();
        _damageType = DamageType.fire;
    }

    public void FreezeBomb()
    {
        SetCharacterAttackStatus();
        _damageType = DamageType.freeze;
    }

    public void AddHP()
    {
        print("add");
        if (currentHealth + 20 >= maxhealth)
        {
            currentHealth = 100;
            GetComponent<Renderer>().material.color = Color.white;
        }
        if (currentHealth + 20 < maxhealth)
        {
            currentHealth += 20;
        }
        cardLimited--;
        AddHp.interactable = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            Item buffItem = collision.gameObject.GetComponent<Item>();

            if (buffItem.bufftype == Item.BuffType.AddSpeed)
            {
                speed += 5;
            }

            if (buffItem.bufftype == Item.BuffType.AddAttackValue)
            {
                attackValue += 5;
            }

            if (buffItem.bufftype == Item.BuffType.AddHP)
            {
                maxhealth += 30;
                currentHealth += 30;
            }

            Destroy(collision.gameObject);
        }
    }


}
