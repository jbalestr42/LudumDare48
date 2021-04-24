using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    AControlable _controlledObject = null;
    [SerializeField] Camera _camera = null;

    enum PlayerState
    {
        ControllingPlayer,
        TransitionCameraToObject,
        ControllingObject,
        TransitionCameraToPlayer,
    }

    PlayerState _state = PlayerState.ControllingPlayer;

    void Start()
    {
        _camera.transform.LookAt(transform.position);
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
                            _state = PlayerState.TransitionCameraToObject;
                            StartCoroutine(TransitionCameraToObject());
                        }
                    }
                }
                else
                {
                    Vector3 movement = new Vector3();
                    if (Input.GetKey(KeyCode.Q))
                    {
                        movement.x = -10f;
                    }
                    if (Input.GetKey(KeyCode.D))
                    {
                        movement.x = 10f;
                    }
                    if (Input.GetKey(KeyCode.Z))
                    {
                        movement.z = 10f;
                    }
                    else if (Input.GetKey(KeyCode.S))
                    {
                        movement.z = -10f;
                    }
                    _camera.transform.Translate(movement * Time.deltaTime);
                }
                break;
            }
            // TODO add state to transition camera
            case PlayerState.ControllingObject:
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _controlledObject.TryDoAction();
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
                        movement.x = -10f;
                    }
                    else if (Input.GetKey(KeyCode.D))
                    {
                        movement.x = 10f;
                    }
                    if (Input.GetKey(KeyCode.Z))
                    {
                        movement.z = 10f;
                    }
                    else if (Input.GetKey(KeyCode.S))
                    {
                        movement.z = -10f;
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

    IEnumerator TransitionCameraToObject() 
    {
        float duration = 1f;
        float timer = 0f;

        _camera.transform.SetParent(_controlledObject.transform);
        Vector3 start = _camera.transform.position;
        Vector3 end = _controlledObject.transform.position;
        end.y = start.y;
        Vector3 direction = end - _camera.transform.position;
        end = start + direction - direction.normalized * 2f; // keep the camera at N unit from the object

        while (timer < duration)
        {
            _camera.transform.position = Vector3.Lerp(start, end, timer / duration);
            _camera.transform.rotation = Quaternion.Lerp(_camera.transform.rotation, Quaternion.LookRotation(_controlledObject.transform.position - _camera.transform.position), timer / duration);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        _state = PlayerState.ControllingObject;
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
