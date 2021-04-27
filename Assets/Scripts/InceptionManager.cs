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

    public bool debugNextHouse = true;
    public bool closeDoorIfValidate = true;

    void Start()
    {
        _player.OnObjectReleased.AddListener(CheckObjectState);
        _player.OnDoAction.AddListener(CheckObjectState);
        foreach (var controlableClose in _refMaison._controlables) {
            controlableClose.isLocked = true;
        }
        _refMaison.ActivateObject();
        _maisons[0].ActivateObject();
    }

    void Destroy()
    {
        _player.OnObjectReleased.RemoveListener(CheckObjectState);
        _player.OnDoAction.RemoveListener(CheckObjectState);
    }

    bool IsNeeded(AControlable controlable)
    {
        foreach (AControlable houseControlable in _maisons[_currentHouse]._controlables) {
            if (!houseControlable.ReactionableValidated()) {
                foreach (AReactionable reactionable in houseControlable.reactionables) {
                    if (reactionable.objectActionable == controlable.objectType) {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    void CheckObjectState(AControlable controlable)
    {
        if (_refMaison.CheckObjectState(controlable) && !IsNeeded(controlable)) {
            Debug.Log("Object placed properly -> TODO add feedback");
            SnapObject(controlable);
            if (!_refMaison.CheckValidated(controlable)) {
                return;
            }
            controlable.isLocked = true;
            SoundManager.PlaySound(Random.value > 0.5 ? Random.value > 0.5f ? "snap_1" : "snap_2" : "snap_3", controlable.transform.position);
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

    void OpenNextHouse()
    {
        _currentHouse++;
        _maisons[_currentHouse - 1].OpenDoor();
        if (_currentHouse >= _maisons.Count) {
            Debug.Log("GAME Is DONE, yeaahhhhh");
        } else {
            _maisons[_currentHouse].ActivateObject();
        }
    }

    void SnapObject(AControlable controlable)
    {
        Debug.Log("Snap Object");
        AControlable refControlable = _refMaison.GetObject(controlable.objectType);
        StartCoroutine(SnapObjectCoroutine(controlable, refControlable));
    }

    IEnumerator SnapObjectCoroutine(AControlable controlable, AControlable refControlable)
    {
        Vector3 originPosition = controlable.transform.localPosition;
        Vector3 refPosition = refControlable.transform.localPosition;
        Quaternion originRotation = controlable.transform.localRotation;
        Quaternion refRotation = refControlable.transform.localRotation;
        float time = 1f;
        Renderer renderer = GetComponent<Renderer>();

        while (time > 0f) {
            time -= Time.deltaTime;
            controlable.transform.localRotation = Quaternion.Lerp(refRotation, originRotation, time);
            controlable.transform.localPosition = Vector3.Lerp(refPosition, originPosition, time);
            yield return null;
        }
        yield return null;
    }

    public void End()
    {
        List<AControlable> controlableList = new List<AControlable>();

        foreach (var controlable in _refMaison._controlables) {
            controlableList.Add(controlable);
        }
        foreach (var maison in _maisons) {
            foreach (var controlable in maison._controlables) {
                controlableList.Add(controlable);
            }
        }
        foreach (var controlable in controlableList) {
            Animator animator = controlable.GetComponentInChildren<Animator>();
            if (animator != null) {
                animator.SetBool("Walk", true);
            }
            // controlable.isLocked = false;
        }
    }
}
