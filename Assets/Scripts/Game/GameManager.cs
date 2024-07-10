using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Component")]
    public CharacterManager selfCharacterManager; 
    public CharacterManager enemyCharacterManager;
    public GameStateManager gameStateManager;
    
    [Space(15)]
    public CinemachineVirtualCamera redMainCamera;
    public CinemachineVirtualCamera blueMainCamera;
    
    
    [Header("Settings")]
    public Team selfTeam; // Update by CharacterGenerate
    
    //[Header("Debug")]
    
    private int currentPlayerCount = 0;

    public override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 300;
    }

    private void OnEnable()
    {
        EventHandler.ReturnCharacterInitializedDone += GameStart;
    }
    private void OnDisable()
    {
        EventHandler.ReturnCharacterInitializedDone -= GameStart;
    }

    private void GameStart()
    {
        currentPlayerCount++;
        if (currentPlayerCount == MatchManager.Instance.maxPlayers)
            StartCoroutine(WaitAndStartGame());
    }

    private IEnumerator WaitAndStartGame()
    {
        yield return new WaitForSeconds(0.5f);
        EventHandler.CallCharacterObjectGenerate();
        yield return new WaitForSeconds(0.3f);
        EventHandler.CallUIObjectGenerate();
    }
    
    
    // Tools
    public void SetSelfCharacterManager(CharacterManager characterManager)
    {
        selfCharacterManager = characterManager;
    }
    public void SetEnemyCharacterManager(CharacterManager characterManager)
    {
        enemyCharacterManager = characterManager;
    }
    
    public CameraController GetSelfCameraController()
    {
        return selfTeam == Team.Red ? redMainCamera.GetComponent<CameraController>() : blueMainCamera.GetComponent<CameraController>();
    } 
}
