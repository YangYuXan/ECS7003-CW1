using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [SerializeField] private GameObject player = null;
    float Xrotation = 0f;
    [SerializeField] private float Xsens = 200f, Ysens = 200f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float Xangle = Input.GetAxis("Mouse Y") * Ysens * Time.deltaTime;
        float Yangle = Input.GetAxis("Mouse X") * Xsens * Time.deltaTime;

        Xrotation -= Xangle;
        Xrotation = Mathf.Clamp(Xrotation, -80, 80);

        transform.localRotation = Quaternion.Euler(Xrotation, 0, 0);
        player.transform.Rotate(0, Yangle, 0);
    }
}
