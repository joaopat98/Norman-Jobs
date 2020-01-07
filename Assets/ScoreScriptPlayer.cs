using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScriptPlayer : MonoBehaviour
{
    private int Score;
    private int hpScore;
    private int punchScore;
    private int timeScorePenalty;

    // Start is called before the first frame update
    void Start()
    {
        Score = 0;
        hpScore = 0;
        punchScore = 0;
        timeScorePenalty = 0;
    }

    // Update is called once per frame
    void Update()
    {
        int scorePreview = hpScore + punchScore + timeScorePenalty;

        if(scorePreview < 0)
        {
            Score = 0;
        }

        else
        {
            Score = scorePreview;
        }
    }

    public void Damage()
    {
        punchScore += 10;
    }

    public void FinalDamage()
    {
        punchScore += 50;
    }

    public void GetHPScore(int hp)
    {
        hpScore = 100 * hp;
    }

    public void GetTimeScorePenalty(int TimePenalty)
    {
        timeScorePenalty = TimePenalty;
    }

    public string GetScore()
    {
        return Score.ToString();
    }

}
