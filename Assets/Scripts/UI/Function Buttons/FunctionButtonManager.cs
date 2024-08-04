using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ButtonCode
{
    CharacterAction,
    ClearButton,
    PlayButton,
    FightButton,
}

public class FunctionButtonManager : Singleton<FunctionButtonManager>
{
    //[Header("Component")]
    public Button clearButton;
    public Button playButton;
    public Button doneButton;
    
    //[Header("Settings")]
    //[Header("Debug")]
    private ButtonCode _currentCode;
    private HashSet<ButtonCode> codesSet = new();
    
    private void OnEnable()
    {
        EventHandler.CharacterActionEnd += (isOwner) =>
        {
            if(isOwner)  CallButtonEnableEvent(ButtonCode.CharacterAction);
        }; 
        
        EventHandler.LastPlayActionEnd += () => CallButtonEnableEvent(ButtonCode.PlayButton); 
        EventHandler.ChangeStateDone += _ => CallButtonEnableEvent(ButtonCode.FightButton);
        EventHandler.HitPanelAnyButtonPress += () => CallButtonEnableEvent(ButtonCode.ClearButton);
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

    // Tools
    public void CallButtonDisableEvent(ButtonCode code)
    {
        _currentCode = code;
        codesSet.Add(code);
        if(codesSet.Count == 1)
            CloseAllButtonEnable();
    }

    /// <summary>
    /// Use Code Find and Remove code from Set, if set is empty, Open All Button Enable
    /// </summary>
    /// <param name="code"></param>
    /// <returns>has found</returns>
    private bool CallButtonEnableEvent(ButtonCode code)
    {
        if (!codesSet.Contains(code)) return false; // not found

        // found
        codesSet.Remove(code);
        if (codesSet.Count == 0)
            OpenAllButtonEnable();
        
        return true;
    }
    private void CallButtonEnableEvent(ButtonCode code1, ButtonCode code2)
    {
        if (!CallButtonEnableEvent(code1))
            CallButtonEnableEvent(code2);
    } 
    private void CallButtonEnableEvent(ButtonCode[] codes)
    {
        foreach (var code in codes)
        {
            if(CallButtonEnableEvent(code))
                break;
        }
    }
}
