using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewBehaviourScript : MonoBehaviour
{
    public float hp;
    public float defence;
    public float damage;
    public GameObject gameObject;
    public NavMeshAgent navmeshAgent;
    GameMode gameMode = FindFirstObjectByType<GameMode>();
    

    // Start is called before the first frame update
    void Start()
    {
        navmeshAgent = GetComponent<NavMeshAgent>();
        navmeshAgent.stoppingDistance = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyDamage()
    {
        navmeshAgent.SetDestination(gameObject.transform.position);
    }

    public void RecvDamage(float value)
    {
        if(value > hp)
        {
            Destroy(this.gameObject);
        }
        else
        {
            hp = hp - (value - defence);
            if (hp <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
