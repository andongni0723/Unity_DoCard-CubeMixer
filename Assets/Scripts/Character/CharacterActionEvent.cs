using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterActionEvent : MonoBehaviour
{
    public abstract void CharacterUseSkill();
    public abstract void CharacterSkillHit();
    public abstract void CharacterUsePower();
    public abstract void CharacterHasBeenDamage();
}
