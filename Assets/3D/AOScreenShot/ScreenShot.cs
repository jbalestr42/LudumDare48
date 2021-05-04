using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShot : MonoBehaviour {
    private void Start()
    {
        Debug.Log(Application.dataPath + "/AO");
        ScreenCapture.CaptureScreenshot(Application.dataPath + "/AO.jpeg");
    }
}
