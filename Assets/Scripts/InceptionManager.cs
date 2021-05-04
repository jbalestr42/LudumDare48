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
    public AnimationCurve animationCurve;

    void Start()
    {
        _player.OnObjectReleased.AddListener(CheckObjectState);
        _player.OnDoAction.AddListener(CheckObjectState);
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
        if (_currentHouse >= _maisons.Count) {
            return false;
        }
        foreach (AControlable houseControlable in _maisons[_currentHouse]._controlables) {
            if (!houseControlable.ReactionableValidated() && controlable.objectType != houseControlable.objectType) {
                foreach (AReactionable reactionable in houseControlable.reactionableList) {
                    if (reactionable.objectActionable == controlable.objectType) {
                        return true;
                    }
                }
            }
        }
        return false;
    }


    public bool IsLost()
    {
        int currentLockedObject = 0;
        if (_currentHouse >= _maisons.Count) {
            return false;
        }
        foreach (var controlable in _maisons[_currentHouse]._controlables) {
            AControlable refControlable = _refMaison.GetObject(controlable.objectType);
            Vector3 position1 = refControlable.transform.root.position - refControlable.transform.position;
            Vector3 position2 = controlable.transform.root.position - controlable.transform.position;
            float distance = Vector3.Distance(position1, position2);

            if (distance < 1f) {
                currentLockedObject++;
            } else {
                controlable.isSnapped = false;
                controlable.isLocked = false;
            }
        }
        if (currentLockedObject < _maisons[_currentHouse].originLockedObject - 1) {
            return true;
        }
        return false;
    }

    private void Update()
    {
        if (IsLost()) {
            foreach (var controlable in _maisons[_currentHouse]._controlables) {
                SnapObject(controlable, true);
            }
            _player.ObjectExit(new UnityEngine.InputSystem.InputAction.CallbackContext());
        }
    }

    void CheckObjectState(AControlable controlable)
    {
        if (_refMaison.CheckObjectState(controlable) && !IsNeeded(controlable)) {
            SnapObject(controlable);
            controlable.isSnapped = true;
            if (!_refMaison.CheckValidated(controlable)) {
                return;
            }
            if (!controlable.isLocked) {
                controlable.isLocked = true;
                SoundManager.PlaySound(Random.value > 0.5 ? Random.value > 0.5f ? "snap_1" : "snap_2" : "snap_3", controlable.transform.position);
            }
            if (_refMaison.CheckObjects(_maisons[_currentHouse])) {
                Debug.Log("All objects are ok, opening next house.");
                OpenNextHouse();
                if (closeDoorIfValidate) {
                    foreach (var controlableClose in _maisons[_currentHouse - 1]._controlables) {
                        controlableClose.isLocked = true;
                    }
                }
            }
        } else {
            controlable.isSnapped = false;
            controlable.isLocked = false;
        }
    }

    void OpenNextHouse()
    {
        StartCoroutine(ObjectLockCoroutine());
        _currentHouse++;
        _maisons[_currentHouse - 1].OpenDoor();
        if (_currentHouse >= _maisons.Count) {
            Debug.Log("GAME Is DONE, yeaahhhhh");
        } else {
            _maisons[_currentHouse].ActivateObject();
        }
    }

    void SnapObject(AControlable controlable, bool isReset = false)
    {
        Debug.Log("Snap Object");
        AControlable refControlable = _refMaison.GetObject(controlable.objectType);
        StartCoroutine(SnapObjectCoroutine(controlable, refControlable, isReset));
    }

    IEnumerator SnapObjectCoroutine(AControlable controlable, AControlable refControlable, bool isReset)
    {
        Vector3 originPosition = controlable.controlableParent.InverseTransformPoint(controlable.transform.position);
        Quaternion originRotation = controlable.transform.rotation;
        Vector3 refPosition = refControlable.transform.localPosition;
        Quaternion refRotation = refControlable.transform.rotation;
        if (isReset) {
            refPosition = controlable.originLocalPosition;
            refRotation = controlable.originRotation;
        }
        float time = 1f;
        Renderer renderer = GetComponent<Renderer>();

        controlable.rb.isKinematic = true;
        controlable.rb.detectCollisions = false;

        while (time > 0f) {
            time -= Time.deltaTime;

            controlable.transform.rotation = Quaternion.Lerp(refRotation, originRotation, time);
            Vector3 localPosition = Vector3.Lerp(refPosition, originPosition, time);
            // localPosition.y += animationCurve.Evaluate(time);
            controlable.transform.localPosition = localPosition;

            yield return null;
        }
        controlable.rb.isKinematic = false;
        controlable.rb.detectCollisions = true;
        controlable.rb.velocity = Vector3.zero;
        yield return null;
    }

    IEnumerator ObjectLockCoroutine()
    {
        if (_currentHouse >= _maisons.Count) {
            yield return null;
        }
        int i = 6;
        while (i > 0) {
            foreach (var otherControlable in _maisons[_currentHouse]._controlables) {
                Animator animator = otherControlable.GetComponentInChildren<Animator>();
                if (animator != null) {
                    animator.SetTrigger("Action");
                }
            }
            i--;
            yield return new WaitForSeconds(0.25f);
        }
    }

    public void End()
    {
        List<AControlable> controlableList = new List<AControlable>();

        foreach (var controlable in _refMaison._controlables) {
            // controlableList.Add(controlable);
            Animator animator = controlable.GetComponentInChildren<Animator>();
            if (animator != null) {
                animator.SetBool("Walk", true);
            }
        }
        foreach (var maison in _maisons) {
            foreach (var controlable in maison._controlables) {
                controlable.isLocked = false;
                controlable.isSnapped = false;
            }
        }
        // foreach (var controlable in controlableList) {
        //     Animator animator = controlable.GetComponentInChildren<Animator>();
        //     if (animator != null) {
        //         animator.SetBool("Walk", true);
        //     }
        //     // controlable.isLocked = false;
        // }
    }
}
