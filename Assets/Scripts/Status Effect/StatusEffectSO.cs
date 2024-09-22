using System.Collections.Generic;
using AYellowpaper;
using NUnit.Framework;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "StatusEffect/StatusEffectSO")]
public class StatusEffectSO : ScriptableObject
{
    [PreviewField(100, ObjectFieldAlignment.Left), HideLabel]
    public Sprite effectIcon;

    public string effectName;
    public string effectID;
    [TextArea] public string effectDescription;

    // Effect Action Setting
    
    // Max have count
    [Space(10)] [EnumToggleButtons] public EffectMaxHaveCountType maxHaveCountType;
    
    [ShowIf("maxHaveCountType", EffectMaxHaveCountType.Limit)]
    public int maxHaveCount;

    // Effect
    [Space(10)] [EnumToggleButtons] public EffectType effectType;

    // Status
    [Space(10)] [EnumToggleButtons] public StatusType StatusType;

    // Skill Change
    [ShowIf("StatusType", StatusType.SkillChange)]
    public List<string> enableSkillIDList;

    [ShowIf("StatusType", StatusType.SkillChange)]
    public List<string> disableSkillIDList;

    // Status Disappear 
    [Space(10)] [EnumToggleButtons] public StatusEffectDisappearType disappearType;

    [ShowIf("disappearType", StatusEffectDisappearType.Event)]
    public MultipleGameState DisappearWhenGameState;

    // [Space(15)] [InfoBox("This field must be a class that implements IStatus interface.", InfoMessageType.Warning)]
    // public Object effectIStatus;
    // // public BaseStatus script;
    // public IStatus script => effectIStatus as IStatus;


}