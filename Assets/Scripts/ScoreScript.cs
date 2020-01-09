using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    public GameObject player;

    private Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<Text>();
        scoreText.text = "0";
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = player.GetComponent<ScoreScriptPlayer>().GetScore();
    }
}
