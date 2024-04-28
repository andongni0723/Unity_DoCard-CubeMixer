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
