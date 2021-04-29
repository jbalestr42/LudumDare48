using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEnterObject : MonoBehaviour {
    static public PlayerState playerStateAccessor;

    public enum PlayerState {
        ControllingPlayer,
        ControllingObject,
    }

    AControlable _controlledObject = null;
    Transform _controlledObjectParent = null;
    [SerializeField] Camera _camera = null;
    PlayerState _state = PlayerState.ControllingPlayer;

    public UnityEvent<AControlable> OnObjectReleased = new UnityEvent<AControlable>();
    public UnityEvent<AControlable> OnDoAction = new UnityEvent<AControlable>();

    [SerializeField] private AnimationCurve lockedCurve;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        switch (_state) {
            case PlayerState.ControllingPlayer: {
                    if (Input.GetMouseButtonDown(1)) {
                        RaycastHit hit;
                        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, Mathf.Infinity)) {
                            AControlable controlable = hit.collider.gameObject.GetComponentInParent<AControlable>();
                            if (controlable != null) {
                                if (Input.GetMouseButtonDown(1)) {
                                    controlable.TryDoAction();
                                    OnDoAction.Invoke(controlable);
                                    return;
                                }
                            }
                        }
                    }
                    if (Input.GetMouseButtonDown(0)) {
                        RaycastHit hit;
                        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, Mathf.Infinity)) {
                            AControlable controlable = hit.collider.gameObject.GetComponentInParent<AControlable>();
                            if (controlable != null) {
                                if (Input.GetMouseButtonDown(1)) {
                                    controlable.TryDoAction();
                                    OnDoAction.Invoke(controlable);
                                    return;
                                }
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
                                    _camera.GetComponent<CameraOcclusionProtector>().distanceToTarget = 8f;
                                    _state = PlayerState.ControllingObject;
                                    _controlledObject.SetWalking(true);
                                    SoundManager.PlaySound("control_1", _controlledObject.transform.position);
                                } else {
                                    SoundManager.PlaySound(Random.value > 0.5 ? "lock_1" : "lock_2", hit.point);
                                    StartCoroutine(LockedCoroutine(controlable));
                                }
                            }
                        }
                    }
                    break;
                }
            case PlayerState.ControllingObject: {
                    if (Input.GetMouseButtonDown(1)) {
                        _controlledObject.TryDoAction();
                        OnDoAction.Invoke(_controlledObject);
                    } else if (Input.GetMouseButtonDown(0)) {
                        Vector3 positionCamera = _camera.transform.position;
                        Vector3 controledPosition = _controlledObject.transform.position;
                        positionCamera.y = transform.position.y;
                        gameObject.GetComponent<CharacterController>().Move(positionCamera - transform.position);
                        // transform.position = positionCamera;
                        _controlledObject.transform.SetParent(_controlledObjectParent);
                        _controlledObject.transform.position = controledPosition;
                        _camera.GetComponent<CameraOcclusionProtector>().distanceToTarget = 0f;
                        _controlledObject.SetWalking(false);
                        OnObjectReleased.Invoke(_controlledObject);
                        _state = PlayerState.ControllingPlayer;
                        SoundManager.PlaySound("release_1", _controlledObject.transform.position);
                        _controlledObject = null;
                    }
                    break;
                }
        }
        playerStateAccessor = _state;
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
