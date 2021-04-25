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
    Tapis = 9192,
    TableBasse = 16384,
    Plante = 32768,
    Verre = 129072,
}

public class AControlable : MonoBehaviour
{
    [SerializeField] float _radius = 1f;
    public ObjectType objectType = ObjectType.Undefined;
    public List<AActionable> actionables { get; private set; } = new List<AActionable>();
    AActionable selfActionable = null;
    public bool _hasSelfAction = false;

    void Start()
    {
        // Get all actionables in children only
        AActionable[] actionablesArray = GetComponentsInChildren<AActionable>();
        foreach (AActionable actionable in actionablesArray)
        {
            // If the object type is this one, it's the self action
            if ((actionable.objectActionable & objectType) != ObjectType.Undefined)
            {
                selfActionable = actionable;
            }
            else
            {
                actionables.Add(actionable);
            }
        }
    }

    public void TryDoAction()
    {
        bool canDoAction = false;
        int layerMask = 1 << LayerMask.NameToLayer("Controlable");
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, _radius, Vector3.up, 1f, layerMask);
        foreach (RaycastHit hit in hits)
        {
            AControlable controlable = hit.collider.gameObject.GetComponent<AControlable>();
            if (controlable && hit.collider.gameObject != gameObject)
            {
                foreach (AActionable destActionable in controlable.actionables)
                {
                    if ((objectType & destActionable.objectActionable) != ObjectType.Undefined)
                    {
                        canDoAction = true;
                        Debug.Log($"ACTION: '{objectType}' is actioning '{controlable.objectType}'.");
                        destActionable.DoAction();
                    }
                }
            }
        }

        // Use bool to avoid doing action multiple times
        if (canDoAction || _hasSelfAction)
        {
            DoAction();
            DoChildAction();
        }
    }

    public void DoAction()
    {
        Debug.Log($"ACTION: '{objectType}' is self actioning.");
        selfActionable.DoAction();
    }

    public virtual void DoChildAction()
    {
        Debug.Log("Inehrit in child class if needed");
    }

    public virtual bool IsSameState(AControlable controlable)
    {
        //TODO: check other state, is lamp turned on ?

        // Check position
        Vector3 diff = transform.localPosition - controlable.transform.localPosition;
        Debug.Log("Diff " + diff  + " - " + diff.magnitude);
        return diff.magnitude < 1f;
    }
}