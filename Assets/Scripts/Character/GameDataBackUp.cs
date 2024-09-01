using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataBackUp : MonoBehaviour
{
    //[Header("Component")]
    public Character character;
    public CharacterHealth characterHealth;
    public CharacterPowerManager characterPower;
    public CharacterStatusManager characterStatusManager;
    
    //[Header("Settings")]
    public CharacterDetailsSO characterDetails;
    
    //[Header("BackUp Data")]
    private CharacterGameData turnStartGameData;
    
    //[Header("Debug")]

    private void Awake()
    {
        character = GetComponent<Character>();
        characterHealth = GetComponent<CharacterHealth>();
        characterPower = GetComponent<CharacterPowerManager>();
        characterStatusManager = GetComponent<CharacterStatusManager>();
    }

    /// <summary>
    /// Call by Character.cs
    /// </summary>
    public void InitialUpdate()
    {
        characterDetails = character.characterDetails;
    }

    private void OnEnable()
    {
        EventHandler.ChangeStateDone += OnChangeStateDone; // check is Action state to update turn start data
        EventHandler.CharacterActionClear += RestoreCharacterData; // character data back to turn start data
        EventHandler.CharacterBackToTurnStartPoint += BackToTurnStartPoint; // setting variable
    }
    private void OnDisable()
    {
        EventHandler.ChangeStateDone -= OnChangeStateDone;
        EventHandler.CharacterActionClear -= RestoreCharacterData; 
        EventHandler.CharacterBackToTurnStartPoint -= BackToTurnStartPoint;
    }

    private void OnChangeStateDone(GameState currentState)
    {
        if(currentState != GameState.ActionState) return;
        
        StartCoroutine(FirstSaveCharacterData());
    }
    
    IEnumerator FirstSaveCharacterData()
    {
        yield return new WaitUntil(() => characterDetails != null);
        
        
        turnStartGameData = new CharacterGameData(
            characterDetails.health, 
            characterDetails.power, 
            characterHealth.currentHealth,
            characterHealth.currentPower, 
            tilePosition: character.characterTilePosition,
            statusList:characterStatusManager.GetStatusList()); 
    }

    private void RestoreCharacterData()
    {
        StartCoroutine(RestoreCharacterDataCoroutine());
    }
    
    IEnumerator RestoreCharacterDataCoroutine()
    {
        yield return new WaitUntil(() => turnStartGameData != null);
        
        character.MoveAction(turnStartGameData.tilePosition, 0);
        characterHealth.SetHealth(turnStartGameData.currentHealth);
        characterHealth.SetPower(turnStartGameData.currentPower); 
        characterStatusManager.SetStatusList(turnStartGameData.statusList);
    }

    private void BackToTurnStartPoint()
    {
        StartCoroutine(BackToTurnStartPointCoroutine());
    }
    
    IEnumerator BackToTurnStartPointCoroutine()
    {
        yield return new WaitUntil(() => turnStartGameData != null);
        
        character.MoveAction(turnStartGameData.tilePosition, 0);
        characterHealth.SetHealth(turnStartGameData.currentHealth);
    }
}
