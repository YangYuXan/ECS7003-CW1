using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 moveValue;
    public float speed;
    // public EncounterEnemy encounterEnemy;

    // Start is called before the first frame update
    void Start()
    {
        // encounterEnemy = FindFirstObjectByType<EncounterEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        // EncounterEnemy.Instance.CheckTrigger(transform.position, "Enemy", (float)20.0);
    }

    void OnMove(InputValue input)
    {
        moveValue = input.Get<Vector2>();
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveValue.x, 0, moveValue.y);
        GetComponent<Rigidbody>().AddForce(movement * speed * Time.fixedDeltaTime);
        // CheckTrigger(transform.position, "Enemy", (float)20.0);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("test");
            BattleManager.Instance.EndBattle();
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
