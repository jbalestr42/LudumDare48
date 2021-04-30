using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DisableOnInputMouse : MonoBehaviour {
    [SerializeField] private InputAction input;
    [SerializeField] private TextMeshProUGUI textMesh;

    private Transform cameraTransform;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        input.performed += Disable;
        input.Enable();
    }

    private void Disable(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity)) {
            AControlable controlable = hit.collider.gameObject.GetComponentInParent<AControlable>();
            if (controlable != null) {
                // string[] split = textMesh.text.Split(' ');
                // if (split.Length > 1) {
                //     textMesh.text = textMesh.text.Split(' ')[0];
                // } else {
                //     textMesh.text = "";
                // }
                if (textMesh.text.Length > 0) {
                    textMesh.text = textMesh.text.Remove(textMesh.text.Length - 1);
                }
            }
        }
    }
}
