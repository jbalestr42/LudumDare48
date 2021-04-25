using System.Collections;
using UnityEngine;
using UnityEngine.Events;

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

    public UnityEvent<AControlable> OnObjectReleased = new UnityEvent<AControlable>();


    float mainSpeed = 10.0f; //regular speed
    float shiftAdd = 25.0f; //multiplied by how long shift is held.  Basically running
    float maxShift = 100.0f; //Maximum speed when holdin gshift
    float camSens = 0.25f; //How sensitive it with mouse
    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
    private float totalRun = 1.0f;

    private Rigidbody rb;
    private Collider colliderPlayer;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Debug.Log("RigidBody : " + rb);
        colliderPlayer = GetComponent<Collider>();
        _camera.transform.LookAt(transform.position);
    }

    void FixedUpdate()
    {
        switch (_state) {
            case PlayerState.ControllingPlayer: {
                    if (Input.GetMouseButtonDown(0)) {
                        RaycastHit hit;
                        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, Mathf.Infinity)) {
                            AControlable controlable = hit.collider.gameObject.GetComponentInParent<AControlable>();
                            if (controlable != null) {
                                Debug.Log("Object Selected: " + controlable);
                                _controlledObject = controlable;
                                _state = PlayerState.TransitionCameraToObject;
                                StartCoroutine(TransitionCameraToObject());
                            }
                        }
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
                    break;
                }
        }
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

    Rigidbody tpsRb = null;
    void TPSMovement(Transform transformTarget, Transform transformCamera)
    {
        if (tpsRb == null) {
            tpsRb = transformTarget.GetComponent<Rigidbody>();
            if (tpsRb == null) {
                tpsRb = transformTarget.gameObject.AddComponent<Rigidbody>();
                tpsRb.constraints = RigidbodyConstraints.FreezeRotation;
                tpsRb.useGravity = false;
            }
        }
        lastMouse = Input.mousePosition - lastMouse;
        transformTarget.RotateAround(transformTarget.position, Vector3.up, lastMouse.x * camSens);
        rb.rotation = transformTarget.rotation;
        lastMouse = Input.mousePosition;

        Vector3 p = GetBaseInput() * mainSpeed;
        Vector3 cameraNextPosition = (tpsRb.position - transformTarget.forward * 5f + transformTarget.up * 4f);
        rb.velocity = cameraNextPosition - rb.position;
        RigidBodyLookAt(tpsRb.transform, rb);

        p = Vector3.ProjectOnPlane(rb.transform.forward * p.z + rb.transform.right * p.x, Vector3.up);
        tpsRb.velocity = p;
    }

    void RigidBodyLookAt(Transform target, Rigidbody rb)
    {
        var targetDir = target.position - rb.transform.position;
        var forward = rb.transform.forward;
        var localTarget = rb.transform.InverseTransformPoint(target.position);

        float angle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

        var eulerAngleVelocity = new Vector3(0, angle, 0);
        var deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    void FPSMovement(Transform transformTarget)
    {
        tpsRb = null;
        lastMouse = Input.mousePosition - lastMouse;
        lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
        lastMouse = new Vector3(transformTarget.eulerAngles.x + lastMouse.x, transformTarget.eulerAngles.y + lastMouse.y, 0);
        transformTarget.eulerAngles = lastMouse;
        lastMouse = Input.mousePosition;

        Vector3 p = GetBaseInput();
        p = p * mainSpeed;

        p = Vector3.ProjectOnPlane(transform.forward * p.z + transform.right * p.x, Vector3.up);
        rb.velocity = p;
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
        colliderPlayer.enabled = false;
        _controlledObject = obj;
    }

    void ReleaseObject()
    {
        colliderPlayer.enabled = true;
        OnObjectReleased.Invoke(_controlledObject);
        _camera.transform.SetParent(null);
        _controlledObject = null;
    }
}