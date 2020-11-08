using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    // Audio Params
    [SerializeField] AudioClip[] explosionClips;
    AudioSource audioSource;
    float startVolume;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(explosionClips[Random.Range(0,4)]);
        startVolume = audioSource.volume;
    }
    
    void Update() 
    {
        if(audioSource.volume > 0)
        {
            audioSource.volume -= Time.deltaTime / 6;
        }
    }
}
