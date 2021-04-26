using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorActionable : AActionable {
    public Animator _animator;

    private void Start()
    {
        isReaction = true;
    }

    public override void DoAction()
    {
        GetComponentInParent<AControlable>().isReactionValidated = true;
        _animator.SetTrigger("Grow");
        _animator.SetBool("IsOn", true);
        isValidated = true;
    }
}