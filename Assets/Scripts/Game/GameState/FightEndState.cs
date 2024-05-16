using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightEndState : IGameState
{
    public GameState state;
    public int needCallbackCount => MatchManager.Instance.maxPlayers;

    public void EnterState()
    {
        state = GameState.FightEndState;
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
