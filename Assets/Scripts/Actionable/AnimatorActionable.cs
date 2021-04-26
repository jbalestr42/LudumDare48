using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorActionable : AActionable {
    public Animator _animator;
    [HideInInspector] public bool isGrow = true;

    private void Awake()
    {
        isReaction = true;
    }

    private void OnEnable()
    {
        if (isValidated) {
            DoAction();
        }
    }

    public override void DoAction()
    {
        Debug.Log("DO 'REACTION " + transform.parent.name, this);
        if (isGrow) {
            _animator.SetTrigger("Grow");
        }
        _animator.SetBool("IsOn", true);
        isValidated = true;
        GetComponentInParent<AControlable>().isReactionValidated = true;
    }
}