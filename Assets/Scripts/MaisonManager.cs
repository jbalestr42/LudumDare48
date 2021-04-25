using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaisonManager : MonoBehaviour
{
    List<AControlable> _controlables = new List<AControlable>();
    public GameObject _door;
    public GameObject _smallDoorCollider;
    public GameObject _nextDoorCollider;
    public AudioSource _doorSound;

    void Start()
    {
        _controlables.AddRange(GetComponentsInChildren<AControlable>());
    }

    public bool CheckObjects(MaisonManager maison)
    {
        foreach (AControlable controlable in _controlables)
        {
            if (!IsSameState(controlable, maison.GetObject(controlable.objectType)))
            {
                return false;
            }
        }
        return true;
    }

    public bool CheckObject(AControlable controlable)
    {
        return IsSameState(GetObject(controlable.objectType), controlable);
    }

    public bool IsSameState(AControlable refControlable, AControlable controlable)
    {
        return refControlable.IsSameState(controlable);
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

    IEnumerator OpenDoorCor()
    {
        _doorSound.Play();
        while (_door.transform.localRotation.y > -0.60f)
        {
            _door.transform.RotateAround(_door.transform.position, transform.up, -Time.deltaTime * 20f);
            yield return new WaitForEndOfFrame();
        }
    }
}
