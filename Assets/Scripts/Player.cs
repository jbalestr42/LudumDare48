using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {
    AControlable _controlledObject = null;
    [SerializeField] Camera _camera = null;

    enum PlayerState {
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

    float mainSpeed = 10.0f; //regular speed
    float shiftAdd = 25.0f; //multiplied by how long shift is held.  Basically running
    float maxShift = 100.0f; //Maximum speed when holdin gshift
    float camSens = 0.25f; //How sensitive it with mouse
    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
    private float totalRun = 1.0f;

    void Update()
    {
        switch (_state) {
            case PlayerState.ControllingPlayer: {
                    if (Input.GetMouseButtonDown(0)) {
                        RaycastHit hit;
                        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, Mathf.Infinity)) {
                            AControlable controlable = hit.collider.gameObject.GetComponent<AControlable>();
                            if (controlable != null) {
                                Debug.Log("Object Selected: " + controlable);
                                _controlledObject = controlable;
                                _state = PlayerState.TransitionCameraToObject;
                                StartCoroutine(TransitionCameraToObject());
                            }
                        } else {
                            FPSMovement(null);
                        }
                        break;
                    } else {
                        FPSMovement(transform);
                    }
                    break;
                }
            case PlayerState.ControllingObject: {
                    if (Input.GetMouseButtonDown(0)) {
                        _controlledObject.TryDoAction();
                    } else if (Input.GetMouseButtonDown(1)) {
                        ReleaseObject();
                        _state = PlayerState.ControllingPlayer;
                    } else {
                        TPSMovement(_controlledObject.transform, _camera.transform);
                    }
                }
                break;
        }

        Vector3 GetBaseInput()
        {
            Vector3 p_Velocity = new Vector3();
            if (Input.GetKey(KeyCode.Z)) {
                p_Velocity += new Vector3(0, 0, 1);
            }
            if (Input.GetKey(KeyCode.S)) {
                p_Velocity += new Vector3(0, 0, -1);
            }
            if (Input.GetKey(KeyCode.Q)) {
                p_Velocity += new Vector3(-1, 0, 0);
            }
            if (Input.GetKey(KeyCode.D)) {
                p_Velocity += new Vector3(1, 0, 0);
            }
            return p_Velocity;
        }

        void TPSMovement(Transform transformTarget, Transform transformCamera)
        {
            lastMouse = Input.mousePosition - lastMouse;
            transformCamera.RotateAround(transformTarget.position, Vector3.up, lastMouse.x * camSens);
            transformCamera.RotateAround(transformTarget.position, -transformCamera.right, lastMouse.y * camSens);
            lastMouse = Input.mousePosition;

            Vector3 dir = Vector3.zero;
            if (Input.GetKey(KeyCode.Z)) {
                dir += new Vector3(transformCamera.forward.x, 0f, transformCamera.forward.z).normalized;
            } else if (Input.GetKey(KeyCode.S)) {
                dir += new Vector3(transformCamera.forward.x, 0f, transformCamera.forward.z).normalized * -1f;
            }
            if (Input.GetKey(KeyCode.Q)) {
                Vector3 tmp = new Vector3(transformCamera.forward.x, 0f, transformCamera.forward.z).normalized;
                dir += Vector3.Cross(tmp, Vector3.up).normalized;
            } else if (Input.GetKey(KeyCode.D)) {
                Vector3 tmp = new Vector3(transformCamera.forward.x, 0f, transformCamera.forward.z).normalized;
                dir += Vector3.Cross(tmp, Vector3.up).normalized * -1;
            }

            Vector3 p = dir.normalized * mainSpeed * Time.deltaTime;
            Vector3 newPosition = transformTarget.position;
            transformTarget.Translate(p);
            newPosition.x = transformTarget.position.x;
            newPosition.z = transformTarget.position.z;
            transformTarget.position = newPosition;
        }

        void FPSMovement(Transform transformTarget)
        {
            lastMouse = Input.mousePosition - lastMouse;
            lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
            lastMouse = new Vector3(transformTarget.eulerAngles.x + lastMouse.x, transformTarget.eulerAngles.y + lastMouse.y, 0);
            transformTarget.eulerAngles = lastMouse;
            lastMouse = Input.mousePosition;

            Vector3 p = GetBaseInput();
            if (Input.GetKey(KeyCode.LeftShift)) {
                totalRun += Time.deltaTime;
                p = p * totalRun * shiftAdd;
                p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
                p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
                p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
            } else {
                totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
                p = p * mainSpeed;
            }

            p = p * Time.deltaTime;
            Vector3 newPosition = transformTarget.position;
            transformTarget.Translate(p);
            newPosition.x = transformTarget.position.x;
            newPosition.z = transformTarget.position.z;
            transformTarget.position = newPosition;
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

            while (timer < duration) {
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
            _camera.transform.SetParent(null);
            _controlledObject = null;
        }
    }
}
