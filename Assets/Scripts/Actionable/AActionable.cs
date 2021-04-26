using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AActionable : MonoBehaviour {
    // Determine chich type can activate this object
    public ObjectType objectActionable = ObjectType.Undefined;
    [HideInInspector] public bool isValidated = false;
    [HideInInspector] public bool isReaction = false;
    public abstract void DoAction();
}