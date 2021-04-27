using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleActionable : AActionable {
    [SerializeField] private ParticleSystem particle;

    public override void DoAction()
    {
        particle.Play();
    }
}