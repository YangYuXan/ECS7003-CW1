using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Task1 : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI TextMesh;

    public bool task1=false;
    public GameObject task1Obj;

    public bool task2 = false;
    public GameObject task2Obj;
    public GameObject task2mark;

    public bool task3 = false;
    public GameObject Task3enermyTarget;


    public bool task4 = false;
    public GameObject Task4enermyTargets;


    public bool task5 = false;
    public GameObject task5point;
    public GameObject task5mark;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(task1 == false)
        {
            TextMesh.text = "Get ammo";
        }
        if(task1 && !task2)
        {
            TextMesh.text = "Get Gun";
        }
        if (task2 && !task3)
        {
            TextMesh.text = "Kill ground Zombie";
        }
        if (task3 && !task4)
        {
            TextMesh.text = "Kill Zombie";
        }
        if (task4 && !task5)
        {
            TextMesh.text = "Go to The Car";
        }

        if (task1Obj != null)
        {
            if(Vector3.Distance(transform.position, task1Obj.transform.position) < 2)
            {
            GetComponent<Character>().ammo += 20;
            Destroy(task1Obj);
            task1 = true;
            }
        }
        

        if (Vector3.Distance(transform.position, task2Obj.transform.position) < 2)
        
        {
                //Destroy(task2mark);
                task2 = true;
        }

        if (Task3enermyTarget == null)
        {
            
            task3 = true;
        }
 

            if (Task4enermyTargets == null)
            {
                FindFirstObjectByType<GameMode>()._isBattle = false;
                task4 = true;
            }

        

        if (Vector3.Distance(transform.position, task5point.transform.position) < 2)

        {
             
                task5 = true;

        }
    }
}
