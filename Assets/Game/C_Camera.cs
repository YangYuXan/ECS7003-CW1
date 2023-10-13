using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Camera = UnityEngine.Camera;

public class C_Camera : MonoBehaviour
{
    public enum CameraMode
    {
        freedom,
        track
    }

    public Vector3 moveValue;
    public float _moveSpeed;
    public int currentIndex;
    public CameraMode cameraMode = CameraMode.freedom;
    private Camera camera;
    Vector3 hitpoint = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += moveValue;
    }

    void OnCameraMove(InputValue value)
    {
        moveValue = new Vector3(value.Get<Vector2>().x, 0, value.Get<Vector2>().y)*_moveSpeed;

    }

    void OnInteract()
    {
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit casHit;

        Debug.DrawRay(transform.position, ray.direction*200, Color.red);

        if (Physics.Raycast(ray, out casHit))
        {
            if (casHit.collider.gameObject.tag == "Character")
            {
                print("jit");
            }
            
        }
    }

    /*
    float GetMovespeed()
    {
        return _moveSpeed;
    }
    */
}
