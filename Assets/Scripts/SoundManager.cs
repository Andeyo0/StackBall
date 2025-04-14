using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;
    private AudioSource audioSource;
    public bool sound;

    private void Awake()
    {
        makeSingleton();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        
    }

    private void makeSingleton()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void Update()
    {
        
    }

    public void soundOnOff()
    {
        sound = !sound;
    }

    public void playSoundFX(AudioClip clip, float volume)
    {
        if (sound)
        {
            audioSource.PlayOneShot(clip, volume);
        }
    }
}
