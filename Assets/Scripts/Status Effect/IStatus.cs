using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatus
{
    void OnEnter(Character character, SkillDetailsSO skillDetails, StatusEffectSO statusDetails);
    void OnStateStart(Character character, GameState newState);
    void OnStateEnd(Character character, GameState cur);
    void OnExit(Character character, SkillDetailsSO skillDetails, StatusEffectSO statusDetails);
    // void OnEnter();
    // void OnStateStart();
    // void OnGameEnd();
    // void OnExit();
}
