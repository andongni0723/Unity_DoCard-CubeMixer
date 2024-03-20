using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGameData
{
    public int maxHealth;
    public int currentHealth;
}
public class CharacterManager : MonoBehaviour
{
    //[Header("Component")]
    [Header("Settings")]
    public List<CharacterDetailsSO> characterDetailsList;
    public Dictionary<string, CharacterGameData> characterGameDataDictionary = new();
    
    //[Header("Debug")]


    private void Start()
    {
        EventHandler.CallPlayerCharactersInitialized(characterDetailsList);
    }
}
