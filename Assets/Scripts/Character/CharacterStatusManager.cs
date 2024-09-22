using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class StatusEffect
{
    public SkillDetailsSO statusFrom;
    public StatusEffectSO data;
    public int count;

    public StatusEffect(SkillDetailsSO statusFrom, StatusEffectSO data, int count)
    {
        this.statusFrom = statusFrom;
        this.data = data;
        this.count = count;
    }
}

public class CharacterStatusManager : MonoBehaviour, IStatus
{
    //[Header("Component")]
    private Character character;
    
    //[Header("Settings")]
    [Header("Debug")]
    [ReadOnly][SerializeField]private List<StatusEffect> statusList = new();
    
    //[Header("Event")] 
    public Action<StatusEffect, int, int> StatusChange;
    public void CallStatusChange(StatusEffect statusEffect, int oldValue, int newValue) =>
        StatusChange?.Invoke(statusEffect, oldValue, newValue);
    
    private void Awake()
    {
        character = GetComponent<Character>();
    }

    private void OnEnable()
    {
        EventHandler.ChangeStateDone += OnChangeStateDone;
    }
    
    private void OnDisable()
    {
        EventHandler.ChangeStateDone -= OnChangeStateDone;
    }
    
    private void OnChangeStateDone(GameState newState) => OnStateStart(character, newState);

    // ----------------- Tools -----------------
    public void SetStatusList(List<StatusEffect> statusList)
    {
        // this.statusList.ForEach(s => s.data.script.OnExit(character, s.statusFrom, s.data));
        this.statusList.ForEach(s =>
        {
            OnExit(character, s.statusFrom, s.data);
            CallStatusChange(s, s.count, 0);
        });
        this.statusList = new(statusList); 
        this.statusList.ForEach(s =>
        {
            OnEnter(character, s.statusFrom, s.data);
            CallStatusChange(s, 0, s.count);
        });
    }
    public List<StatusEffect> GetStatusList() => statusList;
    
    /// <summary>
    /// Adds a status effect to the character. If the status effect already exists, increments the count.
    /// </summary>
    /// <param name="skillDetails">The skill that applies this status effect.</param>
    /// <param name="statusEffect">The status effect data.</param>
    /// <param name="count">The initial count of the status effect.</param>
    public void AddStatusEffect(SkillDetailsSO skillDetails, StatusEffectSO statusEffect, int count)
    {
        var status = statusList.FirstOrDefault(x => x.data == statusEffect);
        if (status != null)
        {
            if (statusEffect.maxHaveCountType == EffectMaxHaveCountType.Limit && 
                status.count >= statusEffect.maxHaveCount) return;
            
            CallStatusChange(status, status.count, status.count + count);
            status.count += count;
        }
        else
        {
            var newStatus = new StatusEffect(skillDetails, statusEffect, count);
            statusList.Add(newStatus);
            OnEnter(character, skillDetails, statusEffect); // 只有在新的狀態效果時才調用
            CallStatusChange(newStatus, 0, count);
        }
    }
    
    /// <summary>
    /// Removes a status effect from the character. 
    /// Decreases the count of the specified status effect, and if the count reaches zero, the status effect is completely removed.
    /// Calls the OnExit method when the status effect is removed.
    /// </summary>
    /// <param name="skillDetails">The skill that applied the status effect.</param>
    /// <param name="statusEffect">The status effect to be removed.</param>
    /// <param name="removeCount">The number of times the status effect should be decremented.</param>
    public void RemoveStatusEffect(SkillDetailsSO skillDetails, StatusEffectSO statusEffect, int removeCount)
    {
        var status = statusList.FirstOrDefault(x => x.data == statusEffect);
        if (status == null) return;
    
        var newCount = Math.Max(status.count - removeCount, 0);
        CallStatusChange(status, status.count, newCount);
        status.count = newCount;
        
        if (status.count > 0) return;
        // Effect is removed
        statusList.Remove(status);
        OnExit(character, skillDetails, statusEffect); // 只有當狀態效果完全移除時才調用
    }
    /// <summary>
    /// Removes a status effect from the character.  
    /// </summary>
    /// <param name="skillDetails"></param>
    /// <param name="statusEffect"></param>
    public void RemoveStatusEffect(SkillDetailsSO skillDetails, StatusEffectSO statusEffect)
    {
        RemoveStatusEffect(skillDetails, statusEffect, GetStatusCount(statusEffect));
    }

    public int GetStatusCount(StatusEffectSO statusEffect)
    {
        return !statusList.Exists(x => x.data == statusEffect) ? 0 : // not found 
            statusList.Find(x => x.data == statusEffect).count;
    }
    
    private MultipleGameState ConvertToMultipleGameState(GameState gameState)
    {
        return (MultipleGameState)(1 << (int)gameState);
    }
    
    // ----------------- Status -----------------

    public void OnEnter(Character character, SkillDetailsSO skillDetails, StatusEffectSO statusDetails)
    {
        switch (statusDetails.StatusType)
        {
            case StatusType.None:
                break;
            case StatusType.SkillChange:
                this.character.characterSkillButtonsGroup
                    .UseSkillIDToSkillButton(skillDetails.skillID)
                    .OnEnter(character, skillDetails, statusDetails);
                // EventHandler.CallCharacterAddStatusEffect(character, skillDetails, statusDetails);
                break;
            
            default:
                Debug.LogWarning($"Unknown StatusType: {statusDetails.StatusType}");
                break;

        }
    }
    
    public void OnStateStart(Character character, GameState newState)
    {
        var currentMultGameState = ConvertToMultipleGameState(newState);
        
        // statusList.ForEach(s =>
        // {
        //     switch (s.data.disappearType)
        //     {
        //         case StatusEffectDisappearType.None:
        //             break;
        //         case StatusEffectDisappearType.Event:
        //             if ((s.data.DisappearWhenGameState & currentMultGameState) != 0)
        //             {
        //                 RemoveStatusEffect(s.statusFrom, s.data, 1);
        //             }
        //             break;
        //         default:
        //             throw new ArgumentOutOfRangeException();
        //     }
        // });

        for (int i = 0; i < statusList.Count; i++)
        {
            switch (statusList[i].data.disappearType)
            {
                case StatusEffectDisappearType.None:
                    break;
                
                case StatusEffectDisappearType.Event:
                    if ((statusList[i].data.DisappearWhenGameState & currentMultGameState) != 0)
                    {
                        RemoveStatusEffect(statusList[i].statusFrom, statusList[i].data, 1);
                    }
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            } 
        }
    }

    public void OnStateEnd(Character character, GameState cur)
    {
        throw new System.NotImplementedException();
    }

    public void OnExit(Character character, SkillDetailsSO skillDetails, StatusEffectSO statusDetails)
    {
        switch (statusDetails.StatusType)
        {
            case StatusType.None:
                break;
            case StatusType.SkillChange:
                this.character.characterSkillButtonsGroup
                    .UseSkillIDToSkillButton(skillDetails.skillID)
                    .OnExit(character, skillDetails, statusDetails);
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
