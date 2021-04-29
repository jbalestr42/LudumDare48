using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVActionable : AReactionable {
    [SerializeField] private GameObject screen;

    public override void Init()
    {
        screen.SetActive(false);
        base.Init();
    }

    public override void DoAction()
    {
        base.DoAction();
        screen.SetActive(isValidated);
    }
}