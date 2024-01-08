using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStatementManager : MonoBehaviour
{
    public float moveSpeed = 3;
    [HideInInspector] public Vector3 dir;
    float hzInput, vInput;
    CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        GetDirectionAndMove();
    }

    void GetDirectionAndMove()
    {
        hzInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");

        dir = transform.forward * vInput + transform.right * hzInput;

        characterController.Move(dir * moveSpeed * Time.deltaTime);
    }
}
