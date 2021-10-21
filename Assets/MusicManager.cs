using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    AudioClip victoryMusic;

    [SerializeField]
    AudioClip radarSound, UIclickSound;

    AudioSource radarAS, musicAS, UIsoundsAS;

    [SerializeField]
    AudioMixerGroup roverMixerGroup;

    private void Start()
    {
        radarAS = gameObject.AddComponent<AudioSource>();        
        musicAS = gameObject.AddComponent<AudioSource>();
        UIsoundsAS = gameObject.AddComponent<AudioSource>();
        foreach (var audioSource in GetComponents<AudioSource>())
        {
            audioSource.loop = false;
            audioSource.playOnAwake = false;
        }
        radarAS.outputAudioMixerGroup = roverMixerGroup ?? null;
        musicAS.volume = 0.5f;
        
    }

    public void PlayVictoryMusic()
    {
        musicAS.loop = false;
        musicAS.clip = victoryMusic;
        musicAS.Play();
    }

    public void PlayRadarSound()
    {
        radarAS.PlayOneShot(radarSound);
    }

    public void PlayUIClickSound()
    {
        
        UIsoundsAS.PlayOneShot(UIclickSound);
    }

    

}
