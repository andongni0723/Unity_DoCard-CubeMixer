using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterPowerManager : MonoBehaviour
{ 
    [Header("Component")]
    public CharacterHealth characterHealth; // set by Character.cs
    public PowerPanel powerPanel;
    
    //[Header("Settings")]
    //[Header("Debug")]
    
    // ----------------- Event -----------------
    // private void OnEnable()
    // {
    //     EventHandler.CharacterActionEnd += OnCharacterActionEnd;
    // }
    //
    // private void OnDisable()
    // {
    //     EventHandler.CharacterActionEnd -= OnCharacterActionEnd;
    // }
    //
    // private void OnCharacterActionEnd(bool isOwner)
    // {
    //     if(isOwner)
    //         CancelReadyPower();
    // }

    // ----------------- Game -----------------
    private void Awake()
    {
        powerPanel ??= transform.GetChild(0).GetChild(0).GetComponent<PowerPanel>();
    }
    
    public bool CheckPowerEnough(int powerCost)
    {
        if (characterHealth.currentPower < powerCost) return false;
        
        powerPanel.ReadyPowerUI(characterHealth.currentPower, powerCost);
        return true;
    }
    
    public void UsePower(int powerCost)
    {
        characterHealth.SetPower(characterHealth.currentPower - powerCost);
        powerPanel.SetPowerUI(characterHealth.currentPower);
    }

    public void CancelReadyPower()
    {
        powerPanel.CancelReady(characterHealth.currentPower);
    }
}
