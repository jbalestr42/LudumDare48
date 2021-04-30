using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DisableOnInput : MonoBehaviour {
    [SerializeField] private InputAction input;
    [SerializeField] private GameObject go;

    private void Start()
    {
        input.performed += Disable;
        input.Enable();
    }

    private void Disable(InputAction.CallbackContext context)
    {
        gameObject.SetActive(false);
        go.SetActive(false);
    }
}
