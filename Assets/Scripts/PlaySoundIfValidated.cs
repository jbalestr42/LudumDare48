using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundIfValidated : MonoBehaviour {
    [SerializeField] AControlable controlable;
    [SerializeField] AudioSource audioSource;
    [SerializeField] bool needSnap = false;

    private void Update()
    {
        if (controlable.isSnapped) {
            foreach (var reactionable in controlable.reactionableList) {
                if (!reactionable.isValidated) {
                    return;
                }
            }
            if (!audioSource.isPlaying) {
                Debug.Log("Play sound " + audioSource.clip.name);
                audioSource.Play();
            }
        }
    }
}
