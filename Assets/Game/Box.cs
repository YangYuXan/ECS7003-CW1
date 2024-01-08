using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public int ammo;
    public bool isUse;
    public GameObject Mark;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isUse)
        {
            if (Mark!=null)
            {
                Destroy(Mark);
            }
           
        }
    }
}
