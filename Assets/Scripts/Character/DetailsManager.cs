using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

public class DetailsManager : Singleton<DetailsManager>
{
    //[Header("Component")]
    [Header("Settings")]
    [SerializeField] private List<CharacterDetailsSO> allCharacterDetailsList = new();
    [SerializeField] private Dictionary<string, Character> characterDict = new();
    public Dictionary<StatusEffectSO, BaseStatus> statusScriptDict = new();

    [Header("Public Data")]
    public Vector2 mouseHitTilePos;
    
    //[Header("Debug")]

    private void OnEnable()
    {
        EventHandler.ReturnMouseHitTilePosition += OnReturnMouseHitTilePosition;
    }

    private void OnDisable()
    {
        EventHandler.ReturnMouseHitTilePosition -= OnReturnMouseHitTilePosition;
    }

    private void OnReturnMouseHitTilePosition(Vector2 tilePos)
    {
        mouseHitTilePos = tilePos;
    }


    public CharacterDetailsSO UseIndexSearchCharacterDetailsSO(int index)
    {
        try
        {
            return allCharacterDetailsList[index];
        }
        catch (Exception e)
        {
            Debug.LogError("SearchCharacterDetailsError: Index Error\n" + e);
            return null;
        }
    }
    
    public void NewCharacterDetails(Character character)
    {
        characterDict.Add(character.ID, character);
    }
    
    public Character UseCharacterIDSearchCharacter(string id)
    {
        try
        {
            return characterDict[id];
        }
        catch (Exception e)
        {
            Debug.LogError("SearchCharacterError: ID Error Or Dictionary Initial Error\n" + e);
            return null;
        }
    }
    
    // public IStatus UseStatusEffectSOGetStatusScript(StatusEffectSO statusEffect)
    // {
    //     try
    //     {
    //         // return statusScriptDict[statusEffect];
    //         return new();
    //     }
    //     catch (Exception e)
    //     {
    //         Debug.LogError("SearchStatusScriptError: Doesn't put the ScriptableObject bind with script to dict\n" + e);
    //         return null;
    //     }
    // }
    
}

