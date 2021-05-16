using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisableIfTouch : MonoBehaviour {
    [SerializeField] private bool setEnable = false;
    void Update()
    {
        bool doSetEnable = false;
        if (InputManager.IsTouch && setEnable) {
            doSetEnable = true;
        }
        if (!InputManager.IsTouch && !setEnable) {
            doSetEnable = true;
        }
        foreach (Transform child in transform) {
            child.gameObject.SetActive(doSetEnable);
        }
    }
}
