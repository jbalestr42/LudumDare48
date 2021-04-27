﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorActionable : AReactionable {
    public Animator _animator;
    [HideInInspector] public bool isGrow = true;

    public override void DoAction()
    {
        base.DoAction();

        Debug.Log("DO 'REACTION " + transform.parent.name, this);
        if (isGrow) {
            _animator.SetTrigger("Grow");
        }
        _animator.SetBool("IsOn", true);
        isValidated = true;
    }
}