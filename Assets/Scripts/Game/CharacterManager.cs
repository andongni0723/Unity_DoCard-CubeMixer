using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;


[Serializable]
public class CharacterGameData
{
    public int maxHealth;
    public int currentHealth;
    public GameObject characterObject;
    public Vector2 tilePosition;

    public CharacterGameData(int maxHealth, int currentHealth, GameObject characterObject = null)
    {
        this.maxHealth = maxHealth;
        this.currentHealth = currentHealth;
        this.characterObject = characterObject;
        this.tilePosition = new Vector2(-1, -1);
    }
}

public class CharacterManager : NetworkBehaviour
{
    [Header("Component")] 
    private CharacterGenerator characterGenerator;

    
    
    [Header("Settings")]
    // public List<CharacterDetailsSO> characterDetailsList;
    public List<int> characterDetailsList;
    public Dictionary<string, CharacterGameData> characterGameDataDict = new();
    private NetworkList<int> characterIDList;

    //[Header("Debug")]


    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            GameManager.Instance.SetSelfCharacterManager(this);
            characterGenerator.SetCharacterIndexList(characterDetailsList);
        }
        else
            GameManager.Instance.SetEnemyCharacterManager(this);
    }

    private void Awake()
    {
        characterGenerator = GetComponent<CharacterGenerator>();
        characterIDList = new();
    }

    // Event
    private void OnEnable()
    {
        EventHandler.CharacterObjectGenerate += CallCharacterGenerate;
    }

    private void OnDisable()
    {
        EventHandler.CharacterObjectGenerate -= CallCharacterGenerate;
    }

    private void CallCharacterGenerate()
    {
        if(!IsOwner) return;
        characterGenerator.ExecuteCharacterGenerate(characterDetailsList);
    }


    private void Start()
    {
        foreach (var index in characterDetailsList)
        {
            CharacterDetailsSO data = DetailsManager.Instance.UseIndexSearchCharacterDetailsSO(index);
            SaveData(data.characterName, new CharacterGameData(data.health, data.health));
        }
    }

    [ServerRpc]
    private void AddIDServerRpc(int id)
    {
        characterIDList.Add(id);
    }
    
    public void AddID(int id)
    {
        AddIDServerRpc(id);
    }

    public void SavePosition(string key, Vector2 saveTilePos)
    {
        characterGameDataDict[key].tilePosition = saveTilePos;
    }
    public bool LoadPosition(string key, ref Vector2 data)
    {
        if (characterGameDataDict[key].tilePosition == new Vector2(-1, -1))
            return false;
        
        data = characterGameDataDict[key].tilePosition;
        return true;
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
