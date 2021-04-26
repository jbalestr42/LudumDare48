using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioActionable : AActionable {
    private static float playbackTime = 0f;
    private static bool isSetup = false;

    private bool hasOwnership = false;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        if (!isSetup) {
            isSetup = true;
            hasOwnership = true;
        }
    }

    private void OnEnable()
    {
        isReaction = true;
        if (isValidated) {
            DoAction();
        }
    }

    public override void DoAction()
    {
        audioSource.time = playbackTime;
        Debug.Log("Play with " + playbackTime);
        audioSource.Play();
    }

    private void Update()
    {
        if (hasOwnership) {
            playbackTime = audioSource.time;
        }
    }
}