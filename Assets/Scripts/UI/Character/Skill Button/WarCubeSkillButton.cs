using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarCubeSkillButton : SkillButton
{
    //[Header("Component")]
    //[Header("Settings")]
    //[Header("Debug")]

    protected override void OnCharacterChangeSkillActive(Character character, SkillDetailsSO skillDetailsSo,
        StatusEffectSO statusEffectSo, List<string> enableSkillIDList, List<string> disableSkillIDList)
    {
        // base.OnCharacterUseSpecialSkill(character, skillDetailsSo, statusEffectSo);
        if (parentManager.character != character || skillDetails != skillDetailsSo) return;
        if(!statusEffectSo.effectID.Equals("dark")) return;

        Debug.Log("Dark");
        
        // enable or disable skill button
        enableSkillIDList.ForEach(id => 
            parentManager.UseSkillIDToSkillButton(id).gameObject.SetActive(true));
        disableSkillIDList.ForEach(id => 
            parentManager.UseSkillIDToSkillButton(id).gameObject.SetActive(false));
    }
}
