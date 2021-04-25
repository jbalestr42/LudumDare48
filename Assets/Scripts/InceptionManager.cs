using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InceptionManager : MonoBehaviour {
    [SerializeField]
    MaisonManager _refMaison = null;

    [SerializeField]
    List<MaisonManager> _maisons = new List<MaisonManager>();

    [SerializeField]
    PlayerEnterObject _player = null;
    int _currentHouse = 0;

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
        if (_refMaison.CheckObject(controlable)) {
            Debug.Log("Object placed properly -> TODO add feedback");
            SnapObject(controlable);

            if (_refMaison.CheckObjects(_maisons[_currentHouse])) {
                Debug.Log("All objects are ok, opening next house.");
                OpenNextHouse();
            }
        }
    }

    void OpenNextHouse()
    {
        _currentHouse++;
        _maisons[_currentHouse - 1].OpenDoor();
        if (_currentHouse >= _maisons.Count) {
            Debug.Log("GAME Is DONE, yeaahhhhh");
        }
    }

    void SnapObject(AControlable controlable)
    {
        Debug.Log("Snap Object");
        AControlable refControlable = _refMaison.GetObject(controlable.objectType);
        controlable.transform.localPosition = refControlable.transform.localPosition;
        controlable.transform.localRotation = refControlable.transform.localRotation;
    }
}
