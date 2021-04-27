using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager instance = null;
    [SerializeField] private List<AudioClip> audioClipList;
    [SerializeField] private AudioSource audioSource;


    private void Awake()
    {
        instance = this;
    }

    public static void PlaySound(string name, Vector3 position)
    {
        //Debug.Log("Play sound " + name);
        bool fouded = false;
        foreach (var clip in instance.audioClipList) {
            if (clip.name.Contains(name)) {
                instance.audioSource.PlayOneShot(clip);
                instance.transform.position = position;
                fouded = true;
            }
        }
        if (!fouded) {
            Debug.LogWarning("No sound named " + name + "founded.", instance);
        }
    }
}