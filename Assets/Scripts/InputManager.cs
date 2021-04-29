using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {
    public static Vector2 mouse;
    public static Vector2 movement;

    private static InputActionAsset inputActionAsset;
    private static InputActionMap playerActionMap;

    private void Awake()
    {
        inputActionAsset = Resources.Load("Inputs") as InputActionAsset;
        playerActionMap = inputActionAsset.FindActionMap("Player");

        var movement = inputActionAsset.FindAction("Movement");
        movement.performed += OnMovementChanged;
        movement.canceled += OnMovementChanged;
        movement.Enable();

        var mouse = inputActionAsset.FindAction("Mouse");
        mouse.performed += OnMouseChanged;
        mouse.canceled += OnMouseChanged;
        mouse.Enable();

        var objectAction = playerActionMap.FindAction("Object");
        objectAction.Enable();

        var actionAction = playerActionMap.FindAction("Action");
        actionAction.Enable();
    }

    public static void RegisterCallback(string action, System.Action<InputAction.CallbackContext> callback, bool remove)
    {
        var objectAction = playerActionMap.FindAction(action);
        if (remove) {
            objectAction.performed -= callback;
        } else {
            objectAction.performed += callback;
        }
    }

    private void OnMovementChanged(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    private void OnMouseChanged(InputAction.CallbackContext context)
    {
        mouse = context.ReadValue<Vector2>();
    }
}
