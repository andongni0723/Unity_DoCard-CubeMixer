using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

// public class NetworkCharacterDetailsIndexList : INetworkSerializable
// {
//     public int[] dataArray = Array.Empty<int>();
//
//     public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
//     {
//         // Length
//         int length = 0;
//         if (!serializer.IsReader)
//         {
//             length = dataArray.Length;
//         }
//         
//         serializer.SerializeValue(ref length);
//         
//         // Array
//         if (serializer.IsReader)
//         {
//             dataArray = new int[length];
//         }
//         
//         for (int n = 0; n < length; ++n)
//         {
//             serializer.SerializeValue(ref dataArray[n]);
//         }
//     }
// }

public class CharacterGenerator : NetworkBehaviour
{
    //[Header("Component")]
    [Header("Settings")]
    public Vector2 redTeamSpawnPosition;
    public Vector2 blueTeamSpawnPosition;

    private CharacterManager characterManager;

    //[Header("Debug")]
    private bool isInitial = false;
    public Team team; 
    private Vector2 startSpawnPosition;

    public NetworkVariable<int> teamNetwork = new(0, writePerm: NetworkVariableWritePermission.Owner);
    private NetworkVariable<int> currentGenerateID = new(1000);
    private NetworkList<int> tempDataIndexList = new(writePerm: NetworkVariableWritePermission.Owner);
    
    public override void OnNetworkSpawn()
    {
        // tempList value initialize
        // if( IsOwner && tempDataIndexList.Value == null)
        //     tempDataIndexList.Value = new NetworkCharacterDetailsIndexList();
        
        // Event
        teamNetwork.OnValueChanged += OnTeamValueChanged;
        
        // Set Team
        if (IsOwner)
        {
            teamNetwork.Value = IsServer ? 0 : 1;
        }
        
        
        
        isInitial = true;
        EventHandler.ReturnCharacterInitializedDone();
    }
    
    // Call by Owner in CharacterManageder
    public void SetCharacterIndexList(List<int> indexList)
    {
        foreach (var index in indexList)
        {
            tempDataIndexList.Add(index);
        }
    }

    private void Awake()
    {
        characterManager = GetComponent<CharacterManager>();
        
        if(IsOwner)
            tempDataIndexList = new();
    }

    private void OnTeamValueChanged(int previousvalue, int newvalue)
    {
        team = (Team)newvalue;
    }
    
    // Call by Owner
    public void ExecuteCharacterGenerate(List<int> data)
    {
        StartCoroutine(ExecuteCharacterGenerateCoroutine(data));
    }
    
    private IEnumerator ExecuteCharacterGenerateCoroutine(List<int> data)
    {
        yield return new WaitUntil(() => isInitial);
        
        // List<int> to int[]
       

        //tempDataIndexList.Value.dataArray = tempArray;
        GenerateCallServerRpc();
        CharacterGenerate(tempDataIndexList);
    }
    
    [ServerRpc]
    private void currentIDUpdateServerRpc()
    {
        currentGenerateID.Value++;
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
        Debug.Log($"{teamNetwork.Value}: {tempDataIndexList.Count}");
        
        // Set Start Position
        switch (teamNetwork.Value)
        {
            case 0:
                Debug.Log("RED");
                startSpawnPosition = redTeamSpawnPosition;
                break;
            case 1:
                startSpawnPosition = blueTeamSpawnPosition;
                break;
        }

        foreach (var index in data)
        {
            CharacterDetailsSO character = DetailsManager.Instance.UseIndexSearchCharacterDetailsSO(index);
            Transform tileTransform = GridManager.Instance.GetTileWithTilePos((int)startSpawnPosition.x, 
                (int)startSpawnPosition.y).transform;
            
            // Debug.Log($"{teamNetwork.Value}: Generate {character.characterName} at {startSpawnPosition}"); //TODO: Debug
            
            // Generate
            Character characterObj = Instantiate(character.characterPrefab, 
                tileTransform.position + Vector3.up * 0.1f, Quaternion.identity).GetComponent<Character>();
            
            // Update Data
            // characterManager.AddID(currentGenerateID.Value);
            
            // New Object Setting
            characterObj.transform.parent = tileTransform;
            characterObj.characterTilePosition = startSpawnPosition;
            characterObj.InitialUpdateData(currentGenerateID.Value); // TODO: add character to save pos and load pos
            characterObj.SetTeam(team);
            
            // currentIDUpdateServerRpc();
            
            // Save Data
            characterManager.SaveData(character.characterName,
                new CharacterGameData(character.health, character.health, characterObj.gameObject));
            
            // Set spawn position
            startSpawnPosition += Vector2.right;
            // RandomSpawnPosition();
        }

        EventHandler.CallCharacterObjectGeneratedDone();
    }

    
    private void RandomSpawnPosition()
    {
        startSpawnPosition = new Vector2(Random.Range(0, 7), Random.Range(0, 15));
    }
}
