using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightEndState : IGameState
{
    public GameState state;
    private int getCallbackCount = 0;

    public int needCallbackCount => 0;

    public void EnterState()
    {
        state = GameState.FightState;
    }

    public void UpdateState()
    {
    }

    public void CallChangeNextState()
    {
        getCallbackCount++;
        if(getCallbackCount == MatchManager.Instance.maxPlayers)
        {
            EventHandler.CallChangeState(GameState.FightEnd);
        }
    }

    public void ExitState()
    {
    }
}
