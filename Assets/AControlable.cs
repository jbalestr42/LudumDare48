using UnityEngine;

public class AControlable : MonoBehaviour
{
    // List de on action ?
    Transform _cameraPlaceHolder;

    public void DoAction()
    {
        Debug.Log("Action !!");
    }

    public Vector3 GetCameraPosition()
    {
        return _cameraPlaceHolder.position;
    }

    public Vector3 GetCameraDirection()
    {
        return (_cameraPlaceHolder.position - this.transform.position).normalized;
    }
}