using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameState
{
    int needCallbackCount { get;}
    void EnterState(); 
    void UpdateState(); 
    void CallChangeNextState();
    void ExitState();
}
