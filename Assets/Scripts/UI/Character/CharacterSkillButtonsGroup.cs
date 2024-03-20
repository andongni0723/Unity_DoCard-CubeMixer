using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkillButtonsGroup : MonoBehaviour
{
    [Header("Component")] 
    public GameObject buttons;
    
    [Header("Settings")]
    public CharacterDetailsSO characterDetails;
    //[Header("Debug")]

    private void OnEnable()
    {
        EventHandler.CharacterCardPress += OnCharacterCardPress;
    }
    private void OnDisable()
    {
        EventHandler.CharacterCardPress -= OnCharacterCardPress;
    }

    private void OnCharacterCardPress(CharacterDetailsSO data)
    {
        buttons.SetActive(data == characterDetails);
    }

    public void InitialUpdateUI()
    {
        buttons.SetActive(false);
    }
}
