using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButtonManager : MonoBehaviour
{
    [Header("Component")]
    public GameObject skillButtonPrefab;
    // [Header("Settings")]
    // [Header("Debug")]
    
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
            CharacterSkillButtonsGroup characterCard = Instantiate(character.skillButtonsGroupPrefab, transform)
                .GetComponent<CharacterSkillButtonsGroup>();
            
            characterCard.characterDetails = character;
            characterCard.InitialUpdateData();
        }
    }
}
