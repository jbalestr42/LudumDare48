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
    public List<AActionable> actionables { get; private set; } = new List<AActionable>();
    public List<AReactionable> reactionables { get; private set; } = new List<AReactionable>();
    public List<AActionable> selfActionables = new List<AActionable>();
    public bool _hasSelfAction = false;
    public bool isLocked = false;

    Animator _animator;

    void OnEnable()
    {
        // Get all actionables in children only
        AActionable[] actionablesArray = GetComponentsInChildren<AActionable>();
        foreach (AActionable actionable in actionablesArray) {
            // If the object type is this one, it's the self action
            if (actionable is AReactionable) {
                AReactionable reactionable = actionable as AReactionable;
                reactionable.Init();
                reactionables.Add(reactionable);
            }
            if ((actionable.objectActionable & objectType) != ObjectType.Undefined) {
                selfActionables.Add(actionable);
            } else {
                actionables.Add(actionable);
            }
        }

        _animator = GetComponentInChildren<Animator>();
    }

    public bool ReactionableValidated()
    {
        bool isValidated = true;
        foreach (var reactionable in reactionables) {
            if (!reactionable.isValidated) {
                isValidated = false;
            }
        }
        return isValidated;
    }

    // private GameObject debugValidation;
    private void LateUpdate()
    {
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
    }

    public void TryDoAction()
    {
        Debug.Log("Try Do Action");
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, _actionRadius, Vector3.up, 10f);
        foreach (RaycastHit hit in hits) {
            AControlable controlable = hit.collider.gameObject.GetComponentInParent<AControlable>();
            if (controlable && hit.collider.gameObject != gameObject) {
                foreach (AActionable destActionable in controlable.actionables) {
                    if ((objectType & destActionable.objectActionable) != ObjectType.Undefined) {
                        Debug.Log($"ACTION: '{objectType}' is actioning '{controlable.objectType}'.");
                        destActionable.DoAction();
                        break;
                    }
                }
            }
        }


        DoAction();
        DoChildAction();
    }

    public void DoAction()
    {
        Debug.Log($"ACTION: '{objectType}' is self actioning.");
        foreach (var selfActionable in selfActionables) {
            selfActionable.DoAction();
        }
        TriggerAction();
    }

    public virtual void DoChildAction()
    {
        Debug.Log("Inehrit in child class if needed");
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
        _animator?.SetBool("Walk", isWalking);
    }

    public void TriggerAction()
    {
        _animator?.SetTrigger("Action");
    }
}