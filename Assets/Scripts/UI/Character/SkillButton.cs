using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour, IPointerEnterHandler ,IPointerExitHandler
{
    [Header("Component")]
    private Transform handPanelTransform;
    private CharacterSkillButtonsGroup parentManager;
    private Button button;
    private Image backgroundImage;
    [SerializeField]private Image spriteRenderer;
    private SkillDescription skillDescription;

    [Header("Settings")]
    public SkillDetailsSO skillDetails;

    public bool isOn;
    
    //[Header("Debug")]

    private void Awake()
    {
        button = GetComponent<Button>();
        skillDescription = GetComponentInChildren<SkillDescription>();
        button.interactable = false;
        button.onClick.AddListener(ClickAction);
    }
    
    public void InitialUpdate(CharacterSkillButtonsGroup parent)
    {
        button.interactable = true;
        parentManager = parent;
        spriteRenderer.sprite = skillDetails.skillSprite;
        
        skillDescription.InitialUpdate(skillDetails);
    }

    private void OnEnable()
    {
        EventHandler.CharacterActionEnd += OnCharacterCancelMove;
    }
    private void OnDisable()
    {
        EventHandler.CharacterActionEnd -= OnCharacterCancelMove; // isOn to false
    }

    private void OnCharacterCancelMove(bool isOwner)
    {
        isOn = false;
    }
    
    private void ClickAction()
    {
        EventHandler.CallCharacterActionEnd(true);
        
        if(isOn) return; // Prevent double click
        isOn = true;            
        parentManager.character.ButtonCallUseSkill(skillDetails); 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        spriteRenderer.DOColor(Color.black, 0.2f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        spriteRenderer.DOColor(Color.white, 0.2f);
    }
    
}
