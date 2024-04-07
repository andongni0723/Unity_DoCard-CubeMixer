using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailsManager : Singleton<DetailsManager>
{
    //[Header("Component")]
    [Header("Settings")]
    [SerializeField] private List<CharacterDetailsSO> allCharacterDetailsList = new();
    [SerializeField] private Dictionary<string, Character> characterDict = new();
    
    //[Header("Debug")]
    
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
}

