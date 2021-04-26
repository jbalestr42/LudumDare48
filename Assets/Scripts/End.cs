using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour {
    [SerializeField] private GameObject title;
    [SerializeField] private InceptionManager inceptionManager;

    private void Start()
    {
        title.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            title.SetActive(true);
            inceptionManager.End();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            title.SetActive(true);
            inceptionManager.End();
        }
    }
}
