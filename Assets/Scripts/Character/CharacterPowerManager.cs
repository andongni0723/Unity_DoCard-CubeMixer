using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterPowerManager : MonoBehaviour
{ 
    [Header("Component")]
    public CharacterHealth characterHealth; // set by Character.cs
    public CharacterStatusManager characterStatus;
    public Character character;
    public PowerPanel powerPanel;
    
    
    //[Header("Settings")]
    //[Header("Debug")]

    // ----------------- Game -----------------
    private void Awake()
    {
        powerPanel ??= transform.GetChild(0).GetChild(0).GetComponent<PowerPanel>();
        character ??= GetComponent<Character>();
        characterHealth ??= GetComponent<CharacterHealth>();
        characterStatus ??= GetComponent<CharacterStatusManager>();
    }
    
    public bool CheckPowerEnough(SkillDetailsSO skillDetails)
    {
        switch (skillDetails.skillUseCondition)
        {
            case SkillUseCondition.Power:
                if (characterHealth.currentPower < skillDetails.skillNeedPower) return false;
                powerPanel.ReadyPowerUI(characterHealth.currentPower,skillDetails.skillNeedPower);
                return true;
            
            case SkillUseCondition.Count:
                return characterStatus.GetStatusCount(skillDetails.countStatusEffectData) >= skillDetails.needCount;

            default:
                throw new ArgumentOutOfRangeException();
        }

        return false;
    }
    
    public void UsePower(int powerCost)
    {
        characterHealth.SetPower(characterHealth.currentPower - powerCost);
        powerPanel.SetPowerUI(characterHealth.currentPower);
        character.CharacterUsePower();
    }

    public void CancelReadyPower()
    {
        powerPanel.CancelReady(characterHealth.currentPower);
    }
}
