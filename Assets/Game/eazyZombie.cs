using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eazyZombie : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Death()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetBool("isDeath", true);
    }
}
