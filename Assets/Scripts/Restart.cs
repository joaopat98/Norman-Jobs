using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    // Start is called before the first frame update
   /* void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }*/
    public GameObject boss;
    public GameObject panel;
    public GameObject panelWin;
    public AudioClip winSound;
    public float winSoundVolume;
    private bool justOne;

    void Start()
    {
        winSoundVolume = 0.5f;
        panel.SetActive(false);
        panelWin.SetActive(false);
        justOne = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(boss == null && justOne)
        {

            StartCoroutine(LoseSound());
            justOne = false;
        }

    }

    IEnumerator LoseSound()
    {
        AudioSource.PlayClipAtPoint(winSound, Camera.main.transform.position, winSoundVolume);
        yield return new WaitForSeconds(2.0f);
        panel.SetActive(true);
       
    }

    public void RestartScene()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
