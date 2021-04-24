using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugControlable : AActionable
{
    public override void DoAction()
    {
        Debug.Log("DebugControlable ");
    }
}