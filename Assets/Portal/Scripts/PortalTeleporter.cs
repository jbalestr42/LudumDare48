using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour {
    public Transform player;
    public Transform reciever;

    private Transform overlappingPlayer;
    private PortalTeleporter[] portalTeleporterList;
    private PlayerScalePortal playerScalePortal;

    private void Start()
    {
        portalTeleporterList = FindObjectsOfType<PortalTeleporter>();
        playerScalePortal = player.GetComponent<PlayerScalePortal>();
    }

    void LateUpdate()
    {
        Vector3 portalPosition = transform.position;
        portalPosition.y = 0f;
        Vector3 playerPosition = player.position;
        playerPosition.y = 0f;
        float distance = Vector3.Distance(portalPosition, playerPosition);
        if (distance < 1f) {
            Vector3 positionOffset = player.position - transform.position;
            float dotProduct = Vector3.Dot(transform.up, positionOffset);

            // If this is true: The player has moved across the portal
            if (dotProduct < 0f) {
                // Teleport him!
                // float rotationDiff = -Quaternion.Angle(transform.rotation, reciever.rotation);
                // rotationDiff += 180;
                // player.Rotate(Vector3.up, rotationDiff);
                // Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;

                positionOffset *= (reciever.lossyScale.x / transform.lossyScale.x);
                // player.position = reciever.position + positionOffset + reciever.forward;
                Vector3 position = reciever.position + positionOffset;
                // prevent glitch :/
                position.y = player.position.y;
                CharacterController controller = player.GetComponent<CharacterController>();
                controller.enabled = false;
                player.position = position;
                controller.enabled = true;

                playerScalePortal.OnEnterPortal();
            }
        }
    }
}
