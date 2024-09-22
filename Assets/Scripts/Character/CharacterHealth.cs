using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    [Header("Component")]
    public HurtUI hurtUI;

    public Character character;

    [BoxGroup("Extra Setting")]
    public bool isBot;
    
    [Header("Debug")]
    public int maxHealth;
    public int maxPower;
    
    [FoldoutGroup("Debug")][field: SerializeReference]public int currentHealth { get; private set; }
    [FoldoutGroup("Debug")][field: SerializeReference]public int currentPower { get; private set; }
    
    private void Awake()
    {
        hurtUI ??= GetComponentInChildren<HurtUI>();
        
        if(!isBot)
            character ??= GetComponent<Character>();
    }

    public void InitialUpdateData(int maxHealth, int maxPower)
    {
        this.maxHealth = maxHealth;
        this.maxPower = maxPower;
        currentHealth = maxHealth;
        currentPower = maxPower;
    }
    
    // ----------------- Tools -----------------
    
    public void SetPower(int targetValue)
    {
        currentPower = targetValue;
        EventHandler.CallPowerChange(character, currentPower, maxPower);
    }

    public void SetHealth(int targetValue)
    {
        currentHealth = targetValue;
        EventHandler.CallHealthChange(character, currentHealth, maxHealth);
    }
    
    public void PowerBackToStart()
    {
        currentPower = maxPower;
    }

    public void Damage(int damage)
    {
        hurtUI.CallHurtTextAnimation(damage);

        if (isBot) return;

        character.HitAnimation();
        // the value change must be in fight state
        if(GameManager.Instance.gameStateManager.currentState != GameState.FightState) return;
        currentHealth -= damage;
        character.CharacterHasBeenDamage();
        EventHandler.CallHealthChange(character, currentHealth, maxHealth);
        if(currentHealth <= 0)
            Dead();
    }

    private void Dead()
    {
        Debug.LogWarning("DEAD");
        EventHandler.CallCharacterDead(character);
        gameObject.SetActive(false);
    }
    
}
