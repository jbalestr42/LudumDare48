using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaisonManager : MonoBehaviour {
    public List<AControlable> _controlables = new List<AControlable>();
    public GameObject _door;
    public GameObject _smallDoorCollider;
    public GameObject _nextDoorCollider;
    public AudioSource _doorSound;
    public GameObject _originObject;

    void Start()
    {
        _controlables.AddRange(GetComponentsInChildren<AControlable>());
        // _originObject.SetActive(false);
    }

    public bool CheckObjects(MaisonManager maison)
    {
        foreach (AControlable controlable in _controlables) {
            if (!IsSameState(controlable, maison.GetObject(controlable.objectType)) ||
                !IsValidated(controlable, maison.GetObject(controlable.objectType))) {
                return false;
            }
        }
        return true;
    }

    public bool CheckObjectState(AControlable controlable)
    {
        return IsSameState(GetObject(controlable.objectType), controlable);
    }

    public bool IsSameState(AControlable refControlable, AControlable controlable)
    {
        return refControlable.IsSameState(controlable);
    }

    public bool CheckValidated(AControlable controlable)
    {
        return IsValidated(GetObject(controlable.objectType), controlable);
    }

    public bool IsValidated(AControlable refControlable, AControlable controlable)
    {
        return refControlable.IsValidated(controlable);
    }

    public AControlable GetObject(ObjectType type)
    {
        return _controlables.Find(o => o.objectType == type);
    }

    public void OpenDoor()
    {
        StartCoroutine(OpenDoorCor());
        _smallDoorCollider.SetActive(false);
        _nextDoorCollider.SetActive(false);
    }

    public void ActivateObject()
    {
        _originObject.SetActive(true);
    }

    IEnumerator OpenDoorCor()
    {
        _doorSound.Play();
        while (_door.transform.localRotation.y > -0.60f) {
            _door.transform.RotateAround(_door.transform.position, transform.up, -Time.deltaTime * 20f);
            yield return new WaitForEndOfFrame();
        }
    }
}
