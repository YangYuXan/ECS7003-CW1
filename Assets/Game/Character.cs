using System.Collections;
using System.Collections.Generic;
using CharacterCampSpace;
using UnityEditor.UI;
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

    public float health;
    public float attackValue;
    public float attackRadius;
    public float speed;
    public int attackOrder=-1;

    private GameObject target;
    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
                if (Vector3.Distance(transform.position,target.transform.position) < attackRadius)
                {
                    agent.isStopped = true;
                    ApplyDamage(target);
                    moveStatus = MoveStatus.idile;
                }
                break;
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
        target.GetComponent<Character>().RecvDamage();
    }

    void RecvDamage()
    {
        //通知伙伴加入战斗
        GameMode gameMode = FindFirstObjectByType<GameMode>();
        if (!gameMode.GetBattleStatus())
        {
            gameMode.GenerateQueue(transform.position,camp);
        }
        else
        {
            gameMode.SetBattle(true);
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
}
