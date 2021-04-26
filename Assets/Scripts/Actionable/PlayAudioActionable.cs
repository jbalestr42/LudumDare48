using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioActionable : AActionable {
    [SerializeField] private AudioSource audioSource;

    private void OnEnable()
    {
        isReaction = true;
        if (isValidated) {
            DoAction();
        }
    }

    public override void DoAction()
    {
        audioSource.Play();
    }
}