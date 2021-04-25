using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorActionable : AActionable
{
    Animator _animator;

    public override void DoAction()
    {
        _animator.SetTrigger("Reaction");
    }
}