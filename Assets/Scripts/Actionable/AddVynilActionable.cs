using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddVynilActionable : AActionable {
    public Animator _animator;
    [HideInInspector] public bool isGrow = true;
    [SerializeField] public GameObject vynil;

    private void Awake()
    {
        isReaction = true;
        vynil.SetActive(false);
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
        vynil.SetActive(true);
        isValidated = true;
        GetComponentInParent<AControlable>().isReactionValidated = true;
    }
}