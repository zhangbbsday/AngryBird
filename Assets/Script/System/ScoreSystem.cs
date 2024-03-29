﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : BaseSystem
{
    public int BestScore { get; private set; }
    public int NowScore { get; private set; }
    public int[] EveryLevelScore { get; private set; }
    public float[] StarPercent { get; } = { 0.3f, 0.6f, 0.9f };

    private int levelNow;
    private Text bestScore;
    private Text nowScore;
    private Text bestScoreClear;
    private Text nowScoreClear;

    public ScoreSystem()
    {
        Initialize();
    }

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
        EveryLevelScore = new int[GameManager.LevelNumber] { 30000, 60000, 70000, 80000, 80000, 60000, 50000, 40000, 40000, 220000 };
    }
}
