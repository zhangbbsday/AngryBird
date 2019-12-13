using System;
using UnityEngine;

public class JudgeSystem : BaseSystem
{
    public enum JudgeStateType
    {
        Run,
        Clear,
        Fail
    }

    public JudgeStateType JudgeState { get; private set; }
    public event Action Judge;
    public bool IsJudged { get; private set; }
    private Transform pigs;

    public JudgeSystem()
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

        if (!IsJudged)
            JudgeUpdata();

    }

    protected override void Initialize()
    {
        IsRuning = false;
    }

    public void SetJudge(Transform pigsObject, Action action)
    {
        Judge = null;
        pigs = pigsObject;
        Judge += action;
        IsJudged = false;

        JudgeState = JudgeStateType.Run;
    }

    private void JudgeUpdata()
    {
        if (!(pigs.childCount == 0 || GameManager.Instance.BirdControlSystemControl.IsOver))
            return;

        if (pigs.childCount == 0)
            JudgeState = JudgeStateType.Clear;
        else if (GameManager.Instance.BirdControlSystemControl.IsOver)
            JudgeState = JudgeStateType.Fail;

        Judge?.Invoke();
        IsJudged = true;
    }
}
