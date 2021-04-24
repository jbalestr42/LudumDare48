using UnityEngine;

public class Player : MonoBehaviour
{
    AControlable _controlledObject = null;
    [SerializeField] Camera _camera = null;

    enum PlayerState
    {
        ControllingPlayer,
        ControllingObject,
    }

    PlayerState _state = PlayerState.ControllingPlayer;

    void Start()
    {
        
    }

    void Update()
    {
        switch (_state)
        {
            case PlayerState.ControllingPlayer:
            {
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit; 
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        AControlable controlable = hit.collider.gameObject.GetComponent<AControlable>();
                        if (controlable != null)
                        {
                            Debug.Log("has controlable");
                            _controlledObject = controlable;
                            _state = PlayerState.ControllingObject;
                        }
                    }
                }
                else
                {
                    Vector3 movement = new Vector3();
                    if (Input.GetKey(KeyCode.Q))
                    {
                        movement.x = 10f;
                    }
                    if (Input.GetKey(KeyCode.D))
                    {
                        movement.x = -10f;
                    }
                    transform.Translate(movement * Time.deltaTime);
                }
                break;
            }
            // TODO add state to transition camera
            case PlayerState.ControllingObject:
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _controlledObject.DoAction();
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    ReleaseObject();
                    _state = PlayerState.ControllingPlayer;
                    // TODO move camera
                }
                else
                {
                    // TODO object movement
                    Vector3 movement = new Vector3();
                    if (Input.GetKey(KeyCode.Q))
                    {
                        movement.x = 10f;
                    }
                    if (Input.GetKey(KeyCode.D))
                    {
                        movement.x = -10f;
                    }
                    _controlledObject.transform.Translate(movement * Time.deltaTime);
                }
                break;
            }
            default:
            {
                break;
            }
        }
    }

    bool IsControllingObject()
    {
        return _controlledObject != null;
    }

    void ControlObject(AControlable obj)
    {
        _controlledObject = obj;
    }

    void ReleaseObject()
    {
        _controlledObject = null;
    }
}
