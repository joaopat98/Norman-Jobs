using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public float delayInSeconds = 2f;
    public AudioClip startSound;
    public float waitSound = 0.6f;
    public  float startSoundVolume = 1f;
    public void LoadStartMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadGame(int level)
    {
       
        StartCoroutine(PlaySoundAndStartGame(level));

       
    }
    // Update is called once per frame

    IEnumerator PlaySoundAndStartGame(int level)
    {

        AudioSource.PlayClipAtPoint(startSound, Camera.main.transform.position, startSoundVolume);
        yield return new WaitForSeconds(waitSound);
       
        SceneManager.LoadScene(level);

    }
}
