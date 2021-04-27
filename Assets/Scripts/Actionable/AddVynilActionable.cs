using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddVynilActionable : AReactionable {
    public Animator _animator;
    [HideInInspector] public bool isGrow = true;
    [SerializeField] public GameObject vynil;

    public override void Init()
    {
        vynil.SetActive(false);
        base.Init();
    }

    public override void DoAction()
    {
        base.DoAction();
        Debug.Log("DO 'REACTION " + transform.parent.name, this);
        if (isGrow) {
            _animator.SetTrigger("Grow");
        }
        _animator.SetBool("IsOn", true);
        vynil.SetActive(true);
    }
}