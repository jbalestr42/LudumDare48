using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaisonManager : MonoBehaviour
{
    List<AControlable> _controlables = new List<AControlable>();

    void Start()
    {
        _controlables.AddRange(GetComponentsInChildren<AControlable>());
    }

    void Update()
    {
        
    }

    public bool CheckObjects(MaisonManager maison)
    {
        foreach (AControlable controlable in _controlables)
        {
            if (!IsSameState(controlable, maison.GetObject(controlable.objectType)))
            {
                return false;
            }
        }
        return true;
    }

    public bool CheckObject(AControlable controlable)
    {
        return IsSameState(GetObject(controlable.objectType), controlable);
    }

    public bool IsSameState(AControlable refControlable, AControlable controlable)
    {
        //TODO: return refControlable.IsSameState(controlable); check other state

        // Is position near the reference controlable
        Vector3 diff = refControlable.transform.localPosition - controlable.transform.localPosition;
        Debug.Log("Diff " + diff  + " - " + diff.magnitude);
        return diff.magnitude < 1f;
    }

    public AControlable GetObject(ObjectType type)
    {
        return _controlables.Find(o => o.objectType == type);
    }
}
