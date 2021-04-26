using UnityEngine;

public class Quit : MonoBehaviour {
#if UNITY_WEBGL
#else
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }
#endif
}