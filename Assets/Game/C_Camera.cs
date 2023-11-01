using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
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

    public Vector3 moveValue = new Vector3(0, 0, 0);
    public CameraMode cameraMode = CameraMode.freedom;
    float _duration = 10f;
    private float _duration_Zoom = 0.5f;
    float _timer = 0f;
    private Vector3 _moveDir;

    private float _y_distance;
    private float _z_distance;
    private float _rotateDegree;

    public TextMeshProUGUI TargetHPInformation;
    public Slider TargetHPSlider;
    public Image HPImage;
    public GameObject TargetHPUI;


    RaycastHit casHit;

    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        var ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out casHit);
        _y_distance = (transform.position.y - casHit.point.y) / 15;
        _z_distance = (transform.position.z - casHit.point.z) / 15;
        _rotateDegree = (transform.rotation.x - 5) / 3;
        print(_rotateDegree);
        

        //FindAnyObjectByType()

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        transform.position += _moveDir;

        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        Debug.DrawRay(transform.position, ray.direction * 200, Color.red);
        if (Physics.Raycast(ray, out casHit))
        {
            //移动到人物上时出现血条
            if (casHit.collider.gameObject.tag == "Character")
            {
                TargetHPUI.SetActive(true);
                Character showCharacter = casHit.collider.gameObject.GetComponent<Character>();
                TargetHPInformation.text =
                    showCharacter.currentHealth.ToString() + "/" + showCharacter.maxhealth.ToString();
                HPImage.fillAmount = showCharacter.currentHealth / showCharacter.maxhealth;
            }
            else
            {
                TargetHPUI.SetActive(false);
            }

        }

        switch (cameraMode)
            {
                case CameraMode.clockwiseRotate:
                    transform.Rotate(Vector3.up, 3, Space.World);
                    break;

                case CameraMode.anticlockwiseRotation:
                    transform.Rotate(Vector3.up, -3, Space.World);
                    break;

                case CameraMode.track:
                    if (_timer < _duration - 9.3)
                    {
                        float t = _timer / _duration;
                        transform.position = Vector3.Lerp(transform.position,
                            new Vector3(casHit.collider.gameObject.transform.position.x, transform.position.y,
                                casHit.collider.gameObject.transform.position.y), t);
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
                    break;
        }

    }

    void OnCameraMove(InputValue value)
    {
        moveValue.z = value.Get<Vector2>().y;
        moveValue.x = value.Get<Vector2>().x;
        _moveDir = new Vector3(transform.forward.x, 0, transform.forward.z) * moveValue.z +
                   transform.right * moveValue.x;
    }

    void OnInteract()
    {

        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out casHit))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (casHit.collider.gameObject.tag != "Character")
                { 
                    Transform spawnPoint = casHit.collider.gameObject.transform;
                    Instantiate(targetPointMark, casHit.point, spawnPoint.rotation);
                    if (GameObject.FindGameObjectWithTag("mark"))
                    {
                        Destroy(GameObject.FindGameObjectWithTag("mark"));
                    }
                    Instantiate(targetPointMark, casHit.point, spawnPoint.rotation);
                    Character component = playerPawn.gameObject.GetComponent<Character>();

                    //-1 为未进入战斗模式
                    if (component.attackOrder == -1)
                    {
                        component.AI_MovetoPoint(casHit.point);
                    }
                    //进入战斗
                    else
                    {
                        if (component.GetCanOperate())
                        {
                            component.AI_MovetoPoint(casHit.point);
                        }
                    }
                }
                else
                {
                    Transform spawnPoint = casHit.collider.gameObject.transform;
                    Instantiate(targetPointMark, casHit.point, spawnPoint.rotation);
                    Character component = playerPawn.gameObject.GetComponent<Character>();
                    //只有在选定了攻击模式后，才能打人
                    if (component.GetCharacterStatus()==Character.CharacterStatus.attack)
                    {
                        switch (component._damageType)
                        {
                            case Character.DamageType.normal:
                                component.AI_Attack(casHit.collider.gameObject);
                                break;

                            case Character.DamageType.fire:
                                //发射火焰飞弹，并设置灼烧
                                component.AI_UsingSkill(casHit.collider.gameObject);
                                //Button 消失
                                break;
                            case Character.DamageType.freeze:
                                //
                                break;

                        }
                        
                    }
                }

            }
        }
    }

    void OnCameraRotate(InputValue value)
    {
        if (value.Get<Vector2>().y != 0)
        {
            cameraMode = value.Get<Vector2>().y > 0
                ? cameraMode = CameraMode.clockwiseRotate
                : cameraMode = CameraMode.anticlockwiseRotation;

        }
        else
        {
            cameraMode = CameraMode.freedom;
        }
    }

    void OnZoom(InputValue value)
    {
        if (value.Get<Vector2>().y > 0)
        {
            cameraMode = CameraMode.zoom_out;
        }
        else if (value.Get<Vector2>().y < 0)
        {
            cameraMode = CameraMode.zoom;
        }
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
