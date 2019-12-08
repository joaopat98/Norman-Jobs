﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedDoor : MonoBehaviour
{
    public GameObject door;
    public bool doorBoss;
    public AudioClip bossAudioClip;
    public AudioSource audioSource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            door.SetActive(true);
        }
        if (doorBoss)
        {
            switchSong();
            doorBoss = false;
        }
    }

    private void switchSong()
    {
        audioSource.Stop();
        audioSource.clip = bossAudioClip;
        audioSource.Play();
    }
}
