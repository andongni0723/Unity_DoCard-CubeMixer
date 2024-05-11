using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActionUI : MonoBehaviour
{
    [Header("Component")]
    public TextMeshProUGUI actionCharacterNameText;
    public TextMeshProUGUI actionDisplayNameText;
    
    [Header("Settings")]
    private string actionCharacterName;
    private SkillDetailsSO skillDetailsSO;
    //[Header("Debug")]
    
    // ----------------- Event -----------------
    private void OnEnable()
    {
        EventHandler.CharacterActionClear += DestroySelf;
    }
    private void OnDisable()
    {
        EventHandler.CharacterActionClear -= DestroySelf;
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    // -----------------Tools-----------------
    
    /// <summary>
    /// Call By ActionPanelManager
    /// </summary>
    /// <param name="characterName"></param>
    /// <param name="skillData"></param>
    public void SetActionUI(string characterName, SkillDetailsSO skillData)
    {
        // Set Data
        actionCharacterName = characterName;
        skillDetailsSO = skillData;
        
        // Update UI
        actionCharacterNameText.text = actionCharacterName;
        actionDisplayNameText.text = skillDetailsSO.skillDisplayName;
    }
}
