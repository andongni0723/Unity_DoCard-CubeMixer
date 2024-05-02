using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    //[Header("Component")]
    //[Header("Settings")]
    //[Header("Debug")]
    public int maxHealth;
    public int currentHealth;
    public int maxPower;
    public int currentPower;
    
    public void InitialUpdateDate(int maxHealth, int maxPower)
    {
        this.maxHealth = maxHealth;
        this.maxPower = maxPower;
        currentHealth = maxHealth;
        currentPower = 0;
    }
}
