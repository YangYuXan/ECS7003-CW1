using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zom2 : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;
    public bool dead;
    public int order;
    public GameObject target;
    public GameObject friend;
    public bool isAttack;
    public bool isLast;
    public bool inBattle;

    public float HP;

    // Start is called before the first frame update
    void Start()
    {
        agent.stoppingDistance = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        print(agent.velocity);
        if (isAttack)
        {
            
            if (agent.velocity.magnitude == 0&&Vector3.Distance(gameObject.transform.position,target.transform.position)<2)
            {
                animator.SetBool("isMove", false);
                animator.SetBool("attack", true);
                ApplyDamage();
                isAttack = false;

                target.GetComponent<NewCharacter>().attackButton.interactable = true;
                target.GetComponent<NewCharacter>().attackDouble.interactable = true;
                target.GetComponent<NewCharacter>().canAttack = true;
            }
            else
            {
                animator.SetBool("isMove", true);
            }
        }
        else
        {
            animator.SetBool("isMove", false);
        }
    }

    public void playDeath()
    {
        animator.SetBool("isDeath", true);
        target.GetComponent<Task1>().task4 = true;
        target.GetComponent<NewCharacter>().canAttack = true;
        Destroy(gameObject);

    }

    public void RecvHurt(bool doublehuet)
    {
        if (doublehuet)
        {
            HP -= 20;
        }
        else
        {
            HP -= 10;
        }
        if (HP <= 0)
        {
            playDeath();
        }
        else
        {
            Attack();
        }

    }

    public void Attack()
    {
        isAttack = true;
        agent.SetDestination(target.transform.position);
    }
    public void ApplyDamage()
    {
        target.gameObject.GetComponent<NewCharacter>().HP -= 10;
    }

    public void DelayedFunction()
    {

    }
    
    public void BackStatus()
    {
        animator.SetBool("attack", false);
    }
}
