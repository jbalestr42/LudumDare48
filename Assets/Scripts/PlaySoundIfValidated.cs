using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundIfValidated : MonoBehaviour {
    [SerializeField] AControlable controlable;
    [SerializeField] AudioSource audioSource;
    [SerializeField] bool needSnap = false;

    private float originVolume;

    private void Start()
    {
        audioSource.Play();
        originVolume = audioSource.volume;
    }

    private void Update()
    {
        bool isOn = true;
        if (controlable.isSnapped) {
            foreach (var reactionable in controlable.reactionableList) {
                if (!reactionable.isValidated) {
                    isOn = false;
                }
            }
        }
        if (isOn) {
            audioSource.volume = Mathf.Min(originVolume, audioSource.volume + Time.deltaTime * 10f);
        } else {
            audioSource.volume = Mathf.Max(0f, audioSource.volume - Time.deltaTime * 10f);
        }
    }
}
