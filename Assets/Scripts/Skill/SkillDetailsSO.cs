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
    public string skillENName;
    public string skillDisplayName;
    public string skillID;
    [TextArea]public string skillDescription;
    public bool isSkillAreaCanEnemyOn;
    
    // Skill Use Condition
    [Space(10)][EnumToggleButtons]
    public SkillUseCondition skillUseCondition;
    
    // Power
    [ShowIf("skillUseCondition", SkillUseCondition.Power)]
    public int skillNeedPower;
    
    // Count
    [ShowIf("skillUseCondition", SkillUseCondition.Count)]
    public int needCount;
    [ShowIf("skillUseCondition", SkillUseCondition.Count)]
    public StatusEffectSO countStatusEffectData;

    [ShowIf("skillUseCondition", SkillUseCondition.Count)]
    public bool isUseSkill;

    [ShowIf("skillUseCondition", SkillUseCondition.Count)]
    public bool isSkillHit;

    [ShowIf("skillUseCondition", SkillUseCondition.Count)]
    public bool isUsePower;

    [ShowIf("skillUseCondition", SkillUseCondition.Count)]
    public bool hasBeenDamage;
    
    
    // Skill Effect
    [Space(10)][EnumToggleButtons]
    public SkillButtonType skillType;
 
    // Move
    [ShowIf("skillType", SkillButtonType.Move)]
    [Range(0, 20)]public int moveRange;

    // Attack
    [ShowIf("skillType", SkillButtonType.Attack)]
    public int damage;
    [ShowIf("skillType", SkillButtonType.Attack)] [Min(1)]
    public int attackAimTime;
    [ShowIf("skillType", SkillButtonType.Attack)]
    public bool isDirectionAttack;
    [ShowIf("skillType", SkillButtonType.Attack)]
    public List<SkillAimData> SkillAimDataList;
    
    //Skill
    [ShowIf("skillType", SkillButtonType.Skill)]
    public int skillBuffRound;
    [ShowIf("skillType", SkillButtonType.Skill)]
    public List<StatusEffect> skillEffectList;
}
