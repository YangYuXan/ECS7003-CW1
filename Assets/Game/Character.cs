using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{

    public enum CharacterCamp
    {
        human,
        monster
    }

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

    private NavMeshAgent agent;
    public CharacterCamp camp;
    public string CharacterName;

    private MoveStatus moveStatus = MoveStatus.idile;

    public float health;
    public float attackValue;
    public float attackRadius;
    public float speed;

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
                    ApplyDamage();
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

    void ApplyDamage()
    {
        print("attack");
    }

    void RecvDamage()
    {
        //通知伙伴加入战斗
        GameMode gameMode = FindFirstObjectByType<GameMode>();
        if (gameMode.GetBattleStatus())
        {
            
        }
        else
        {
            gameMode.SetBattle(true);
        }
    }
}
