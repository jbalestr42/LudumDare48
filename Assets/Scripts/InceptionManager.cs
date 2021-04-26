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

    public bool closeDoorIfValidate = true;

    void Start()
    {
        _player.OnObjectReleased.AddListener(CheckObjectState);
        _player.OnDoAction.AddListener(CheckObjectState);
        foreach (var controlableClose in _refMaison._controlables) {
            // controlableClose.isLocked = true;
        }
        _refMaison.ActivateObject();
        _maisons[0].ActivateObject();
    }

    void Destroy()
    {
        _player.OnObjectReleased.RemoveListener(CheckObjectState);
        _player.OnDoAction.RemoveListener(CheckObjectState);
    }

    void CheckObjectState(AControlable controlable)
    {
        Debug.Log("Check object " + _refMaison.GetObject(controlable.objectType).isReactionValidated + " " + controlable.isReactionValidated);
        if (_refMaison.CheckObjectPosisition(controlable)) {
            Debug.Log("Object placed properly -> TODO add feedback");
            SnapObject(controlable);
            SoundManager.PlaySound(Random.value > 0.5 ? Random.value > 0.5f ? "snap_1" : "snap_2" : "snap_3", controlable.transform.position);
            if (_refMaison.CheckObjectReactionState(controlable)) {
                if (_refMaison.CheckObjects(_maisons[_currentHouse])) {
                    Debug.Log("All objects are ok, opening next house.");
                    OpenNextHouse();
                    if (closeDoorIfValidate) {
                        foreach (var controlableClose in _maisons[_currentHouse - 1]._controlables) {
                            controlableClose.isLocked = true;
                        }
                    }
                }
            }
        }
    }

    void OpenNextHouse()
    {
        _currentHouse++;
        _maisons[_currentHouse - 1].OpenDoor();
        _maisons[_currentHouse].ActivateObject();
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
