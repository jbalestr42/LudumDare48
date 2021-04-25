using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerYPortal : MonoBehaviour {
    [SerializeField] float basePlayerY = 2f;
    [SerializeField] float maxDistance = 10f;
    [SerializeField] private List<PortalTeleporter> portalTeleporterList;

    private void Update()
    {
        foreach (var teleporter in portalTeleporterList) {
            Vector3 teleporterPosition = teleporter.transform.position;
            float distance = Vector3.Distance(teleporterPosition, transform.position);

            if (distance <= maxDistance) {
                Vector3 position = transform.position;
                position.y = Mathf.Lerp(teleporterPosition.y, basePlayerY, distance / maxDistance);
                transform.position = position;
            }
        }
    }
}
