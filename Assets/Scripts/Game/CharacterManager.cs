using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class CharacterManager : NetworkBehaviour
{
    [Header("Component")] 
    private CharacterGenerator characterGenerator;
    public CharacterActionRecord characterActionRecord;
    
    [Header("Settings")]
    // public List<CharacterDetailsSO> characterDetailsList;
    public List<int> characterDetailsList;
    public Dictionary<string, CharacterGameData> characterGameDataDict = new();
    // private NetworkList<int> characterIDList;
    // public NetworkCharacterActionDataList characterActionDataList = new();
    public NetworkVariable<FixedString512Bytes> characterActionDataString
        = new(writePerm: NetworkVariableWritePermission.Owner);


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
        
        foreach (var index in characterDetailsList)
        {
            CharacterDetailsSO data = DetailsManager.Instance.UseIndexSearchCharacterDetailsSO(index);
            SaveData(data.characterName, new CharacterGameData(data.health, data.power, data.health, data.power));
        }
        
        //character action list On Value Changed
        characterActionDataString.OnValueChanged += (previous, current) =>
        {
            if (!IsOwner)
            {
                characterActionRecord.StringDataToListData(current.Value);
            }
        };
    }

    private void Awake()
    {
        characterGenerator = GetComponent<CharacterGenerator>();
        characterActionRecord = GetComponent<CharacterActionRecord>();
        // characterIDList = new();
    }

    // ------------------- Event -------------------
    
    private void OnEnable()
    {
        EventHandler.CharacterObjectGenerate += CallCharacterGenerate; // not owner generate character
        EventHandler.UpdateCharacterActionData += SaveCharacterActionData; // update character action data
        EventHandler.TurnCharacterStartAction += OnTurnCharacterStartAction; // play the character action
        EventHandler.LastPlayActionEnd += OnLastPlayActionEnd; // Check is fight state and callback
        EventHandler.ChangeStateDone += OnChangeStateDone;
    }

    private void OnDisable()
    {
        EventHandler.CharacterObjectGenerate -= CallCharacterGenerate;
        EventHandler.UpdateCharacterActionData -= SaveCharacterActionData;
        EventHandler.TurnCharacterStartAction -= OnTurnCharacterStartAction;
        EventHandler.LastPlayActionEnd -= OnLastPlayActionEnd;
        EventHandler.ChangeStateDone -= OnChangeStateDone;
    }

    private void OnChangeStateDone(GameState newState)
    {
        if (GameManager.Instance.gameStateManager.currentState == GameState.FightEndState && IsOwner)
        {
            Invoke(nameof(TestCallBackFightEnd), 2);
        }
    }

    public void TestCallBackFightEnd() // TODO: test 
    {
        EventHandler.CallStateCallback(GameState.FightEndState);
    }

    private void OnLastPlayActionEnd()
    {
        if (GameManager.Instance.gameStateManager.currentState == GameState.FightState && IsOwner)
        {
            EventHandler.CallStateCallback(GameState.FightState);
        }
    }

    private void CallCharacterGenerate()
    {
        if(!IsOwner) return;
        characterGenerator.ExecuteCharacterGenerate(characterDetailsList);
    }

    private void OnTurnCharacterStartAction()
    {
        StartCoroutine(CharacterActionListToAttackAction(characterActionRecord.characterActionDataList));
    }

    // ------------------- Game -------------------
    private void SaveCharacterActionData()
    {
        if(IsOwner)
            characterActionDataString.Value = characterActionRecord.ListDataToStringData();
    }
    
    private IEnumerator CharacterActionListToAttackAction(List<CharacterActionData> actionDataList)
    {
        if(IsOwner) EventHandler.CallCharacterBackToTurnStartPoint();
        yield return new WaitForSeconds(1.5f);

        if (actionDataList.IsNullOrEmpty())
        {
            if(IsOwner) EventHandler.CallLastPlayActionEnd();
            yield break;
        }
        
        for(int i = 0; i < actionDataList.Count; i++)
        {
            var character = DetailsManager.Instance.UseCharacterIDSearchCharacter(actionDataList[i].actionCharacterID);
            
            // Execute Move or Skill
            if(actionDataList[i].actionType == SkillButtonType.Move)
                 character.MoveAction(
                    actionDataList[i].actionTilePosList[0], 
                    isLastPlayAction: i == actionDataList.Count - 1);
            else
            {
                yield return StartCoroutine(character.AttackAction(
                    actionDataList[i].actionSkillName,
                    actionDataList[i].actionType,
                    actionDataList[i].actionTilePosList,
                    i == actionDataList.Count - 1));
            }

            yield return new WaitForSeconds(0.3f);
        }
    }
    
    // [ServerRpc]
    // private void AddIDServerRpc(int id)
    // {
    //     characterIDList.Add(id);
    // }
    
    // public void AddID(int id)
    // {
    //     AddIDServerRpc(id);
    // }
    //
    // public void SavePosition(string key, Vector2 saveTilePos)
    // {
    //     characterGameDataDict[key].tilePosition = saveTilePos;
    // }
    // public bool LoadPosition(string key, ref Vector2 data)
    // {
    //     if(characterGameDataDict.TryGetValue(key, out var value))
    //     {
    //         data = value.tilePosition;
    //         return true;
    //     }
    //     
    //     if (characterGameDataDict[key].tilePosition == new Vector2(-1, -1))
    //         return false;
    //     
    //     data = characterGameDataDict[key].tilePosition;
    //     return true;
    // }

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
    
    // public CharacterGameData LoadData(string key)
    // {
    //     if (characterGameDataDict.TryGetValue(key, out var data))
    //     {
    //         return data;
    //     }
    //     else
    //     {
    //         Debug.LogError("Key not found");
    //         return null;
    //     }
    // }
}

// ------------------- Data -------------------

[Serializable]
public class CharacterGameData
{
    public int maxHealth;
    public int maxPower;
    public int currentHealth;
    public int currentPower;
    public GameObject characterObject;
    public Vector2 tilePosition;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="maxHealth">if value is -1, method will not setting</param>
    /// <param name="maxPower">if value is -1, method will not setting</param>
    /// <param name="currentHealth"></param>
    /// <param name="currentPower"></param>
    /// <param name="characterObject"></param>
    public CharacterGameData(int maxHealth, int maxPower, int currentHealth, int currentPower, GameObject characterObject = null, Vector2 tilePosition = default)
    {
        this.maxHealth = maxHealth;
        this.currentHealth = currentHealth;
        this.maxPower = maxPower;
        this.currentPower = currentPower;
        this.characterObject = characterObject;
        this.tilePosition = tilePosition;
    }
}

// [Serializable]
// public class NetworkCharacterActionDataList : NetworkVariableBase
// {
//        public List<CharacterActionData> tilePosList = new();
//
//        public override void WriteField(FastBufferWriter writer)
//        {
//            // Serialize the data we need to synchronize
//            writer.WriteValueSafe(tilePosList.Count);
//            foreach (var data in tilePosList)
//            {
//                WriteString(ref data.actionCharacterID, writer);
//                WriteString(ref data.actionSkillName, writer);
//                
//                writer.WriteValueSafe(data.actionType);
//                writer.WriteValueSafe(data.actionTilePosList.Count);
//                foreach (var tilePos in data.actionTilePosList)
//                {
//                    writer.WriteValueSafe(tilePos);
//                }
//            }
//        }
//
//        public override void ReadField(FastBufferReader reader)
//        {
//            // De-Serialize the data being synchronized
//            reader.ReadValueSafe(out int itemsToUpdate);
//            tilePosList.Clear();
//            for (int i = 0; i < itemsToUpdate; i++)
//            {
//                var newTilePosList = new CharacterActionData();
//                
//                ReadString(out newTilePosList.actionCharacterID, reader);
//                ReadString(out newTilePosList.actionSkillName, reader);
//                reader.ReadValueSafe(out newTilePosList.actionType);
//
//                reader.ReadValueSafe(out int itemsCount);
//                newTilePosList.actionTilePosList.Clear();
//                for (int j = 0; j < itemsCount; j++)
//                {
//                    reader.ReadValueSafe(out Vector2 tempTilePos);
//                    newTilePosList.actionTilePosList.Add(tempTilePos);
//                }
//                tilePosList.Add(newTilePosList);
//            }
//        }
//        
//        public void WriteString(ref string Text, FastBufferWriter writer)
//        {
//            // If there isn't thing, then return 0 as the string size
//            if (string.IsNullOrEmpty(Text))
//            {
//                writer.WriteValueSafe(0);
//                return;
//            }
//
//            var textByteArray = System.Text.Encoding.ASCII.GetBytes(Text);
//
//            // Write the total size of the string
//            writer.WriteValueSafe(textByteArray.Length);
//            var toalBytesWritten = 0;
//            var bytesRemaining = textByteArray.Length;
//            // Write the string values
//            while (bytesRemaining > 0)
//            {
//                writer.WriteValueSafe(textByteArray[toalBytesWritten]);
//                toalBytesWritten++;
//                bytesRemaining = textByteArray.Length - toalBytesWritten;
//            }
//        }
//
//        public void ReadString(out string Text, FastBufferReader reader)
//        {
//            // Reset our string to empty
//            Text = string.Empty;
//            // Get the string size in bytes
//            reader.ReadValueSafe(out int stringSize);
//
//            // If there isn't thing, then we are done
//            if (stringSize == 0)
//            {
//                return;
//            }
//
//            // allocate an byte array to 
//            var byteArray = new byte[stringSize];
//            for(int i = 0; i < stringSize; i++)
//            {
//                reader.ReadValueSafe(out byte tempByte);
//                byteArray[i] = tempByte;
//            }
//         
//            // Convert it back to a string
//            Text = System.Text.Encoding.ASCII.GetString(byteArray);
//        }
//
//        public override void ReadDelta(FastBufferReader reader, bool keepDirtyDelta)
//        {
//            // Do nothing for this example
//        }
//
//        public override void WriteDelta(FastBufferWriter writer)
//        {
//            // Do nothing for this example
//        }
//
// }
