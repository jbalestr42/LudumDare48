using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioActionable : AReactionable {
    private static float playbackTime = 0f;
    private static bool isSetup = false;

    private bool hasOwnership = false;
    [SerializeField] private AudioSource audioSource;

    public override void Init()
    {
        if (!isSetup) {
            isSetup = true;
            hasOwnership = true;
        }
        base.Init();
    }

    public override void DoAction()
    {
        base.DoAction();
        audioSource.time = playbackTime;
        audioSource.Play();
    }

    private void Update()
    {
        if (hasOwnership) {
            playbackTime = audioSource.time;
        }
    }
}