using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AActionable : MonoBehaviour {
    // Determine chich type can activate this object
    public ObjectType objectActionable = ObjectType.Undefined;
    public bool isValidated = false;
    public bool isReaction = false;
    public abstract void DoAction();
}