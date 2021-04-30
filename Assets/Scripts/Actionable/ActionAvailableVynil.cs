using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionAvailableVynil : AActionAvailable {
    [SerializeField] private Transform vynil;
    [SerializeField] private AnimationCurve curve;

    private bool isEnable;

    public override void Enable()
    {
        isEnable = true;
    }
    public override void Disable()
    {
        isEnable = false;
    }

    public override void DoAction() { }

    private void Start()
    {
        StartCoroutine(VynilCoroutine());
    }

    private IEnumerator VynilCoroutine()
    {
        float time = 0f;
        Vector3 originLocalPosition = vynil.localPosition;

        while (true) {
            if (isEnable) {
                time += Time.deltaTime;
                if (time > 1f) {
                    time = 0f;
                }
                vynil.localPosition = originLocalPosition - Vector3.forward * curve.Evaluate(time);
                yield return null;
            } else {
                time += Time.deltaTime;
                if (time < 1f) {
                    vynil.localPosition = originLocalPosition - Vector3.forward * curve.Evaluate(time);
                }
                yield return null;
            }
        }
    }
}