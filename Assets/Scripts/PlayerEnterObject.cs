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

    private void Update()
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

                                Vector3 positionControlledObject = _controlledObject.transform.position;
                                CharacterController controller = GetComponent<CharacterController>();
                                positionControlledObject.y = transform.position.y;

                                controller.enabled = false;
                                transform.position = positionControlledObject;
                                controller.enabled = true;
                                _controlledObjectParent = _controlledObject.transform.parent;
                                _controlledObject.transform.SetParent(transform);
                                _controlledObject.transform.forward = transform.forward;
                                _camera.GetComponent<CameraOcclusionProtector>().distanceToTarget = 8f;
                                _state = PlayerState.ControllingObject;
                                _controlledObject.SetWalking(true);
                            }
                        }
                    }
                    break;
                }
            case PlayerState.ControllingObject: {
                    if (Input.GetMouseButtonDown(0)) {
                        _controlledObject.TryDoAction();
                    } else if (Input.GetMouseButtonDown(1)) {
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
                        _controlledObject = null;
                    }
                    break;
                }
        }
        playerStateAccessor = _state;
    }

    bool IsControllingObject()
    {
        return _controlledObject != null;
    }
}
