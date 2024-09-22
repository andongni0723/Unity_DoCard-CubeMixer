using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("Component")] 
    public Character character;
    
    [Header("Settings")]
    public SkillDetailsSO skillDetails;
    //[Header("Debug")]


    private void Awake()
    {
        if(character == null)
            Debug.LogError("Attacker doesn't know the Character (character is null)");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<CharacterHealth>(out var targetCharacterHealth)) return; // not can attack 
        
        // Check is different team
        if (targetCharacterHealth.isBot || character.team != targetCharacterHealth.character.team)
        {
            targetCharacterHealth.Damage(skillDetails.damage);
            character.CharacterSkillHit();
        }
    }
}
