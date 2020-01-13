using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedDoor : MonoBehaviour
{
    public GameObject door;
    public bool doorBoss;
    public AudioClip bossAudioClip;
    public AudioSource audioSource;
    public GameObject boss;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            door.SetActive(true);
            this.gameObject.SetActive(false);
        }
        if (doorBoss)
        {
            switchSong();
           
            doorBoss = false;
            boss.GetComponent<Boss>().WakeUp();
        }
    }

    private void switchSong()
    {
        audioSource.Stop();
        audioSource.clip = bossAudioClip;
        audioSource.Play();
    }
}
