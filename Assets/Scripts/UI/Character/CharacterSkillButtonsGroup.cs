using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkillButtonsGroup : MonoBehaviour
{
    [Header("Component")] 
    public GameObject buttons;

    [Header("Settings")] 
    public string ID;
    public CharacterDetailsSO characterDetails;
    public CharacterGameData characterGameData;

    public Character character;
    //[Header("Debug")]

    private void OnEnable()
    {
        EventHandler.CharacterCardPress += OnCharacterCardPress;
        EventHandler.CharacterObjectGeneratedDone += InitialUpdateData;
    }
    private void OnDisable()
    {
        EventHandler.CharacterCardPress -= OnCharacterCardPress;
        EventHandler.CharacterObjectGeneratedDone -= InitialUpdateData;
    }

    private void OnCharacterCardPress(CharacterDetailsSO data, string ID)
    {
        buttons.SetActive(data == characterDetails);
    }

    public void InitialUpdateData()
    {
        buttons.SetActive(false);
        // characterGameData = GameManager.Instance.selfCharacterManager.LoadData(characterDetails.characterName);
        
        for (int i = 0; i < buttons.transform.childCount; i++)
        {
            SkillButton skillButton = buttons.transform.GetChild(i).GetComponent<SkillButton>();
            skillButton.InitialUpdate(this);
        }
    }
}
