using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class SkillAimData
{
    public Vector2 skillAttackRange;
    public Vector2 skillCastRange;
}


[CreateAssetMenu(menuName = "Skill/SkillDetailsSO")]
public class SkillDetailsSO : ScriptableObject
{
   
    [PreviewField(100, ObjectFieldAlignment.Left), HideLabel]
    public Sprite skillSprite;
    
    [Header("Skill Editor")]
    public string skillName;
    public string skillID;
    public string skillDescription;
    public bool isSkillAreaCanEnemyOn;
    
    [Space(10)][EnumToggleButtons]
    public SkillButtonType skillType;
 
    [ShowIf("skillType", SkillButtonType.Move)]
    [Range(0, 20)]public int moveRange;

    [ShowIf("skillType", SkillButtonType.Attack)] [Min(1)]
    public int attackAimTime;

    [ShowIf("skillType", SkillButtonType.Attack)]
    public List<SkillAimData> SkillAimDataList;
}
