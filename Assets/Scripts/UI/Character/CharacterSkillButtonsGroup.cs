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

    public Character character; // set by SkillButtonManager

    [Header("Data")] private Dictionary<string, SkillButton> skillIDToSkillButtonDict = new();

    [Header("Debug")] 
    public bool tempToggleIsOn; // When player play replay action ,
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
        EventHandler.StateCallback += CloseButtonActive; // press done button, close button
        EventHandler.TurnCharacterStartAction += CloseButtonActive; // close button
        EventHandler.LastPlayActionEnd += OnLastPlayActionEnd; // open button if is  Action state
        EventHandler.ChangeStateDone += OnChangeStateDone; // open button if is  Action state
    }
    

    private void OnDisable()
    {
        EventHandler.CharacterCardPress -= OnCharacterCardPress;
        EventHandler.CharacterObjectGeneratedDone -= InitialUpdateData;
        EventHandler.StateCallback += CloseButtonActive;
        EventHandler.TurnCharacterStartAction -= CloseButtonActive;
        EventHandler.LastPlayActionEnd -= OnLastPlayActionEnd;
        EventHandler.ChangeStateDone -= OnChangeStateDone;

    }

    private void OnCharacterCardPress(CharacterDetailsSO data, string ID)
    {
        toggle.isOn = data == characterDetails;
    }

    public void InitialUpdateData()
    {
        buttonsObj.SetActive(true);
        
        foreach (var skillButton in buttonsObj.GetComponentsInChildren<SkillButton>())
        {
            skillIDToSkillButtonDict.Add(skillButton.skillDetails.skillID, skillButton); // Update Dict
            skillButton.InitialUpdate(this); // Initial Button
        }
        buttonsObj.SetActive(false);

        toggle.isOn = true;
    }
    private void OnLastPlayActionEnd()
    {
        if(GameManager.Instance.gameStateManager.currentState == GameState.ActionState)
            OpenButtonActive();
    }
    
    private void OnChangeStateDone(GameState newGameState)
    {
        if (newGameState == GameState.ActionState)
            OpenButtonActive();
    }

    private void OpenButtonActive()
    {
        if(toggle.isOn) return;
        toggle.isOn = tempToggleIsOn;
    }

    private void CloseButtonActive()
    {
        if(!toggle.isOn) return;
        tempToggleIsOn = toggle.isOn;
        toggle.isOn = false;
    }
    private void CloseButtonActive(GameState gameState)
    {
        if(gameState == GameState.ActionState)
            CloseButtonActive();
    }
    
    // ------------------- Tool -------------------
    public SkillButton UseSkillIDToSkillButton(string skillID)
    {
        return skillIDToSkillButtonDict[skillID];
    }
    
    // ------------------- Toggle Event -------------------

    private void OnToggleValueChanged(bool isOn)
    {
        buttonsObj.SetActive(isOn);
    }
}
