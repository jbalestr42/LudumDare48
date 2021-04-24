using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenameAccordingToParent : MonoBehaviour {
    [SerializeField] string baseName;

    private void OnValidate()
    {
        this.name = baseName + "_" + transform.root.name;
    }
}
