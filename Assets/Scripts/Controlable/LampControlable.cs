using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampControlable : AControlable {
    public bool isTurnedOn = false;

    // public override void DoChildAction()
    // {
    //     isTurnedOn = !isTurnedOn;
    //     Debug.Log("Lamp state : isTurnedOn=" + isTurnedOn);
    // }

    // public override bool IsSameState(AControlable controlable)
    // {
    //     LampControlable lampControlable = controlable as LampControlable;
    //     return base.IsSameState(controlable) && lampControlable != null && isTurnedOn == lampControlable.isTurnedOn;
    // }
}
