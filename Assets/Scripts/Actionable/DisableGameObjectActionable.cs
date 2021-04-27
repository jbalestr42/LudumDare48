using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableGameObjectActionable : AActionable {
    public GameObject _gameObject;

    public override void DoAction()
    {
        _gameObject.SetActive(false);
        StartCoroutine(Reactivate());
    }

    IEnumerator Reactivate()
    {
        yield return new WaitForSeconds(4f);
        _gameObject.SetActive(true);
    }
}