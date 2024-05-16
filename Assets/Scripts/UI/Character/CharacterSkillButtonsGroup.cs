using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CharacterSkillButtonsGroup : MonoBehaviour
{
    [Header("Component")] 
    public GameObject buttonsObj;
    public Toggle toggle;

    [Header("Settings")] 
    public string ID;
    public CharacterDetailsSO characterDetails;
    public CharacterGameData characterGameData;

    public Character character;

    [Header("Debug")] 
    private bool tempToggleIsOn; // When player play replay action ,
                                 // record the toggle state and turn off the toggle

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        buttonsObj = transform.GetChild(0).gameObject;
        
        toggle.group = transform.parent.GetComponent<ToggleGroup>();
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }
    
    // ------------------- Event -------------------

    private void OnEnable()
    {
        EventHandler.CharacterCardPress += OnCharacterCardPress; // toggle on/off
        EventHandler.CharacterObjectGeneratedDone += InitialUpdateData; // initial
        EventHandler.TurnCharacterStartAction += CloseButtonActive; // close button
        EventHandler.LastPlayActionEnd += OpenButtonActive; // open button if is owner call
    }

    private void OnDisable()
    {
        EventHandler.CharacterCardPress -= OnCharacterCardPress;
        EventHandler.CharacterObjectGeneratedDone -= InitialUpdateData;
        EventHandler.TurnCharacterStartAction -= CloseButtonActive;
        EventHandler.LastPlayActionEnd -= OpenButtonActive; // close button
    }

    private void OnCharacterCardPress(CharacterDetailsSO data, string ID)
    {
        toggle.isOn = data == characterDetails;
    }

    public void InitialUpdateData()
    {
        buttonsObj.SetActive(true);
        for (int i = 0; i < buttonsObj.transform.childCount; i++)
        {
            SkillButton skillButton = buttonsObj.transform.GetChild(i).GetComponent<SkillButton>();
            skillButton.InitialUpdate(this);
        }
        buttonsObj.SetActive(false);

        toggle.isOn = true;
    }

    private void OpenButtonActive()
    {
        if(toggle.isOn) return;
        toggle.isOn = tempToggleIsOn;
    }

    private void CloseButtonActive()
    {
        Debug.Log("Close");
        tempToggleIsOn = toggle.isOn;
        toggle.isOn = false;
    }
    
    // ------------------- Toggle Event -------------------

    private void OnToggleValueChanged(bool isOn)
    {
        buttonsObj.SetActive(isOn);
    }
}
