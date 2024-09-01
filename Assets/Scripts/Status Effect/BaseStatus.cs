using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStatus : MonoBehaviour
{
    public StatusEffectSO details { get; set; }

    public void OnEnter(Character character, SkillDetailsSO skillDetails, StatusEffectSO statusDetails)
    {
        throw new System.NotImplementedException();
    }

    public void OnStateStart(Character character, GameState cur)
    {
        throw new System.NotImplementedException();
    }

    public void OnGameEnd(Character character, GameState cur)
    {
        throw new System.NotImplementedException();
    }

    public void OnExit(Character character, SkillDetailsSO skillDetails, StatusEffectSO statusDetails)
    {
        throw new System.NotImplementedException();
    }
}
