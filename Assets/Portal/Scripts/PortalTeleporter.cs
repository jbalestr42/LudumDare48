using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour {
    public Transform player;
    public Transform reciever;

    private bool playerIsOverlapping = false;
    // public float imunePortal = 0f;

    void Update()
    {
        // if (imunePortal >= 0f) {
        //     imunePortal -= Time.deltaTime;
        //     return;
        // }
        if (playerIsOverlapping) {
            Vector3 portalToPlayer = player.position - transform.position;
            float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

            // If this is true: The player has moved across the portal
            if (dotProduct < 0f) {
                // Teleport him!
                // float rotationDiff = -Quaternion.Angle(transform.rotation, reciever.rotation);
                // rotationDiff += 180;
                // player.Rotate(Vector3.up, rotationDiff);

                // Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
                Vector3 positionOffset = portalToPlayer;
                positionOffset *= (reciever.lossyScale.x / transform.lossyScale.x);
                player.position = reciever.position + positionOffset;

                playerIsOverlapping = false;

                // Cheap trick to prevent a bug...
                // Avoid taking same portal during 1.5 seconds
                // imunePortal = 1.5f;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            playerIsOverlapping = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") {
            playerIsOverlapping = false;
        }
    }
}
