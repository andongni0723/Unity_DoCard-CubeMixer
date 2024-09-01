using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonPowerPanel : MonoBehaviour
{
    [Header("Component")]
    private CharacterStatusManager statusManager;
    public GameObject powerPanel;
    private SkillDetailsSO skillDetails;
    public TextMeshProUGUI powerText;
    public Image powerBar;
    //[Header("Settings")]
    //[Header("Debug")]
    private bool isCountSkill;

    // Call by SkillButton.cs
    public void InitialUpdate(SkillDetailsSO data, CharacterStatusManager statusManager)
    {
        this.statusManager = statusManager;
        isCountSkill = data.skillUseCondition == SkillUseCondition.Count;
        skillDetails = data;
        PowerUIUpdate(0, data.needCount);
        powerPanel.SetActive(isCountSkill);
        statusManager.StatusChange += OnStatusChange;

        Debug.Log(data);
    }

    private void OnEnable()
    {
        if (statusManager != null) statusManager.StatusChange += OnStatusChange;
    }
    
    private void OnDisable()
    {
        if (statusManager != null) statusManager.StatusChange -= OnStatusChange;
    }

    private void OnStatusChange(StatusEffect statusEffect, int oldValue, int newValue)
    {
        if(isCountSkill && skillDetails.countStatusEffectData == statusEffect.data)
            PowerUIUpdate(newValue, skillDetails.needCount);

        SetPanelActive(newValue != skillDetails.needCount); // if count is full, close panel
    }

    public void SetPanelActive(bool isActive)
    {
        powerPanel.SetActive(isCountSkill && isActive);
    }

    public void PowerUIUpdate(int currentPower, int maxPower)
    {
        powerText.text = currentPower.ToString();
        powerBar.fillAmount = (float)currentPower / maxPower;
    }
}
