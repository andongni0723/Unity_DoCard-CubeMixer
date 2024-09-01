using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
public class CharacterSkillExecutor : Singleton<CharacterSkillExecutor>
{
    // 公開方法執行技能
    public IEnumerator ExecuteSkill(SkillDetailsSO skillDetails, Character character)
    {
        // Initialize
        character.skillTileReturnDataList.Clear();

        switch (skillDetails.skillType)
        {
            case SkillButtonType.Empty:
                Debug.LogError("SkillButtonType is Empty: The skill button is not bind with any skill");
                HintPanelManager.Instance.CallError("SkillButtonType is Empty: The skill button is not bind with any skill");
                break;

            case SkillButtonType.Move:
                yield return CallTileStandAnimation(
                    skillDetails, 
                    character.characterTilePosition,
                    Vector2.zero, 
                    new Vector2(skillDetails.moveRange, skillDetails.moveRange));
                yield return new WaitUntil(() => character.isTileReturn); // Wait Player Choose Tile

                EventHandler.CallCharacterChooseTileRangeDone();
                character.MoveAction(
                    character.skillTileReturnDataList[0].tileGameObject, 
                    character.skillTileReturnDataList[0].targetTilePos);
                break;

            case SkillButtonType.Attack:
                for (int i = 0; i < skillDetails.attackAimTime; i++)
                {
                    yield return CallTileStandAnimation(
                        skillDetails,
                        character.characterTilePosition,
                        skillDetails.SkillAimDataList[i].skillAttackRange, 
                        skillDetails.SkillAimDataList[i].skillCastRange,
                        skillDetails.isDirectionAttack, 
                        i == 0 ? 0.1f : 0f);
                    yield return new WaitUntil(() => character.isTileReturn); // Wait Player Choose Tile

                    character.CharacterUseSkill();
                    EventHandler.CallCharacterChooseTileRangeDone();
                }

                var skillTargetPosList = character.skillTileReturnDataList.
                    Select(data => new Vector2(data.targetTilePos.x, data.targetTilePos.y)).ToList();
                StartCoroutine(character.AttackAction(
                    skillDetails.skillID, 
                    skillDetails.skillType,
                    skillTargetPosList, 
                    true));
                break;

            case SkillButtonType.Skill:
                yield return CallTileStandAnimation(
                    skillDetails,
                    character.characterTilePosition,
                    Vector2.zero, 
                    Vector2.zero);
                yield return new WaitUntil(() => character.isTileReturn); // Wait Player Choose Tile

                character.CharacterUseSkill();
                EventHandler.CallCharacterChooseTileRangeDone();

                skillDetails.skillEffectList.ForEach(effect => 
                    character.characterStatus.AddStatusEffect(skillDetails, effect.data, effect.count));
                Debug.Log("Skill type");

                StartCoroutine(character.AttackAction(
                    skillDetails.skillID, 
                    skillDetails.skillType, 
                    new List<Vector2> { Vector2.zero }, 
                    true));
                break;
        }

        UsePowerOrStatus(skillDetails, character);
        character.characterManager.characterActionRecord.
            AddCharacterActionData(character.ID, skillDetails, character.skillTileReturnDataList);
    }

    private void UsePowerOrStatus(SkillDetailsSO skillDetails, Character character)
    {
        switch (skillDetails.skillUseCondition)
        {
            case SkillUseCondition.Power:
                character.CharacterPower.UsePower(skillDetails.skillNeedPower);
                break;
            case SkillUseCondition.Count:
                character.characterStatus.
                    RemoveStatusEffect(skillDetails, skillDetails.countStatusEffectData, skillDetails.needCount);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private IEnumerator CallTileStandAnimation(SkillDetailsSO data, Vector2 currentTilePosition, Vector2 skillAttackRange, Vector2 maxStandDistance, bool isStrict = false, float duration = 0.1f)
    {
        for (int j = isStrict ? (int)maxStandDistance.y : 0; j <= maxStandDistance.y; j++)
        {
            EventHandler.CallTileUpAnimation(
                data, 
                null,
                skillAttackRange, 
                currentTilePosition, 
                new Vector2(j, j),
                isStrict);
            yield return new WaitForSeconds(duration); // Waiting Animation
        }
    }
}
