using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerySimpleMovement : MonoBehaviour {
    [SerializeField] private float speed = 10f;

    void Update()
    {
        if (Input.GetKey(KeyCode.W)) {
            transform.Translate(transform.forward * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.S)) {
            transform.Translate(-transform.forward * Time.deltaTime * speed);
        }
    }
}
