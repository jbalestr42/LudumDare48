using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorActionable : AReactionable {
    public Animator _animator;

    public override void DoAction()
    {
        base.DoAction();

        _animator.SetTrigger("Grow");
        _animator.SetBool("IsOn", isValidated);
    }
}