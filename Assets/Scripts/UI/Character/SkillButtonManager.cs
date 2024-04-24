using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButtonManager : MonoBehaviour
{
    [Header("Component")]
    public GameObject skillButtonPrefab;

    [Header("Settings")] 
    [SerializeField] private Team team;
    
    [Header("Debug")]
    private int currentGenerateID = 1;
    
    // get event
    private void OnEnable()
    {
        EventHandler.UIObjectGenerate += OnUIObjectGenerate; // Update data and Call Generate Skill Button Group
    }
    private void OnDisable()
    {
        EventHandler.UIObjectGenerate -= OnUIObjectGenerate;
    }

    private void OnUIObjectGenerate()
    {
        team = GameManager.Instance.selfTeam;
        var data = GameManager.Instance.selfCharacterManager.characterDetailsList;
        CharacterSkillGroupGenerate(data);
    }

    private void CharacterSkillGroupGenerate(List<int> data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            var character = DetailsManager.Instance.UseIndexSearchCharacterDetailsSO(data[i]);

            CharacterSkillButtonsGroup characterCard = Instantiate(character.skillButtonsGroupPrefab, transform)
                .GetComponent<CharacterSkillButtonsGroup>();
            
            // Update data
            characterCard.characterDetails = character;
            characterCard.ID = GenerateCharacterID(team, currentGenerateID);
            characterCard.character = DetailsManager.Instance.UseCharacterIDSearchCharacter(characterCard.ID); // TODO:
            characterCard.InitialUpdateData();

            // Last skill button group turn on
            if(i == data.Count - 1) 
                EventHandler.CallCharacterCardPress(character, characterCard.ID);
            
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
