using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionAvailableParticle : AActionAvailable {
    [SerializeField] private ParticleSystem particle;

    public override void Enable()
    {
        var emission = particle.emission;
        emission.enabled = true;
        if (!particle.isPlaying) {
            particle.Play();
        }
    }
    public override void Disable()
    {
        var emission = particle.emission;
        emission.enabled = false;
    }

    public override void DoAction() { }
}