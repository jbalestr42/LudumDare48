using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScalePortal : MonoBehaviour {
    [System.Serializable]
    public class PlayerControllerData {
        public float centerY;
        public float radius;
        public float height;
    }

    [SerializeField] PlayerControllerData playerBase;
    [SerializeField] PlayerControllerData playerMinus;
    [SerializeField] float maxDistance = 10f;
    [SerializeField] private List<PortalTeleporter> portalTeleporterList;
    [SerializeField] private Transform head;

    private CharacterController controller;
    private Camera cam;
    private float lerpValue;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = controller.GetComponent<Character>().Camera;
    }

    private void Update()
    {
        foreach (var teleporter in portalTeleporterList) {
            Vector3 teleporterPosition = teleporter.transform.position;
            float distance = Vector3.Distance(teleporterPosition, transform.position);

            if (distance <= maxDistance) {
                lerpValue = distance / maxDistance;
            } else {
                lerpValue = 1f;//Mathf.Min(1f, lerpValue + Time.deltaTime);
            }
            controller.center = new Vector3(0f, Mathf.Lerp(playerMinus.centerY, playerBase.centerY, lerpValue), 0f);
            controller.radius = Mathf.Lerp(playerMinus.radius, playerBase.radius, lerpValue);
            controller.height = Mathf.Lerp(playerMinus.height, playerBase.height, lerpValue);
            head.localPosition = new Vector3(0f, Mathf.Lerp(playerMinus.height, playerBase.height, lerpValue), 0f);
        }
    }
}
