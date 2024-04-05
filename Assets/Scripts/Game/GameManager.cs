using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Component")]
    [SerializeField] public CharacterManager selfCharacterManager;
    [SerializeField] public CharacterManager enemyCharacterManager;
    //[Header("Settings")]
    
    //[Header("Debug")]
    private int currentPlayerCount = 0;
    
    public void SetSelfCharacterManager(CharacterManager characterManager)
    {
        selfCharacterManager = characterManager;
    }
    public void SetEnemyCharacterManager(CharacterManager characterManager)
    {
        enemyCharacterManager = characterManager;
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
        if (currentPlayerCount == 2)
            StartCoroutine(WaitAndStartGame());
    }

    private IEnumerator WaitAndStartGame()
    {
        yield return new WaitForSeconds(1);
        EventHandler.CallCharacterObjectGenerate();
    }
    public override void Awake()
    {
        base.Awake();
    }
}
