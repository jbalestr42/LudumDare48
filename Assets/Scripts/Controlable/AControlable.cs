using System;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum ObjectType {
    Undefined = 0,
    Chair = 1,
    Mug = 2,
    Closet = 4,
    Lamp = 8,
    TV = 16,
    Ventilo = 32,
    Frigo = 64,
    Platine = 128,
    Vynil = 256,
    Poulet = 512,
    Four = 1024,
    Table = 2048,
    Canape = 4096,
    Tapis = 8192,
    TableBasse = 16384,
    Plante = 32768,
    Verre = 65536,
    Carafe = 131072,
}

public class AControlable : MonoBehaviour {
    [SerializeField] float _actionRadius = 1f;
    [SerializeField] float _distanceSnapMinimum = 1f;
    public ObjectType objectType = ObjectType.Undefined;
    public List<AActionable> actionableList { get; private set; } = new List<AActionable>();
    public List<AReactionable> reactionableList { get; private set; } = new List<AReactionable>();
    public List<AActionAvailable> actionAvailableList { get; private set; } = new List<AActionAvailable>();
    public bool isLocked = false;
    public bool isActionAvailaible = false;
    public bool isSnapped = false;
    public Rigidbody rb;
    public bool hasRigidbody = true;
    public Vector3 originLocalPosition;
    public Quaternion originRotation;
    public Transform controlableParent;
    public bool isSnapping = false;
    public bool startedLocked = false;

    Animator _animator;
    MaisonManager _maisonManager;
    List<AActionable> actionableDoneList = new List<AActionable>();

    void OnEnable()
    {
        // Get all actionableList in children only
        AActionable[] actionablesArray = GetComponentsInChildren<AActionable>();
        foreach (AActionable actionable in actionablesArray) {
            if (actionable is AReactionable) {
                AReactionable reactionable = actionable as AReactionable;
                reactionable.Init();
                reactionableList.Add(reactionable);
            }
            if (actionable is AActionAvailable) {
                AActionAvailable actionAvailable = actionable as AActionAvailable;
                actionAvailableList.Add(actionAvailable);
            } else {
                actionableList.Add(actionable);
            }
        }

        _animator = GetComponentInChildren<Animator>();
        _maisonManager = GetComponentInParent<MaisonManager>();
        isSnapped = isLocked;
        if (hasRigidbody) {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.centerOfMass = Vector3.zero;
            rb.mass = 2f;
            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
        originLocalPosition = transform.localPosition;
        originRotation = transform.rotation;
        controlableParent = transform.parent;
        startedLocked = isLocked;
    }

    public bool ReactionableValidated()
    {
        bool isValidated = true;
        foreach (var reactionable in reactionableList) {
            if (!reactionable.isValidated) {
                isValidated = false;
            }
        }
        return isValidated;
    }

    private void Update()
    {
        if (actionAvailableList.Count > 0) {
            isActionAvailaible = false;
            foreach (var controlable in _maisonManager._controlables) {
                foreach (AReactionable reactionable in controlable.reactionableList) {
                    if (controlable != this && (objectType & reactionable.objectActionable) != ObjectType.Undefined) {
                        float distance = Vector3.Distance(controlable.transform.position, transform.position);
                        if (distance < _actionRadius && !reactionable.isValidated) {
                            foreach (AActionAvailable actionAvailable in actionAvailableList) {
                                isActionAvailaible = true;
                                actionAvailable.Enable();
                            }
                            // Make receiver emit too
                            var receiverActionControlable = _maisonManager.GetObject(controlable.objectType);
                            foreach (AActionAvailable actionAvailable in receiverActionControlable.actionAvailableList) {
                                actionAvailable.Enable();
                            }
                            return;
                        }
                    }
                }
            }
        }
    }

    private void LateUpdate()
    {
        foreach (AActionAvailable actionAvailable in actionAvailableList) {
            actionAvailable.Disable();
        }
    }

    // private GameObject debugValidation;
    // private void LateUpdate()
    // {
    // if (ReactionableValidated()) {
    //     if (debugValidation == null) {
    //         debugValidation = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    //         debugValidation.transform.localScale = Vector3.one * 0.1f + Vector3.up * 10f;
    //         Destroy(debugValidation.GetComponent<Collider>());
    //     }
    // } else {
    //     if (debugValidation != null) {
    //         Destroy(debugValidation);
    //     }
    // }
    // if (debugValidation != null) {
    //     debugValidation.transform.position = transform.position;
    // }
    // }

    public void TryDoAction()
    {
        Debug.Log("Try Do Action");
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, _actionRadius, Vector3.up, 10f);
        foreach (RaycastHit hit in hits) {
            AControlable controlable = hit.collider.gameObject.GetComponentInParent<AControlable>();
            if (controlable) {
                // Prevent trigger if there is two collider for the same controlable
                foreach (AActionable destActionable in controlable.actionableList) {
                    if ((objectType & destActionable.objectActionable) != ObjectType.Undefined
                        && !actionableDoneList.Contains(destActionable)) {
                        actionableDoneList.Add(destActionable);
                        Debug.Log($"ACTION: '{objectType}' is actioning '{controlable.objectType}'.");
                        destActionable.DoAction();
                    }
                }
            }
        }
        actionableDoneList.Clear();
        TriggerAction();
    }

    public virtual bool IsSameState(AControlable controlable)
    {
        Vector3 position1 = transform.root.position - transform.position;
        Vector3 position2 = controlable.transform.root.position - controlable.transform.position;
        float distance = Vector3.Distance(position1, position2);

        return distance < _distanceSnapMinimum;
    }

    public virtual bool IsValidated(AControlable controlable)
    {
        return ReactionableValidated() == controlable.ReactionableValidated();
    }

    public void SetWalking(bool isWalking)
    {
        if (isWalking == true && hasRigidbody) {
            rb.isKinematic = true;
        }
        _animator?.SetBool("Walk", isWalking);
    }

    public void TriggerAction()
    {
        _animator?.SetTrigger("Action");
    }
}