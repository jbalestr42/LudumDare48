using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugActionable : AActionable {
    public override void DoAction()
    {
        Debug.Log("DebugControlable ");
    }
}