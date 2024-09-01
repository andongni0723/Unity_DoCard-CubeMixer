using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkStatus : BaseStatus 
{
    //[Header("Component")]
    //[Header("Settings")]
    //[Header("Debug")]

    public new void OnEnter(Character character, SkillDetailsSO skillDetails, StatusEffectSO statusDetails)
    {
        // EventHandler.CallCharacterAddStatusEffect(character, skillDetails, statusDetails);
    }

    public new void OnStateStart(Character character, GameState cur)
    {
        throw new System.NotImplementedException();
    }

    public new void OnGameEnd(Character character, GameState cur)
    {
        throw new System.NotImplementedException();
    }

    public new void OnExit(Character character, SkillDetailsSO skillDetails, StatusEffectSO statusDetails)
    {
        throw new System.NotImplementedException();
    }
}
