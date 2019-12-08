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
        pigs = pigsObject;
        Judge += action;
        IsJudged = false;

        JudgeState = JudgeStateType.Run;
    }

    private void JudgeUpdata()
    {
        if (!(pigs.childCount == 0 || GameManager.Instance.BirdControlSystemControl.IsOver))
            return;

        if ((pigs.childCount == 0 && !GameManager.Instance.BirdControlSystemControl.IsOver) || (pigs.childCount == 0 && GameManager.Instance.BirdControlSystemControl.IsOver))
            JudgeState = JudgeStateType.Clear;
        else if (pigs.childCount != 0 && GameManager.Instance.BirdControlSystemControl.IsOver)
            JudgeState = JudgeStateType.Fail;

        Judge?.Invoke();
        Judge -= Judge;
        IsJudged = true;
    }
}
