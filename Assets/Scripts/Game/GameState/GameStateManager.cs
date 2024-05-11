using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class GameStateManager : NetworkBehaviour
{
    //[Header("Component")]
    //[Header("Settings")]
    //[Header("Debug")]
    // [DictionaryDrawerSettings(KeyLabel = "GameState", ValueLabel = "BaseGameState")]
    public GameState currentState = GameState.ActionState;
    private Dictionary<GameState, IGameState> stateData = new () {}; 
    public NetworkVariable<int> stateCallbackCount = new ();

    private void Awake()
    {
        stateData.Add(GameState.LoadPlayerInGame, new LoadPlayerState());
        stateData.Add(GameState.ActionState, new ActionState());
        stateData.Add(GameState.FightState, new FightState());
        stateData.Add(GameState.FightEnd, new FightEndState());
        
        
        EventHandler.CallChangeState(GameState.ActionState);
    }

    public override void OnNetworkSpawn()
    { 
        stateCallbackCount.OnValueChanged += CheckCallbackCount;
    }
    

    private void OnEnable()
    {
        EventHandler.StateCallback += OnStateCallback;
        EventHandler.ChangeState += OnChangeState;
    }

    private void OnDisable()
    {
        EventHandler.StateCallback -= OnStateCallback;
        EventHandler.ChangeState -= OnChangeState;
    }

    private void OnStateCallback(GameState targetState)
    {
        if (targetState != currentState)
        {
            HintPanelManager.Instance.CallError("State and State Callback is not match");
            return;
        }

        // add network variable value
        if (IsServer) stateCallbackCount.Value++;
        else AddCallbackCountServerRpc(1);
    }
    
    private void CheckCallbackCount(int previousValue, int newValue)
    {
        // check the callback is enough
        Debug.Log($"{newValue} / {stateData[currentState].needCallbackCount}");
        if (newValue == stateData[currentState].needCallbackCount)
        {
            if (!IsServer) return; // Only server can change state
            
            stateData[currentState].CallChangeNextState();
            Debug.Log("call");       
            ChangeStateClientRpc(); // call client to change state

        }
    }
    
    [ClientRpc(RequireOwnership = false)]
    public void ChangeStateClientRpc()
    {
        if(IsServer) return;
        stateData[currentState].CallChangeNextState();
        Debug.Log("call");       
    }
    
    
    [ServerRpc(RequireOwnership = false)]
    public void AddCallbackCountServerRpc(int i)
    {
        stateCallbackCount.Value += i;
    }

    private void OnChangeState(GameState toState)
    {
        stateData[currentState].ExitState();
        currentState = toState;
        if (IsServer) stateCallbackCount.Value = 0;
        // else AddCallbackCountServerRpc(-stateCallbackCount.Value);
        stateData[currentState].EnterState();
    }

    private void Update()
    {
        stateData[currentState].UpdateState();
    }
}