using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerEnterObject : MonoBehaviour {
    public enum PlayerState {
        ControllingPlayer,
        ControllingObject,
    }

    [SerializeField] private Camera _camera = null;
    [SerializeField] public float objectCameraDistance = 8f;
    [SerializeField] private AnimationCurve lockedCurve;
    [SerializeField] private GameObject cursor;

    public UnityEvent<AControlable> OnObjectReleased = new UnityEvent<AControlable>();
    public UnityEvent<AControlable> OnObjectEnter = new UnityEvent<AControlable>();
    public UnityEvent<AControlable> OnDoAction = new UnityEvent<AControlable>();

    AControlable _controlledObject = null;
    Transform _controlledObjectParent = null;
    PlayerState _state = PlayerState.ControllingPlayer;
    CameraController _cameraController;
    CameraOcclusionProtector _cameraOcclustionProtector;

    private void Start()
    {
        _cameraController = _camera.GetComponent<CameraController>();
        _cameraOcclustionProtector = _camera.GetComponent<CameraOcclusionProtector>();
        Cursor.lockState = CursorLockMode.Locked;
        InputManager.RegisterCallback("Object", ObjectExit, true);
        InputManager.RegisterCallback("Object", ObjectEnter, false);
        InputManager.RegisterCallback("Object", ActionAsObject, true);
        InputManager.RegisterCallback("Object", ActionAsPlayer, false);
    }

    private void OnDestroy()
    {
        InputManager.RegisterCallback("Object", ObjectEnter, true);
        InputManager.RegisterCallback("Object", ObjectExit, true);
        InputManager.RegisterCallback("Action", ActionAsPlayer, true);
        InputManager.RegisterCallback("Action", ActionAsObject, true);
    }

    private void ActionAsPlayer(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, Mathf.Infinity)) {
            AControlable controlable = hit.collider.gameObject.GetComponentInParent<AControlable>();
            if (controlable != null && controlable.isLocked) {
                controlable.TryDoAction();
                OnDoAction.Invoke(controlable);
            }
        }
    }

    private void ActionAsObject(InputAction.CallbackContext context)
    {
        _controlledObject.TryDoAction();
        OnDoAction.Invoke(_controlledObject);
    }

    private void Update()
    {
        if (_state == PlayerState.ControllingPlayer) {
            _cameraOcclustionProtector.distanceToTarget = Mathf.Max(0f, _cameraOcclustionProtector.distanceToTarget - Time.deltaTime * 20f);
            _cameraController.catchSpeedDamp = Mathf.Max(0f, _cameraController.catchSpeedDamp - Time.deltaTime);
        }
    }

    private void ObjectEnter(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, Mathf.Infinity)) {
            AControlable controlable = hit.collider.gameObject.GetComponentInParent<AControlable>();
            if (controlable != null) {
                if (!controlable.isLocked) {
                    Debug.Log("Object Selected: " + controlable);
                    _controlledObject = controlable;

                    Vector3 positionControlledObject = _controlledObject.transform.position;
                    CharacterController controller = GetComponent<CharacterController>();

                    controller.enabled = false;
                    transform.position = positionControlledObject;
                    controller.enabled = true;
                    _controlledObjectParent = _controlledObject.transform.parent;
                    _controlledObject.transform.SetParent(transform);
                    _controlledObject.transform.forward = transform.forward;
                    _controlledObject.transform.localPosition = Vector3.zero;
                    _state = PlayerState.ControllingObject;
                    _controlledObject.SetWalking(true);
                    SoundManager.PlaySound("control_1", _controlledObject.transform.position);
                    cursor.SetActive(false);

                    InputManager.RegisterCallback("Object", ObjectExit, false);
                    InputManager.RegisterCallback("Object", ObjectEnter, true);
                    InputManager.RegisterCallback("Object", ActionAsObject, false);
                    InputManager.RegisterCallback("Object", ActionAsPlayer, true);

                    _cameraOcclustionProtector.distanceToTarget = objectCameraDistance;
                    _cameraController.catchSpeedDamp = 0.4f;
                } else {
                    SoundManager.PlaySound("lock_1", hit.point);
                }
            }
        }
    }

    private void ObjectExit(InputAction.CallbackContext context)
    {
        if (_controlledObject.isActionAvailaible) {
            return;
        }
        Vector3 positionCamera = _camera.transform.position;
        Vector3 controledPosition = _controlledObject.transform.position;
        positionCamera.y = transform.position.y;
        gameObject.GetComponent<CharacterController>().Move(positionCamera - transform.position);
        // transform.position = positionCamera;
        _controlledObject.transform.SetParent(_controlledObjectParent);
        _controlledObject.transform.position = controledPosition;
        _controlledObject.SetWalking(false);
        OnObjectReleased.Invoke(_controlledObject);
        _state = PlayerState.ControllingPlayer;
        SoundManager.PlaySound("release_2", _controlledObject.transform.position);
        _controlledObject = null;
        cursor.SetActive(true);
        InputManager.RegisterCallback("Object", ObjectExit, true);
        InputManager.RegisterCallback("Object", ObjectEnter, false);
        InputManager.RegisterCallback("Object", ActionAsObject, true);
        InputManager.RegisterCallback("Object", ActionAsPlayer, false);
    }

    private IEnumerator LockedCoroutine(AControlable controlable)
    {
        float duration = 0.3f;
        float time = 0f;
        Vector3 originPosition = controlable.transform.position;

        while (time < duration) {
            time += Time.deltaTime;
            controlable.transform.position = originPosition + Vector3.up * lockedCurve.Evaluate(time / duration);
            yield return null;
        }
        controlable.transform.position = originPosition;
    }

    bool IsControllingObject()
    {
        return _controlledObject != null;
    }
}
