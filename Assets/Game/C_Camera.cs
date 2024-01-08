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

    public float MouseSpeed;

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
    public TextMeshProUGUI PlayerPawnHP;


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
        

        //FindAnyObjectByType()

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            transform.position+=new Vector3(0f,0.2f,0f);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            transform.position += new Vector3(0f, -0.2f, 0f);
        }
        PlayerPawnHP.text = playerPawn.GetComponent<Character>().currentHealth.ToString() + "/" +
                            playerPawn.GetComponent<Character>().maxhealth.ToString();

        transform.position += _moveDir;

        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        Debug.DrawRay(transform.position, ray.direction * 200, Color.red);
        if (Physics.Raycast(ray, out casHit))
        {
            //A health bar appears when moving onto a character
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
                    transform.Rotate(Vector3.up, 1.5f, Space.World);
                    break;

                case CameraMode.anticlockwiseRotation:
                    transform.Rotate(Vector3.up, -1.5f, Space.World);
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
        _moveDir = new Vector3(transform.forward.x* MouseSpeed, 0, transform.forward.z*MouseSpeed) * moveValue.z +
                   transform.right * moveValue.x * MouseSpeed;
    }

    void OnInteract()
    {

        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        Character component = playerPawn.gameObject.GetComponent<Character>();

        if (Physics.Raycast(ray, out casHit))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (casHit.collider.gameObject.tag != "Character")
                {
                    if (casHit.collider.gameObject.tag == "box")
                    {
                        if(Vector3.Distance(component.transform.position, casHit.collider.gameObject.transform.position) <= 0.5)
                        {
                            Box box = casHit.collider.gameObject.GetComponent<Box>();
                            component.ammo += box.ammo;
                            box.ammo = 0;
                        }
                    }else if (casHit.collider.gameObject.tag == "Weapon")
                    {
                     
                        component.hasWeapon = true;
                        GetComponent<Task1>().task2 = true;
                        Destroy(GetComponent<Task1>().task2mark.gameObject);

                    }
                    else
                    {
                        

                        //Calculate total navigation path length
                        float totalLength = 0f;
                        for (int i = 0; i < component.pathFound.lineRenderer.positionCount - 1; i++)
                        {
                            Vector3 pointA = component.pathFound.lineRenderer.GetPosition(i);
                            Vector3 pointB = component.pathFound.lineRenderer.GetPosition(i + 1);
                            totalLength += Vector3.Distance(pointA, pointB);
                        }

                        component.totalLength = totalLength;

                        //If attackOrder is -1, it means that the character is not in combat and will not participate in the round settlement.
                        if (component.attackOrder == -1)
                        {
                            component.AI_MovetoPoint(casHit.point);
                        }
                        //If it is not -1, it means that you are in combat. Determine whether you can move based on the value of GetCanOperate().
                        else
                        {
                            if (component.GetCanOperate())
                            {

                                component.AI_MovetoPoint(casHit.point);
                            }
                        }
                    }
                }
                else
                {
                    //You can only hit someone after selecting the attack mode
                    if (component.GetCharacterStatus() == Character.CharacterStatus.attack)
                    {
                        switch (component._damageType)
                        {
                            case Character.DamageType.normal:
                                component.AI_Attack(casHit.collider.gameObject);
                                if (casHit.collider.gameObject.GetComponent<Character>().needBan)
                                {
                                    component.AttackButton.interactable = false;
                                }
                               
                                break;

                            case Character.DamageType.fire:
                                component.AI_UsingSkill(casHit.collider.gameObject);
                                component.fireBomb.interactable = false;
                                component.cardLimited--;
                                break;
                            case Character.DamageType.freeze:
                                component.AI_UsingSkill(casHit.collider.gameObject);
                                component.freezeBomb.interactable = false;
                                component.cardLimited--;
                                break;

                            case Character.DamageType.addHP:
                                component.AddHp.interactable = false;
                                component.cardLimited--;
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
