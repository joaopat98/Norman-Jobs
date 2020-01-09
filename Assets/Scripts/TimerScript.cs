using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{

    public Text timerText;

    private float startTime;

    private float t;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        t = Time.time - startTime;

        float hours = GetHours();

        float minutes = GetMinutes();

        float seconds = GetSeconds();

        timerText.text = hours.ToString() + ":" + minutes.ToString() + ":" + seconds.ToString("f0");

        player.GetComponent<ScoreScriptPlayer>().GetTimeScorePenalty(TimePenalty());
    }

    
    public float GetHours()
    {
        return (((int)t / 60) / 60);
    }

    public float GetMinutes()
    {
        return ((int)t / 60);
    }

    public float GetSeconds()
    {
        return ((int)t % 60);
    }

    public int TimePenalty()
    {
        //blocks of 30 seconds
        int blocks = ((int)t % 60) / 30;

        return -30 * blocks;
    }
}
