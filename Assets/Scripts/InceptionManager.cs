using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InceptionManager : MonoBehaviour
{
    [SerializeField]
    MaisonManager _refMaison = null;

    [SerializeField]
    List<MaisonManager> _maisons = new List<MaisonManager>();

    [SerializeField]
    Player _player = null;

    void Start()
    {
        _player.OnObjectReleased.AddListener(CheckObjectState);
    }

    void Destroy()
    {
        _player.OnObjectReleased.AddListener(CheckObjectState);
    }

    void CheckObjectState(AControlable controlable)
    {
        if (_refMaison.CheckObject(controlable))
        {
            Debug.Log("Object placed properly -> TODO add feedback");
            SnapObject(controlable);
        }
    }

    void SnapObject(AControlable controlable)
    {
        AControlable refControlable = _refMaison.GetObject(controlable.objectType);
        controlable.transform.localPosition = refControlable.transform.localPosition;
        controlable.transform.localRotation = refControlable.transform.localRotation;
    }
}
