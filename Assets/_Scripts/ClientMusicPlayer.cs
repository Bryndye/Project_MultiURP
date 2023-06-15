using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ClientMusicPlayer : Singleton<ClientMusicPlayer>
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip audioClipName;
    public override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudioClipByName()
    {
        audioSource.clip = audioClipName;
        audioSource.Play();
        Debug.Log(audioSource.isPlaying);
    }
}
