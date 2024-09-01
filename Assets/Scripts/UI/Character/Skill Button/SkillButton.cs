using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour, IPointerEnterHandler ,IPointerExitHandler, IStatus
{
    [Header("Component")]
    protected Transform handPanelTransform;
    protected CharacterSkillButtonsGroup parentManager;
    protected Button button;
    protected Image backgroundImage;
    [SerializeField]protected Image spriteRenderer;
    protected SkillDescription skillDescription;
    public SkillButtonPowerPanel skillPowerPanel;

    [Header("Settings")]
    public SkillDetailsSO skillDetails;
    public bool isSpecialSkill;
    
    public UnityEvent OnPointerEnterAction;
    public UnityEvent OnPointerExitAction;

    
    [Header("Debug")]
    public bool isOn;
    
    private void Awake()
    {
        button = GetComponent<Button>();
        skillDescription = GetComponentInChildren<SkillDescription>();
        skillPowerPanel = GetComponentInChildren<SkillButtonPowerPanel>();
        button.interactable = false;
        button.onClick.AddListener(ClickAction);
    }

    private void Start()
    {
        OnPointerEnterAction.AddListener(() => skillPowerPanel.SetPanelActive(false));
        OnPointerExitAction.AddListener(() => skillPowerPanel.SetPanelActive(true));
    }

    public virtual void InitialUpdate(CharacterSkillButtonsGroup parent)
    {
        button.interactable = true;
        parentManager = parent;
        spriteRenderer.sprite = skillDetails.skillSprite;
        skillDescription.InitialUpdate(skillDetails);
        skillPowerPanel.InitialUpdate(skillDetails, parentManager.character.characterStatus);
        gameObject.SetActive(!isSpecialSkill); // Special skill is not show
    }

    private void OnEnable()
    {
        EventHandler.CharacterActionEnd += OnCharacterCancelMove; // isOn to false;
    }
    private void OnDisable()
    {
        EventHandler.CharacterActionEnd -= OnCharacterCancelMove; 
    }

    /// <summary>
    /// New this method by child class
    /// </summary>
    /// <param name="character"></param>
    /// <param name="skillDetailsSo"></param>
    /// <param name="statusEffectSo"></param>
    protected virtual void OnCharacterChangeSkillActive(Character character, SkillDetailsSO skillDetailsSo,
        StatusEffectSO statusEffectSo, List<string> enableSkillIDList, List<string> disableSkillIDList)
    {
        if (parentManager.character != character || skillDetails != skillDetailsSo) return;
    }

    private void OnCharacterCancelMove(bool isOwner)
    {
        isOn = false;
    }
    
    // ----------------- Pointer -----------------
    private void ClickAction()
    {
        EventHandler.CallCharacterActionEnd(true);
        
        if(isOn) return; // Prevent double click
        isOn = true;            
        parentManager.character.ButtonCallUseSkill(skillDetails); 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnterAction?.Invoke();
        spriteRenderer.DOColor(Color.black, 0.2f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        OnPointerExitAction?.Invoke();
        spriteRenderer.DOColor(Color.white, 0.2f);
    }
    
    // ----------------- Status -----------------

    public void OnEnter(Character character, SkillDetailsSO skillDetails, StatusEffectSO statusDetails)
    {
        // Open new skill
        OnCharacterChangeSkillActive(character, skillDetails, statusDetails,
            statusDetails.enableSkillIDList, statusDetails.disableSkillIDList);
    }

    public void OnStateStart(Character character, GameState newState)
    {
        throw new NotImplementedException();
    }

    public void OnStateEnd(Character character, GameState cur)
    {
        throw new NotImplementedException();
    }

    public void OnExit(Character character, SkillDetailsSO skillDetails, StatusEffectSO statusDetails)
    {
        // Turn back to old skill
        OnCharacterChangeSkillActive(character, skillDetails, statusDetails,
            statusDetails.disableSkillIDList, statusDetails.enableSkillIDList);
    }
}
