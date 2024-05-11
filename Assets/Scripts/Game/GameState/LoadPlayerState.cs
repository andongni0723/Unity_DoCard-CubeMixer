using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlayerState : IGameState
{
    //[Header("Component")]
    //[Header("Settings")]
    //[Header("Debug")]
    public int needCallbackCount => MatchManager.Instance.maxPlayers;
    
    public void EnterState()
    {
    }

    public void UpdateState()
    {
    }

    public void CallChangeNextState()
    {
        EventHandler.CallChangeState(GameState.ActionState);
    }

    public void ExitState()
    {
    }
}
