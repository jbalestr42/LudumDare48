using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetAnimationFrame : MonoBehaviour {
    [SerializeField] private Animator animator;

    // [ExecuteInEditMode]
    // void Update()
    // {
    //     if (animator == null) {
    //         animator = GetComponentInChildren<Animator>();
    //     }
    //     // setAnimationFrame(name, 0, 0.1f);
    // }

    // private void setAnimationFrame(string animationName, int layer, float normalizedAnimationTime)
    // {
    //     if (animator != null) {
    //         animator.speed = 0f;
    //         animator.Play("Idle", layer, normalizedAnimationTime);
    //         animator.Update(Time.deltaTime);
    //     }
    // }
}
