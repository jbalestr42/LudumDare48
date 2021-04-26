using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableGameObjectActionable : AActionable {
    public GameObject _gameObject;

    public override void DoAction()
    {
        _gameObject.SetActive(false);
        Debug.Log("Disalbe gameObject " + _gameObject);
    }
}