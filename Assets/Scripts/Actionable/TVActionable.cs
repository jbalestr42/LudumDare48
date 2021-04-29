using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVActionable : AReactionable {
    [SerializeField] private GameObject screen;
    [SerializeField] private float delay;

    public override void Init()
    {
        screen.SetActive(false);
        base.Init();
    }

    public override void DoAction()
    {
        base.DoAction();
        StartCoroutine(OnOffCoroutine(isValidated));
    }

    IEnumerator OnOffCoroutine(bool isOn)
    {
        yield return new WaitForSeconds(delay);
        screen.SetActive(isOn);
    }
}