using System;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum ObjectType {
    Undefined = 0,
    Chair = 1,
    Mug = 2,
    Closet = 4
}

public class AControlable : MonoBehaviour
{
    [SerializeField] float _radius = 1f;
    public ObjectType objectType = ObjectType.Undefined;
    public List<AActionable> actionables { get; private set; } = new List<AActionable>();
    AActionable selfActionable = null;

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
        int layerMask = 1 << LayerMask.NameToLayer("Controlable");
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, _radius, Vector3.up, 1f, layerMask);
        foreach (RaycastHit hit in hits)
        {
            AControlable controlable = hit.collider.gameObject.GetComponent<AControlable>();
            if (controlable && hit.collider.gameObject != gameObject)
            {
                bool canDoAction = false;
                foreach (AActionable destActionable in controlable.actionables)
                {
                    if ((objectType & destActionable.objectActionable) != ObjectType.Undefined)
                    {
                        canDoAction = true;
                        destActionable.DoAction();
                    }
                }

                // Use bool to avoid doing action multiple times
                if (canDoAction)
                {
                    DoAction();
                }
            }
        }
    }

    public void DoAction()
    {
        Debug.Log("Action " + gameObject.name);
        selfActionable.DoAction();
    }
}