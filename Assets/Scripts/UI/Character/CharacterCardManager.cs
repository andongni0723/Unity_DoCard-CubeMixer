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
    
    [Header("Settings")]
    [SerializeField]private Team team;

    [Header("Debug")] 
    [SerializeField]private int currentGenerateID = 1;
    public CharacterGameData currentChooseCharacter;
    
    // get event
    private void OnEnable()
    {
        EventHandler.UIObjectGenerate += OnUIObjectGenerate; // Update data and Call Generate Character Card
    }
    private void OnDisable()
    {
        EventHandler.UIObjectGenerate -= OnUIObjectGenerate;
    }

    private void OnUIObjectGenerate()
    {
        var data = GameManager.Instance.selfCharacterManager.characterDetailsList;
        team = GameManager.Instance.selfTeam;
        GenerateCharacterCard(data);
    }
    
    private void GenerateCharacterCard(List<int> data)
    {
        foreach (var index in data)
        {
            CharacterDetailsSO character = DetailsManager.Instance.UseIndexSearchCharacterDetailsSO(index);
            CharacterCard characterCard = Instantiate(characterCardPrefab, transform).GetComponent<CharacterCard>();
            
            // Update data
            characterCard.characterDetails = character;
            characterCard.ID = GenerateCharacterID(team, currentGenerateID);
            characterCard.InitialUpdateData();

            currentGenerateID++;
        }
    }
    
    private string GenerateCharacterID(Team team, int currentGenerateID)
    {
        string id = team == Team.Red ? "R" : "B";
        
        id += currentGenerateID.ToString("00");
        
        return id;
    }
}
