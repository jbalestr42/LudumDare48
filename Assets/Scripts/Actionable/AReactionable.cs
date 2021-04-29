using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AReactionable : AActionable {
    public bool isValidated = false;
    public bool stayValidated = false;

    public virtual void Init()
    {
        if (isValidated) {
            isValidated = false;
            DoAction();
        }
    }

    public override void DoAction()
    {
        isValidated = !isValidated || stayValidated;
    }
}