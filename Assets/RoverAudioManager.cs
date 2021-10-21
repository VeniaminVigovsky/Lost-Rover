using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class RoverAudioManager : MonoBehaviour
{
    AudioSource roverEngineAS, energyBlockAS;

    [SerializeField]
    AudioClip engineIdle, engineStart, engineMove, engineIdleToMove, engineMoveToIdle, roverFall, runOutOfPower, refillSound;
    [SerializeField]
    AudioClip engineMoveToStop;

    [SerializeField]
    AudioMixerGroup energyMixingGroup;
   

    // Start is called before the first frame update
    void Start()
    {
        roverEngineAS = GetComponent<AudioSource>();
        roverEngineAS.loop = true;
        energyBlockAS = gameObject.AddComponent<AudioSource>();
        energyBlockAS.playOnAwake = false;
        energyBlockAS.loop = false;
        energyBlockAS.outputAudioMixerGroup = energyMixingGroup ?? null;
        PlaySoundToSound(engineStart, engineIdle, roverEngineAS);
    }

    private void PlaySoundToSound(AudioClip soundBegin, AudioClip soundTransitionTo, AudioSource a_source)
    {
        if (soundBegin == null || soundTransitionTo == null) return;
        double l = (double)soundBegin.samples / soundBegin.frequency;
        double t = AudioSettings.dspTime;
        a_source.Stop();
        a_source.PlayOneShot(soundBegin);
        a_source.clip = soundTransitionTo;
        a_source.PlayScheduled(t + l);
        
    }

    public void StartMoving()
    {
        PlaySoundToSound(engineIdleToMove, engineMove, roverEngineAS);
    }
    public void StopMoving()
    {
        PlaySoundToSound(engineMoveToIdle, engineIdle, roverEngineAS);
    }

    public void EnergyDrained()
    {
        roverEngineAS.clip = runOutOfPower;
        roverEngineAS.loop = false;
        roverEngineAS.Play();
    }

    public void RoverFallen()
    {
        roverEngineAS.clip = roverFall;
        roverEngineAS.loop = false;
        roverEngineAS.Play();
    }

    public void PlayRefillEnergySound()
    {
        if (refillSound == null) return;
        energyBlockAS.PlayOneShot(refillSound);
    }

    public void RoverFullStop()
    {
        roverEngineAS.clip = engineMoveToStop;
        roverEngineAS.loop = false;
        roverEngineAS.Play();
    }
}
