using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }

    [SerializeField] AudioClip[] audioClips;
    [SerializeField] AudioSource audioSource;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
        {
            instance = this;
        }
    }

    public AudioClip GetSound(int id)
    {
        if (id < audioClips.Length)
            return audioClips[id];
        else return null;
    }

    public void PlaySound(int id)
    {
        if (id < audioClips.Length)
            audioSource.PlayOneShot(audioClips[id]);
    }
}
