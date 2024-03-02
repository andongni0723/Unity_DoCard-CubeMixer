using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/SkillDetailsSO")]
public class SkillDetailsSO : ScriptableObject
{
    public SkillButtonType skillType;
   
    
    [Header("Skill Editor")]
    public string skillName;
    public string skillDescription;
    [Range(0, 20)]public int range;
    
    // [ShowIf("skillType", SkillButtonType.Move)]
    //
    // [ShowIf("skillType", SkillButtonType.Attack)]
}
