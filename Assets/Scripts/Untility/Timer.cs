using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
#pragma warning disable CS4014

public class Timer
{
    private TaskSingol.CtsInfo ctsInfo;
    public bool isTiming;
    public bool isFinished;
    public event Action timerStartEvent;
    public event Action timerFinishEvent;
    
    private void CallTimerStartEvent()
    {
        timerStartEvent?.Invoke();
    }
    private void CallTimerFinishEvent()
    {
        timerFinishEvent?.Invoke();
    }

    /// <summary>
    /// Start timer (Call by other script)
    /// </summary>
    /// <param name="duration">timer duration</param>
    public void StartTimer(float duration)
    {
        ctsInfo = TaskSingol.CreatCts();
        TimerAction(duration, ctsInfo.cts.Token);
    }
    
    public void StopTimer()
    {
        //isFinished = false;
        isTiming = false;
        if (ctsInfo != null)
            TaskSingol.CancelTask(ctsInfo.id);
    }
    
    private async UniTask TimerAction(float duration, CancellationToken ctx)
    {
        isFinished = false;
        isTiming = true;

        CallTimerStartEvent();
        await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: ctx);
        CallTimerFinishEvent();
        
        isTiming = false;
        isFinished = true;
    }
}