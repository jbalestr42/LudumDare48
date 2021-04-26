using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorActionable : AActionable {
    public Animator _animator;

    private void Awake()
    {
        isReaction = true;
    }

    public override void DoAction()
    {
        Debug.Log("DO 'REACTION " + transform.parent.name, this);
        _animator.SetTrigger("Grow");
        _animator.SetBool("IsOn", true);
        isValidated = true;
        GetComponentInParent<AControlable>().isReactionValidated = true;
    }
}