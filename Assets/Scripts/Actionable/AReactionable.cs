using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AReactionable : AActionable {
    public bool isValidated = false;

    public virtual void Init()
    {
        if (isValidated) {
            DoAction();
        }
    }

    public override void DoAction()
    {
        isValidated = true;
    }
}