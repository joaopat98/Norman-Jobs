using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSong : MonoBehaviour
{
    public AudioClip bossAudioClip;
    public AudioSource audioSource;

    public void switchSong()
    {
        audioSource.clip = bossAudioClip;
    }
}
