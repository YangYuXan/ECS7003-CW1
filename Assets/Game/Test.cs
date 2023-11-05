using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Button btn;
    private float time = 0;
    private int i_time = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        print(i_time);
        time += Time.deltaTime;
        i_time = (int)(time);
        if (i_time % 2 ==0)
        {
            btn.interactable = true;
        }
        else
        {
            btn.interactable = false;
        }
    }

}
