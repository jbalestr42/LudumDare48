using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScalePortal : MonoBehaviour {
    [System.Serializable]
    public class PlayerControllerData {
        public float centerY;
        public float radius;
        public float height;
        public float stepOffset;
    }

    [SerializeField] PlayerControllerData playerBase;
    [SerializeField] PlayerControllerData playerMinus;
    [SerializeField] float maxDistance = 10f;
    [SerializeField] private List<PortalTeleporter> portalTeleporterList;
    [SerializeField] private Transform head;
    [SerializeField] private Transform cameraRig;
    [SerializeField] private AnimationCurve curve;

    private CharacterController controller;
    private Camera cam;
    private float lerpValue;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = controller.GetComponent<Character>().Camera;
    }

    private void LateUpdate()
    {
        float distance = 0f;
        bool isClosePortal = false;

        foreach (var teleporter in portalTeleporterList) {
            Vector3 teleporterPosition = teleporter.transform.position;
            distance = Vector3.Distance(teleporterPosition, transform.position);
            if (distance <= maxDistance) {
                isClosePortal = true;
                break;
            }

        }
        if (isClosePortal) {

            float lerpValueTarget = curve.Evaluate(Mathf.Clamp(distance / maxDistance, 0f, 1f));// Mathf.Clamp((distance - maxDistance * 0.5f) / maxDistance, 0f, 1f);
            if (lerpValue < lerpValueTarget) {
                lerpValue = Mathf.Max(lerpValue - Time.deltaTime * 0.2f, lerpValueTarget);
            } else {
                lerpValue = Mathf.Min(lerpValue + Time.deltaTime * 0.2f, lerpValueTarget);
            }
            // lerpValue = Mathf.Clamp((distance - maxDistance * 0.3f) / maxDistance, 0f, 1f);
            controller.center = new Vector3(0f, Mathf.Lerp(playerMinus.centerY, playerBase.centerY, lerpValue), 0f);
            controller.radius = Mathf.Lerp(playerMinus.radius, playerBase.radius, lerpValue);
            controller.height = Mathf.Lerp(playerMinus.height, playerBase.height, lerpValue);
            controller.stepOffset = Mathf.Lerp(playerMinus.stepOffset, playerBase.stepOffset, lerpValue);
            head.localPosition = new Vector3(0f, Mathf.Lerp(playerMinus.height, playerBase.height, lerpValue), 0f);
        }// } else if (lerpValue < 1f) {
        //     lerpValue = 1f;//Mathf.Min(1f, lerpValue + Time.deltaTime);
        //     controller.center = new Vector3(0f, Mathf.Lerp(playerMinus.centerY, playerBase.centerY, lerpValue), 0f);
        //     controller.radius = Mathf.Lerp(playerMinus.radius, playerBase.radius, lerpValue);
        //     controller.height = Mathf.Lerp(playerMinus.height, playerBase.height, lerpValue);
        //     controller.stepOffset = Mathf.Lerp(playerMinus.stepOffset, playerBase.stepOffset, lerpValue);
        //     head.localPosition = new Vector3(0f, Mathf.Lerp(playerMinus.height, playerBase.height, lerpValue), 0f);
        // }
    }

    public void OnEnterPortal()
    {
        lerpValue = 1f;//Mathf.Min(1f, lerpValue + Time.deltaTime);
        controller.center = new Vector3(0f, Mathf.Lerp(playerMinus.centerY, playerBase.centerY, lerpValue), 0f);
        controller.radius = Mathf.Lerp(playerMinus.radius, playerBase.radius, lerpValue);
        controller.height = Mathf.Lerp(playerMinus.height, playerBase.height, lerpValue);
        controller.stepOffset = Mathf.Lerp(playerMinus.stepOffset, playerBase.stepOffset, lerpValue);
        head.localPosition = new Vector3(0f, Mathf.Lerp(playerMinus.height, playerBase.height, lerpValue), 0f);
        cameraRig.transform.position = head.transform.position;
    }
}
