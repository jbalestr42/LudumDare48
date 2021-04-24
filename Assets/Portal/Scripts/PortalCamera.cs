using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour {

    public Material material;
    public Transform playerCamera;
    public Transform portal;
    public Transform otherPortal;
    public bool isRevert = true;

    private void Start()
    {
        Camera cam = GetComponent<Camera>();
        if (cam.targetTexture != null) {
            cam.targetTexture.Release();
        }
        cam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        material.mainTexture = cam.targetTexture;
    }

    void Update()
    {
        Vector3 playerOffsetFromPortal = playerCamera.position - otherPortal.position;
        playerOffsetFromPortal *= (portal.lossyScale.x / otherPortal.lossyScale.x);
        transform.position = (portal.position + playerOffsetFromPortal);

        float angularDifferenceBetweenPortalRotations = Quaternion.Angle(portal.rotation, otherPortal.rotation);

        Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
        Vector3 newCameraDirection = portalRotationalDifference * playerCamera.forward;
        transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
        if (isRevert) {
            transform.RotateAround(transform.position, Vector3.up, 180f);
        }
    }
}
