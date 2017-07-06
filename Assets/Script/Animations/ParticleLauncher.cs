using UnityEngine;
using System.Collections;

public class ParticleLauncher : MonoBehaviour
{

    public ParticleEmitter[] emitters;

    void Start()
    {
        AnimationEvent ae = new AnimationEvent(); ae.messageOptions = SendMessageOptions.DontRequireReceiver;
    }

    public void fireEverything()
    {
        foreach (ParticleEmitter emitter in emitters)
        {
            emitter.Emit();
        }
    }
}