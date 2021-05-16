using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {
    public static bool IsTouch;
    public static Vector2 TapPosition;
    [SerializeField] bool isTouch = false;

    public static Vector2 mouse;
    public static Vector2 movement;

    private static InputActionAsset inputActionAsset;
    private static InputActionMap playerActionMap;

    private static System.Action<InputAction.CallbackContext> inputAction;

    private UnityEngine.InputSystem.Controls.TouchControl moveControl = null;
    private UnityEngine.InputSystem.Controls.TouchControl mouseControl = null;

    private void Update()
    {
        if (isTouch) {
            UpdateTouch();
        } else {
            UpdateKeyboardMouse();
        }
    }

    private void LateUpdate()
    {
        InputCheck();
        IsTouch = isTouch;
    }

    private void InputCheck()
    {
        if (isTouch) {
            if (Keyboard.current.anyKey.wasPressedThisFrame) {
                isTouch = false;
            }
        } else {
            var touchArray = Touchscreen.current.touches;
            foreach (var touch in touchArray) {
                if (touch.isInProgress) {
                    isTouch = true;
                }
            }
        }
        mouse = Vector2.zero;
        movement = Vector2.zero;
        moveControl = null;
        mouseControl = null;
    }

    private void UpdateTouch()
    {
        var touchArray = Touchscreen.current.touches;
        foreach (var touch in touchArray) {
            if (touch.isInProgress) {
                Vector2 position = touch.position.ReadValue();
                if (position.x > Screen.width * 0.2f) {
                    mouse = touch.delta.ReadValue();
                    mouseControl = touch;
                }
                if (position.x < Screen.width * 0.2f) {
                    movement.y = 1f;
                    moveControl = touch;
                }
            } else {
                if (mouseControl == touch) {
                    mouse = Vector2.zero;
                    mouseControl = null;
                }
                if (moveControl == touch) {
                    movement.y = 0f;
                    moveControl = null;
                }
            }
            if (touch.tapCount.ReadValue() != 0) {
                if (inputAction != null) {
                    TapPosition = touch.position.ReadValue();
                    inputAction.Invoke(new InputAction.CallbackContext());
                    Debug.Log("Click");
                }
            }
        }
    }

    private void UpdateKeyboardMouse()
    {
        mouse = Mouse.current.delta.ReadValue();
        movement.y = Keyboard.current.wKey.ReadValue() - Keyboard.current.sKey.ReadValue();
        movement.x = Keyboard.current.dKey.ReadValue() - Keyboard.current.aKey.ReadValue();
        if (Mouse.current.leftButton.wasPressedThisFrame) {
            if (inputAction != null) {
                inputAction.Invoke(new InputAction.CallbackContext());
            }
        }
    }

    public static void RegisterCallback(string action, System.Action<InputAction.CallbackContext> callback, bool remove)
    {
        if (remove) {
            inputAction -= callback;
        } else {
            inputAction += callback;
        }
    }
}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;

// public class InputManager : MonoBehaviour {
//     public static Vector2 mouse;
//     public static Vector2 movement;

//     private static InputActionAsset inputActionAsset;
//     private static InputActionMap playerActionMap;

//     private static void Init()
//     {
//         inputActionAsset = Resources.Load("Inputs") as InputActionAsset;
//         playerActionMap = inputActionAsset.FindActionMap("Player");
//         if (playerActionMap == null) {
//             return;
//         }
//         // #if ANDROID
//         // var move = inputActionAsset.FindAction("Move");
//         // move.performed += OnMovePerformed;
//         // move.canceled += OnMoveCanceled;
//         // move.Enable();
//         // #else
//         var movement = inputActionAsset.FindAction("Movement");
//         movement.performed += OnMovementChanged;
//         movement.canceled += OnMovementChanged;
//         movement.Enable();
//         // #endif

//         var mouse = inputActionAsset.FindAction("Mouse");
//         mouse.performed += OnMouseChanged;
//         mouse.canceled += OnMouseChanged;
//         mouse.Enable();

//         var objectAction = playerActionMap.FindAction("Object");
//         objectAction.Enable();

//         var actionAction = playerActionMap.FindAction("Object");
//         actionAction.Enable();
//     }

//     private struct SavedCallback {
//         public bool remove;
//         public System.Action<InputAction.CallbackContext> callback;
//         public string action;
//     }

//     private static List<SavedCallback> callbackList = new List<SavedCallback>();

//     private void Update()
//     {
//         if (playerActionMap == null) {
//             Init();
//             foreach (var callback in callbackList) {
//                 RegisterCallback(callback.action, callback.callback, callback.remove);
//             }
//             callbackList.Clear();
//         }
//     }

//     public static void RegisterCallback(string action, System.Action<InputAction.CallbackContext> callback, bool remove)
//     {
//         if (playerActionMap == null) {
//             var savedCallback = new SavedCallback();
//             savedCallback.action = action;
//             savedCallback.callback = callback;
//             savedCallback.remove = remove;
//             callbackList.Add(savedCallback);
//             return;
//         }
//         var objectAction = playerActionMap.FindAction(action);
//         if (remove) {
//             objectAction.performed -= callback;
//         } else {
//             objectAction.performed += callback;
//         }
//     }

//     private static void OnMovementChanged(InputAction.CallbackContext context)
//     {
//         movement = context.ReadValue<Vector2>();
//     }

//     private static void OnMovePerformed(InputAction.CallbackContext context)
//     {
//         if (Mouse.current.position.ReadValue().x < Screen.width * 0.2f) {
//             movement = new Vector2(0f, 100f);
//         }
//     }

//     private static void OnMoveCanceled(InputAction.CallbackContext context)
//     {
//         movement = Vector2.zero;
//     }

//     private static void OnMouseChanged(InputAction.CallbackContext context)
//     {
//         // if (Mouse.current.position.ReadValue().x > Screen.width * 0.2f) {
//         //     mouse = context.ReadValue<Vector2>();
//         // } else {
//         //     mouse = Vector2.zero;
//         // }
//         mouse = context.ReadValue<Vector2>();
//     }
// }