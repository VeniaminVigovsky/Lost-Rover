using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudioManager : MonoBehaviour
{
    [SerializeField]
    AudioClip UISound;
    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = UISound;
    }
    public void PlayUISound()
    {
        audioSource.Play();
    }
}
