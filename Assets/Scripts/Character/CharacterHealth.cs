using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    [Header("Component")]
    public HurtUI hurtUI;

    public Character character;
    
    //[Header("Settings")]
    [Header("Debug")]
    public int maxHealth;
    public int maxPower;
    public int currentHealth { get; private set; }
    public int currentPower { get; private set; }

    private void Awake()
    {
        hurtUI ??= GetComponentInChildren<HurtUI>();
        character ??= GetComponent<Character>();
    }

    public void InitialUpdateDate(int maxHealth, int maxPower)
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
        character.HitAnimation();
        
        // the value change must be in fight state
        if(GameManager.Instance.gameStateManager.currentState != GameState.FightState) return;
        currentHealth -= damage;
        EventHandler.CallHealthChange(character, currentHealth, maxHealth);
        if(currentHealth <= 0)
            Dead();
    }

    private void Dead()
    {
        Debug.LogWarning("DEAD");
    }
    
}
