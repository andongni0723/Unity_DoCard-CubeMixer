using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ActionState : IGameState
{
    public GameState state;
    // private int getCallbackCount = 0;

    public int needCallbackCount => MatchManager.Instance.maxPlayers;

    public void EnterState()
    {
        state = GameState.ActionState;
        HintPanelManager.Instance.CallHint("Start Action");
    }

    public void UpdateState()
    {
    }

    public void CallChangeNextState()
    {
        EventHandler.CallChangeState(GameState.FightState);
    }

    public void ExitState()
    {
    }
}
