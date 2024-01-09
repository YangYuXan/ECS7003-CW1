using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NewCharacter : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;

    public float HP;
    public float Damager;
    public string behaviour;
    public bool isInbattle;
    public bool canAttack;
    public Button attackButton;
    public Button attackDouble;
    public int ammo;
    public bool noMonster;
    public GameObject weapon;

    public AudioClip soundClip;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        weapon.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundClip;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.velocity.magnitude != 0)
        {
            animator.SetBool("isMove", true);
        }
        else
        {
            animator.SetBool("isMove", false);
        }

    }

    public void AI_MovetoPoint(Vector3 point)
    {
        if (isInbattle && !canAttack)
        {
            return;
        }
        agent.SetDestination(point);
    }

    public void ApplyDamage(GameObject enermy,bool doubleHurt)
    {

        if (enermy.gameObject.GetComponent<Zom2>())
        {
            
            isInbattle = true;
            audioSource.Play();

            if (doubleHurt)
            {
                enermy.gameObject.GetComponent<Zom2>().RecvHurt(true);
                int randomInt = Random.Range(0, 11);
                if (randomInt <= 5)
                {
                    HP -= 10;
                }
            }
            else
            {
                enermy.gameObject.GetComponent<Zom2>().RecvHurt(false);
            }

            enermy.gameObject.GetComponent<Zom2>().isAttack = true;
            canAttack = false;
            attackButton.interactable = false;
            attackDouble.interactable = false;
        }

    }

}
