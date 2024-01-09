using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zom1 : MonoBehaviour
{
    public Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayDeath()
    {
        print("Death");
        animator.SetBool("isDeath", true);
        Destroy(gameObject);
    }
}
