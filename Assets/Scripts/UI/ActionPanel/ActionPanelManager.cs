using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPanelManager : MonoBehaviour
{
    [Header("Component")]
    public GameObject group;

    [Header("Settings")]
    public GameObject actionUIPrefab;

    //[Header("Debug")]
    
    // -----------------Event-----------------
    private void OnEnable()
    {
        EventHandler.CharacterUseSkill += GenerateActionUI; // Generate action prefab
    }

    private void OnDisable()
    {
        EventHandler.CharacterUseSkill -= GenerateActionUI;
    }

    private void GenerateActionUI(string characterName, SkillDetailsSO skillData)
    {
        var actionUI = Instantiate(actionUIPrefab, group.transform);
        actionUI.GetComponent<ActionUI>().SetActionUI(characterName, skillData);
    }
}
