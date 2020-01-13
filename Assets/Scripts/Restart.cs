using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public GameObject boss;
    public GameObject panelLose;
    public GameObject panelWin;
    public AudioClip winSound;
    public AudioClip regularSound;
    public float winSoundVolume;
    private bool justOne;
    public float startSoundVolume = 1f;

    void Start()
    {
        winSoundVolume = 0.5f;
        panelLose.SetActive(false);
        panelWin.SetActive(false);
        justOne = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(boss == null && justOne)
        {
            Debug.Log("ENTRE");
            StartCoroutine(WinSound());
            justOne = false;
        }

    }

    IEnumerator WinSound()
    {
        AudioSource.PlayClipAtPoint(winSound, Camera.main.transform.position, winSoundVolume);
        yield return new WaitForSeconds(2.0f);
        panelWin.SetActive(true);
       
    }

    public void RestartScene()
    {
        StartCoroutine(PlaySoundAndLoadScene(SceneManager.GetActiveScene().buildIndex));

    }

    IEnumerator PlaySoundAndLoadScene(int level)
    {

        AudioSource.PlayClipAtPoint(regularSound, Camera.main.transform.position, startSoundVolume);
        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene(level);

    }
}
