using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CharacterCardManager : MonoBehaviour
{
    [Header("Component")]
    public GameObject characterCardPrefab;
    // [Header("Settings")]
    [Header("Debug")]
    public CharacterGameData currentChooseCharacter;
    
    // get event
    private void OnEnable()
    {
        EventHandler.PlayerCharactersInitialized += OnPlayerCharactersInitialized;
    }
    private void OnDisable()
    {
        EventHandler.PlayerCharactersInitialized -= OnPlayerCharactersInitialized;
    }

    private void OnPlayerCharactersInitialized(List<CharacterDetailsSO> data)
    {
        foreach (var character in data)
        {
            CharacterCard characterCard = Instantiate(characterCardPrefab, transform).GetComponent<CharacterCard>();
            characterCard.characterDetails = character;
            characterCard.InitialUpdateData();
        }
    }
}
