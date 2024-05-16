using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunctionButtonManager : MonoBehaviour
{
    //[Header("Component")]
    public Button clearButton;
    public Button playButton;
    public Button doneButton;
    
    //[Header("Settings")]
    //[Header("Debug")]

    private void OnEnable()
    {
        EventHandler.CharacterActionEnd += OnCharacterActionEnd; // Check is action state to set button enable
        EventHandler.TurnCharacterStartAction += CloseAllButtonEnable;
        EventHandler.ButtonCallUseSkillEvent += CloseAllButtonEnable; // set button enable
        EventHandler.LastPlayActionEnd += OpenAllButtonEnable; // set button enable
    }

    private void OnDisable()
    {
        EventHandler.CharacterActionEnd -= OnCharacterActionEnd;
        EventHandler.TurnCharacterStartAction -= CloseAllButtonEnable;
        EventHandler.ButtonCallUseSkillEvent -= CloseAllButtonEnable;
        EventHandler.LastPlayActionEnd -= OpenAllButtonEnable;
    }

    private void OnCharacterActionEnd(bool isOwner)
    {
        if(isOwner && GameManager.Instance.gameStateManager.currentState == GameState.ActionState) 
            OpenAllButtonEnable();
    }

    private void CloseAllButtonEnable()
    {
        clearButton.interactable = false;
        playButton.interactable = false;
        doneButton.interactable = false;
    }

    private void OpenAllButtonEnable()
    {
        clearButton.interactable = true;
        playButton.interactable = true;
        doneButton.interactable = true;
    }

    private void Awake()
    {
        playButton.onClick.AddListener(CloseAllButtonEnable);
        doneButton.onClick.AddListener(CloseAllButtonEnable);
    }
}
