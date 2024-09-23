using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CharacterGenerator : NetworkBehaviour
{
    //[Header("Component")]
    [Header("Settings")]
    public Vector2 redTeamSpawnPosition;
    public Vector2 blueTeamSpawnPosition;

    private CharacterManager characterManager;

    //[Header("Debug")]
    private int currentGenerateID = 1;
    private bool isInitial = false;
    public Team team; 
    private Vector2 startSpawnPosition;

    public NetworkVariable<int> teamNetwork = new(0, writePerm: NetworkVariableWritePermission.Owner);
    private NetworkList<int> tempDataIndexList;
    
    public override void OnNetworkSpawn()
    {
        Debug.Log("d");

        // Event
        teamNetwork.OnValueChanged += OnTeamValueChanged;
        
        // Set Team
        if (IsOwner)
        {
            teamNetwork.Value = IsServer ? 0 : 1;
        }
        
        isInitial = true;
        EventHandler.CallReturnCharacterInitializedDone();
    }
    private void Awake()
    {
        characterManager = GetComponent<CharacterManager>();
        tempDataIndexList = new(writePerm: NetworkVariableWritePermission.Owner);
        //
        // if (IsOwner)
        //     tempDataIndexList = new();
    }
    
    // Call by Owner in CharacterManageder
    public void SetCharacterIndexList(List<int> indexList)
    {
        foreach (var index in indexList)
        {
            tempDataIndexList.Add(index);
        }
    }

    

    private void OnTeamValueChanged(int previousvalue, int newvalue)
    {
        team = (Team)newvalue;
        
        if(IsOwner)
            GameManager.Instance.selfTeam = team;

    }
    
    // Call by Owner
    public void ExecuteCharacterGenerate(List<int> data)
    {
        StartCoroutine(ExecuteCharacterGenerateCoroutine(data));
    }
    
    private IEnumerator ExecuteCharacterGenerateCoroutine(List<int> data)
    {
        yield return new WaitUntil(() => isInitial);
        
        GenerateCallServerRpc();
        CharacterGenerate(tempDataIndexList);
    }
    
    [ServerRpc]
    private void currentIDUpdateServerRpc()
    {
        // currentGenerateID.Value++;
    }
    
    [ServerRpc]
    private void GenerateCallServerRpc()
    {
        GenerateCallClientRpc();
    }
    
    [ClientRpc]
    private void GenerateCallClientRpc()
    {
        if (!IsOwner)
        {
            CharacterGenerate(tempDataIndexList);
        }
    }

    private void CharacterGenerate(NetworkList<int> data)
    {
        data = tempDataIndexList;
        
        // Set Start Position
        switch (teamNetwork.Value)
        {
            case 0:
                startSpawnPosition = redTeamSpawnPosition;
                break;
            case 1:
                startSpawnPosition = blueTeamSpawnPosition;
                break;
        }

        foreach (var index in data)
        {
            CharacterDetailsSO character = DetailsManager.Instance.UseIndexSearchCharacterDetailsSO(index);
            Transform tileTransform = GridManager.Instance.GetTileWithTilePos(startSpawnPosition).transform;
            
            
            // Generate
            Character newCharacter = Instantiate(character.characterPrefab, 
                tileTransform.position + Vector3.up * 0.1f, Quaternion.identity).GetComponent<Character>();
            
            // New Object Setting
            newCharacter.transform.parent = tileTransform;
            newCharacter.characterTilePosition = startSpawnPosition;
            newCharacter.characterManager = characterManager;
            newCharacter.characterDetails = character;
            newCharacter.InitialUpdateData(GenerateCharacterID(team, currentGenerateID)); // TODO: add character to save pos and load pos
            newCharacter.SetTeam(team);
            
            // currentIDUpdateServerRpc();
            
            // Save Data
            characterManager.SaveData(character.characterName,
                new CharacterGameData(
                    character.health,
                    character.power, 
                    character.health, 
                    character.power, 
                    newCharacter.gameObject,
                    statusList: new()));
            
            // Set spawn position
            startSpawnPosition += Vector2.right;
            currentGenerateID++;
        }
        
        EventHandler.CallCharacterObjectGeneratedDone(IsOwner);
    }

    private string GenerateCharacterID(Team team, int currentGenerateID)
    {
        string id = team == Team.Red ? "R" : "B";
        
        id += currentGenerateID.ToString("00");
        
        return id;
    }
    
    private void RandomSpawnPosition()
    {
        startSpawnPosition = new Vector2(Random.Range(0, 7), Random.Range(0, 15));
    }
}
