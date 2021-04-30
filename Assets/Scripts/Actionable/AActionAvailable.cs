using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AActionAvailable : AActionable {
    public virtual void Enable() { }
    public virtual void Disable() { }
}