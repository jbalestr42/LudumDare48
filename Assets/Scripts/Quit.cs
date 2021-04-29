using UnityEngine;
using UnityEngine.InputSystem;

public class Quit : MonoBehaviour {
#if UNITY_WEBGL
#else
    [SerializeField] private InputAction inputAction;

    private void Start()
    {
        inputAction.performed += OnQuit;
        inputAction.Enable();
    }

    private void OnQuit(InputAction.CallbackContext context)
    {
        Application.Quit();
        Debug.Log("Quit");
    }
#endif
}