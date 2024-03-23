using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterGenerator : MonoBehaviour
{
    //[Header("Component")]
    [Header("Settings")]
    public Vector2 startSpawnPosition;

    //[Header("Debug")]
    

    private void OnEnable()
    {
        EventHandler.PlayerCharactersInitialized += CharacterGenerate;
    }
    private void OnDisable()
    {
        EventHandler.PlayerCharactersInitialized -= CharacterGenerate;
    }

    private void CharacterGenerate(List<CharacterDetailsSO> data)
    {
        foreach (var character in data)
        {
            // Set spawn position
            RandomSpawnPosition();
            Transform tileTransform = GridManager.Instance.GetTileWithTilePos((int)startSpawnPosition.x, 
                                        (int)startSpawnPosition.y).transform;
            
            // Generate
            Character characterObj = Instantiate(character.characterPrefab, 
                tileTransform.position + Vector3.up * 0.1f, Quaternion.identity).GetComponent<Character>();
            
            // New Object Setting
            characterObj.transform.parent = tileTransform;
            characterObj.characterTilePosition = startSpawnPosition;
            
            // Save Data
            CharacterManager.Instance.SaveData(character.characterName,
                new CharacterGameData(character.health, character.health, characterObj.gameObject));
        } 
        
        EventHandler.CallCharacterObjectGeneratedDone();
    }


    private void RandomSpawnPosition()
    {
        startSpawnPosition = new Vector2(Random.Range(0, 7), Random.Range(0, 15));
    }
}
