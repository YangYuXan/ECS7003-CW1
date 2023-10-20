using UnityEngine;
using UnityEngine.InputSystem;
using Camera = UnityEngine.Camera;

public class C_Camera : MonoBehaviour
{
    public enum CameraMode
    {
        freedom,
        track,
        clockwiseRotate, 
        anticlockwiseRotation,
        freeRotate,
        zoom,
        zoom_out
    }

    public GameObject targetPointMark;
    public GameObject playerPawn;

    public Vector3 moveValue = new Vector3(0,0,0);
    public CameraMode cameraMode = CameraMode.freedom;
    float _duration = 10f;
    private float _duration_Zoom = 0.5f;
    float _timer = 0f;
    private Vector3 _moveDir;

    private float _y_distance;
    private float _z_distance;
    private float _rotateDegree;

    RaycastHit casHit;

    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        var ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out casHit);
        _y_distance = (transform.position.y - casHit.point.y)/15;
        _z_distance = (transform.position.z - casHit.point.z)/15;
        _rotateDegree = (transform.rotation.x-5) / 3;
        print(_rotateDegree);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        transform.position += _moveDir;

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

            case CameraMode.zoom:
                Vector3 position_buff = new Vector3(0, _y_distance, _z_distance);
                if (_timer < _duration_Zoom)
                { 
                    float t = _timer / _duration_Zoom;
                    transform.position = Vector3.Lerp(transform.position, transform.position - position_buff, t);
                    _timer += 0.1f;
                }
                break;

            case CameraMode.zoom_out:
                break;

            case CameraMode.freeRotate:
                transform.Rotate(0, Mouse.current.delta.ReadValue().x, 0, Space.World);
                break;

            case CameraMode.freedom:
                /*
                var ray = new Ray(transform.position, transform.forward);
                Physics.Raycast(ray, out casHit);
                Debug.DrawRay(transform.position, ray.direction * 200, Color.red);
                */
                break;
        }

    }

    void OnCameraMove(InputValue value)
    {
        moveValue.z = value.Get<Vector2>().y;
        moveValue.x = value.Get<Vector2>().x;
        _moveDir = new Vector3(transform.forward.x, 0, transform.forward.z) * moveValue.z + transform.right * moveValue.x;
    }

    void OnInteract()
    {

        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        

        Debug.DrawRay(transform.position, ray.direction*200, Color.red);

        if (Physics.Raycast(ray, out casHit))
        {
            if (casHit.collider.gameObject.tag != "Character")
            {
                Transform spawnPoint = casHit.collider.gameObject.transform;
                Instantiate(targetPointMark, casHit.point, spawnPoint.rotation);
                Character component =playerPawn.gameObject.GetComponent<Character>();
                component.AI_MovetoPoint(casHit.point);
                
            }
            else
            {
                Transform spawnPoint = casHit.collider.gameObject.transform;
                Instantiate(targetPointMark, casHit.point, spawnPoint.rotation);
                Character component = playerPawn.gameObject.GetComponent<Character>();
                component.AI_Attack(casHit.collider.gameObject);
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
        if (value.Get<Vector2>().y > 0)
        {
            cameraMode = CameraMode.zoom_out;
        }
        else if(value.Get<Vector2>().y < 0)
        {
            cameraMode = CameraMode.zoom;
        }
        //if (value.Get<Vector2>().y > 0)
        //{
        //    Vector3 position_buff = new Vector3(0, _y_distance, _z_distance);
        //    transform.position = transform.position-position_buff;
        //    transform.Rotate(new Vector3(_rotateDegree, 0,0), Space.World);
        //}
        //else if (value.Get<Vector2>().y < 0)
        //{
        //    float rotation_x_buff = transform.rotation.x + _rotateDegree;
        //    Vector3 position_buff = new Vector3(0, _y_distance, _z_distance);
        //    transform.position += position_buff;
        //}

    }

    void OnRotate(InputValue value)
    {
        if (value.isPressed)
        {
            cameraMode = CameraMode.freeRotate;
        }
        else
        {
            cameraMode = CameraMode.freedom;
        }
    }

}
