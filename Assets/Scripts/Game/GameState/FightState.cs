using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightState : IGameState
{
    public GameState state;
    public int needCallbackCount => MatchManager.Instance.maxPlayers;

    public void EnterState()
    {
        state = GameState.FightState;
        HintPanelManager.Instance.CallHint("Fight !!!");
        EventHandler.CallUpdateCharacterActionData();
        EventHandler.CallTurnCharacterStartAction();
    }

    public void UpdateState()
    {
    }

    public void CallChangeNextState()
    {
        EventHandler.CallChangeState(GameState.FightEndState);
    }

    public void ExitState()
    {
    }
}