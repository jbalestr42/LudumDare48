using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVActionable : AActionable {
    [SerializeField] private GameObject screen;

    private void Awake()
    {
        isReaction = true;
        screen.SetActive(false);
    }

    private void OnEnable()
    {
        Debug.Log("On Enable TV" + isValidated);
        if (isValidated) {
            DoAction();
        }
    }

    public override void DoAction()
    {
        Debug.Log("DO 'REACTION " + transform.parent.name, this);
        screen.SetActive(true);
        isValidated = true;
        GetComponentInParent<AControlable>().isReactionValidated = true;
    }
}