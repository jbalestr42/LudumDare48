using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectActionable : AActionable {
    public GameObject _prefab;

    public override void DoAction()
    {
        Instantiate(_prefab);
    }
}