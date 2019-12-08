using UnityEngine;
using System.Collections;

public class SongManager : MonoBehaviour
{
    public AudioClip regularAudioClip;
    public AudioClip bossAudioClip;

    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        if ((regularAudioClip == null) || (bossAudioClip == null))
        {
            string error = "";
            if (regularAudioClip == null)
                error = "The Regular Audio Clip has not been initialized in the inspector, please do this now. ";
            if (bossAudioClip == null)
                error += "The High Damage Audio Clip has not been initialized in the inspector, please do this now. ";
            Debug.LogError(error);
        }

        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource component missing from this gameobject. Adding one.");
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        PlayRegularAudioClip();
    }

    public void PlayRegularAudioClip()
    {
        audioSource.Stop();
        audioSource.loop = true;
        audioSource.PlayOneShot(regularAudioClip);
    }

    public void PlayBossAudioClip()
    {
        audioSource.Stop();
        audioSource.loop = true;
        audioSource.PlayOneShot(bossAudioClip);
    }

    // Update is called once per frame
    void Update()
    {
        //if (DamageManager.dmg_chain > 1500)
        //{
        //    PlaybossAudioClip();
        //}
        //else
        //{
        //    if (high_dmg_music.isPlaying)
        //    {
        //        PlayRegularAudioClip();
        //    }
        //}
    }
}