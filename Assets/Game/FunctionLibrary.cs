using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FunctionLibrary : MonoBehaviour
{
    public void dayin()
    {
        print("test11111111");
    }

    public void AI_Move(NavMeshAgent agent,Vector3 Location)
    {
        
        agent.SetDestination(Location);
    }

    public void AI_Melee(NavMeshAgent agent,Vector3 SelfLocation,Character Enermy,float enermyAttackRadium)
    {
        InvokeRepeating("NewLamdaDelegate", 0.5f,0.5f);

        void NewLamdaDelegate() => CheckDistanceToAttack(agent,SelfLocation,Enermy,enermyAttackRadium);

    }

    public void CheckDistanceToAttack(NavMeshAgent agent, Vector3 SelfLocation, Character Enermy, float enermyAttackRadium)
    {
        if (Vector3.Distance(SelfLocation, Enermy.transform.position) > enermyAttackRadium)
        {
            agent.SetDestination(Enermy.transform.position);
        }
        else
        {
            agent.isStopped = true;
        }
    }
}
