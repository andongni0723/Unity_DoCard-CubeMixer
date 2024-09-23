using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
#if UNITY_EDITOR
using ParrelSync;
#endif
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using UnityEngine;
using TMPro;
#pragma warning disable CS0618 

public class MatchManager : Singleton<MatchManager>
{
    [Header("Component")]
    public GameObject panel;
    public TMP_Text messageText;
    public TMP_Text networkStatusText;
    private UnityTransport transport;
    
    [Header("Settings")]
    public int maxPlayers = 2;
    private const string joinCodeKey = "j";
    private string playerID;
    
    //[Header("Debug")]
    private Lobby currentConnectedLobby;

    public override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 300;
        transport = FindObjectOfType<UnityTransport>();
    }
    
    // Button Event
    public async void CreateOrJoinLobby()
    {
        await Authenticate();

        currentConnectedLobby = await QuickJoinLobby() ?? await CreateLobby();
        
        // 成功連接大廳
        if(currentConnectedLobby != null)
            panel.SetActive(false);
        
        // 顯示網路狀態
        networkStatusText.text = $"IsHost: {NetworkManager.Singleton.IsHost}\n" +
                                 $"IsServer: {NetworkManager.Singleton.IsServer}\n" +
                                 $"IsClient: {NetworkManager.Singleton.IsClient}\n";
    }

    private async Task Authenticate()
    {
        var option = new InitializationOptions();
        
#if UNITY_EDITOR
        option.SetProfile(ClonesManager.IsClone() ? ClonesManager.GetArgument() : "Primary");
#endif
        
        await UnityServices.InitializeAsync(option);
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        playerID = AuthenticationService.Instance.PlayerId;
    }

    private async Task<Lobby> QuickJoinLobby()
    {
        try
        {
            // 如果沒有找到大廳，就會丟出例外
            var lobby = await Lobbies.Instance.QuickJoinLobbyAsync();
            
            var a = await RelayService.Instance.JoinAllocationAsync(lobby.Data[joinCodeKey].Value);

            transport.SetClientRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key,
                a.ConnectionData, a.HostConnectionData);
            
            NetworkManager.Singleton.StartClient();
            return lobby;
        }
        catch (Exception)
        {
            Debug.Log("No Lobby can quick join");
            messageText.text = "No Lobby can quick join, creating new lobby...";
            return null;
        }
    }

    private async Task<Lobby> CreateLobby()
    {
        // const int maxPlayers = 2;

        maxPlayers = TestModePanel.Instance.singlePlayerMode ? 1 : 2;
        var a = await RelayService.Instance.CreateAllocationAsync(maxPlayers);
        var joinCode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

        // 創建大廳
        var options = new CreateLobbyOptions
        {
            Data = new Dictionary<string, DataObject> { { joinCodeKey, new DataObject(DataObject.VisibilityOptions.Public, joinCode) } } 
        };
        var lobby = await Lobbies.Instance.CreateLobbyAsync("Useless Lobby Name", maxPlayers, options);
        
        // 讓大廳保持活躍
        StartCoroutine(HeartbeatLobby(lobby.Id, 15));
        
        transport.SetHostRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);

        NetworkManager.Singleton.StartHost();
        return lobby;
    }
    
    [SuppressMessage("ReSharper", "IteratorNeverReturns")]
    IEnumerator HeartbeatLobby(string lobbyID, float waitTimeSeconds)
    {
        var delay = new WaitForSecondsRealtime(waitTimeSeconds);
        while (true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyID);
            yield return delay;
        }
    }

    private void OnDestroy()
    {
        try
        {
            StopAllCoroutines();
            if (currentConnectedLobby != null)
            {
                if (currentConnectedLobby.HostId == playerID)
                    Lobbies.Instance.DeleteLobbyAsync(currentConnectedLobby.Id);
                else
                    Lobbies.Instance.RemovePlayerAsync(currentConnectedLobby.Id, currentConnectedLobby.Id); 
            }
        }
        catch (Exception e)
        {
            Debug.Log($"Error closing lobby: {e}");
        }
    }
}
