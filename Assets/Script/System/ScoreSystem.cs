using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : BaseSystem
{
    public int BestScore { get; private set; }
    public int NowScore { get; private set; }

    private int levelNow;
    private Text bestScore;
    private Text nowScore;
    private Text bestScoreClear;
    private Text nowScoreClear;


    public override void Release()
    {
        IsRuning = false;
    }

    public override void Update()
    {
        if (!IsRuning)
            return;
    }

    public void SetScore(int level, Text best, Text now, Text bestClear, Text nowClear)
    {
        levelNow = level;
        bestScore = best;
        nowScore = now;
        bestScoreClear = bestClear;
        nowScoreClear = nowClear;

        BestScore = PlayerPrefs.GetInt("Level" + levelNow.ToString(), 0);
        NowScore = 0;
        nowScore.text = NowScore.ToString();
        bestScore.text = BestScore.ToString();
    }

    public void GetScore(int score)
    {
        NowScore += score;
        if (NowScore > BestScore)
            BestScore = NowScore;

        nowScore.text = NowScore.ToString();
        bestScore.text = BestScore.ToString();
        nowScoreClear.text = NowScore.ToString();
        bestScoreClear.text = BestScore.ToString();
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt("Level" + levelNow, BestScore);
    }

    protected override void Initialize()
    {
        IsRuning = false;
    }
}
