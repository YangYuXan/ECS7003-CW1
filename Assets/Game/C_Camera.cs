using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using Camera = UnityEngine.Camera;

public class C_Camera : MonoBehaviour
{
    public enum CameraMode
    {
        freedom,
        track,
        clockwiseRotate, 
        anticlockwiseRotation
    }

    public GameObject targetPointMark;

    public Vector3 moveValue = new Vector3(0,0,0);
    public float _moveSpeed;
    public CameraMode cameraMode = CameraMode.freedom;
    private bool _moveCamera=false;

    float _duration = 10f;
    float _timer = 0f;
    private Vector3 moveDir;


    Vector3 hitpoint = Vector3.zero;
    RaycastHit casHit;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        transform.position += moveDir;

        switch (cameraMode)
        {
            case CameraMode.clockwiseRotate:
                if (casHit.collider.gameObject!=null)
                {
                    transform.RotateAround(casHit.point, casHit.collider.gameObject.transform.up, 3);
                }
                break;

            case CameraMode.anticlockwiseRotation:
                if (casHit.collider.gameObject != null)
                {
                    transform.RotateAround(casHit.point, casHit.collider.gameObject.transform.up, -3);
                }
                break;

            case CameraMode.track:
                if (_timer < _duration - 9.3)
                { 
                    float t = _timer / _duration;
                    transform.position = Vector3.Lerp(transform.position,
                        new Vector3(casHit.collider.gameObject.transform.position.x, transform.position.y, casHit.collider.gameObject.transform.position.y), t);
                    _timer += Time.deltaTime;
                }
                else if (Vector3.Distance(transform.position, casHit.collider.gameObject.transform.position) < 20)
                {
                    cameraMode = CameraMode.freedom;
                    _timer = 0;
                }
                break;
            case CameraMode.freedom:
                break;
        }

    }

    void OnCameraMove(InputValue value)
    {
        moveValue.z = value.Get<Vector2>().y;
        moveValue.x = value.Get<Vector2>().x;
        moveDir = new Vector3(transform.forward.x, 0, transform.forward.z) * moveValue.z + transform.right * moveValue.x;
    }

    void OnInteract()
    {
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        

        Debug.DrawRay(transform.position, ray.direction*200, Color.red);

        if (Physics.Raycast(ray, out casHit))
        {
            if (casHit.collider.gameObject.tag == "Character")
            {
                //SetMoveCamera(true);
                cameraMode = CameraMode.track;
            }

            if (casHit.collider.gameObject.tag == "Terrain")
            {
                Transform spawnPoint = casHit.collider.gameObject.transform;
                Instantiate(targetPointMark, casHit.point, spawnPoint.rotation);
            }
            
        }
    }

    void OnCameraRotate(InputValue value)
    {
        if (value.Get<Vector2>().y!=0)
        {
            print("rotate");
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (value.Get<Vector2>().y > 0)
            {
                Physics.Raycast(ray, out casHit);
                cameraMode = CameraMode.clockwiseRotate;
            }
            else
            {
                Physics.Raycast(ray, out casHit);
                cameraMode = CameraMode.anticlockwiseRotation;
            }
            
        }
        else
        {
            print("stop");
            cameraMode = CameraMode.freedom;
        }
    }

    void OnZoom(InputValue value)
    {
        //float height = transform.position.y-;


        print("value: "+value.Get<Vector2>());
    }

    bool GetMoveCameraValue()
    {
        return _moveCamera;
    }

}
