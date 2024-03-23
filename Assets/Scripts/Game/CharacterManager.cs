using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class CharacterGameData
{
    public int maxHealth;
    public int currentHealth;
    public GameObject characterObject;

    public CharacterGameData(int maxHealth, int currentHealth, GameObject characterObject = null)
    {
        this.maxHealth = maxHealth;
        this.currentHealth = currentHealth;
        this.characterObject = characterObject;
    }
}

public class CharacterManager : Singleton<CharacterManager>
{
    //[Header("Component")]
    [Header("Settings")]
    public List<CharacterDetailsSO> characterDetailsList;
    public Dictionary<string, CharacterGameData> characterGameDataDict = new();
    
    //[Header("Debug")]

    private void Start()
    {
        foreach (var data in characterDetailsList)
        {
            SaveData(data.characterName, new CharacterGameData(data.health, data.health));
        }
        EventHandler.CallPlayerCharactersInitialized(characterDetailsList);
    }

    public void SaveData(string key, CharacterGameData data)
    {
        if (characterGameDataDict.ContainsKey(key))
        {
            characterGameDataDict[key].maxHealth = data.maxHealth;
            characterGameDataDict[key].currentHealth = data.currentHealth;
            characterGameDataDict[key].characterObject = data.characterObject;
        }
        else
        {
            characterGameDataDict.Add(key, data); 
        }
       
    }
    
    public CharacterGameData LoadData(string key)
    {
        if (characterGameDataDict.TryGetValue(key, out var data))
        {
            return data;
        }
        else
        {
            Debug.LogError("Key not found");
            return null;
        }
    }
}
