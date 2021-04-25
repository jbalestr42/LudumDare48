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
                                positionControlledObject.y = transform.position.y;
                                transform.position = positionControlledObject;
                                _controlledObject.transform.SetParent(transform);
                                _camera.GetComponent<CameraOcclusionProtector>().distanceToTarget = 8f;
                                // Change character controller here
                            }
                        }
                    }
                    break;
                }
            case PlayerState.ControllingObject: {
                    if (Input.GetMouseButtonDown(0)) {
                        _controlledObject.TryDoAction();
                    } else if (Input.GetMouseButtonDown(1)) {
                        ReleaseObject();
                        _state = PlayerState.ControllingPlayer;
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

    void ControlObject(AControlable obj)
    {
        _controlledObject = obj;
    }

    void ReleaseObject()
    {
        OnObjectReleased.Invoke(_controlledObject);
        _camera.transform.SetParent(null);
        _controlledObject = null;
    }
}