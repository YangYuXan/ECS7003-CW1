using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterEnemy : MonoBehaviour
{
    public static EncounterEnemy Instance;
    public bool outBattle;   // out of battle

    private void Awake()
    {
        Instance = this;
        outBattle = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger");
        if(other.gameObject.CompareTag("Enemy") && outBattle)
        {
            Debug.Log("if trigger");
            BattleManager.Instance.EnterBattle(other.gameObject);
        }
    }

    /*public void CheckTrigger(Vector3 position, string targetTag, float encounterRadius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, encounterRadius);
        foreach (Collider col in hitColliders)
        {
            if (col.gameObject.CompareTag(targetTag))
            {
                Debug.Log("Battle in");
            }
        }
    }*/
}
