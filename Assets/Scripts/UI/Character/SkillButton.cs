using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    //[Header("Component")]
    private Transform handPanelTransform;
    private CharacterSkillButtonsGroup parentManager;
    private Button button;
    private Image backgroundImage;
    [SerializeField]private Image spriteRenderer;

    [Header("Settings")]
    public SkillDetailsSO skillDetails;

    public bool isOn;
    
    //[Header("Debug")]

    private void Awake()
    {
        // handPanelTransform = GameObject.FindWithTag("HandPanel").transform;
        // parentManager = GetComponentInParent<CharacterSkillButtonsGroup>();
        // backgroundImage = GetComponentInChildren<Image>();
        button = GetComponent<Button>();
        button.interactable = false;
        button.onClick.AddListener(ClickAction);
    }
    
    public void InitialUpdate(CharacterSkillButtonsGroup parent)
    {
        button.interactable = true;
        parentManager = parent;
        spriteRenderer.sprite = skillDetails.skillSprite;
    }

    private void OnEnable()
    {
        EventHandler.CharacterMoveEnd += OnCharacterCancelMove;
    }
    private void OnDisable()
    {
        EventHandler.CharacterMoveEnd -= OnCharacterCancelMove; // isOn to false
    }

    private void OnCharacterCancelMove()
    {
        isOn = false;
    }


    // Remember to add the method to CharacterCard in the inspector 
    public void ParentClickMoveAnimation()
    {
        transform.SetParent(handPanelTransform);
        transform.SetSiblingIndex(parentManager.transform.GetSiblingIndex());
        backgroundImage.gameObject.SetActive(true);
        
        // Animation
        Sequence sequence = DOTween.Sequence();
        sequence.Append(backgroundImage.DOFade(1, 0.5f).From(0)); // color
        sequence.Join(backgroundImage.transform.DOMove(transform.position, 0.5f)
            .From(parentManager.transform.position)); // move

    }

    public void ParentClickToClose()
    {
        transform.SetParent(parentManager.transform);
        backgroundImage.gameObject.SetActive(false);
    }
    
    private void ClickAction()
    {
        EventHandler.CallCharacterMoveEnd();
        
        if(isOn)
            EventHandler.CallCharacterMoveEnd();
        else
        {
            isOn = true;            
            parentManager.characterGameData.characterObject.GetComponent<Character>().ButtonCallUseSkill(skillDetails);
        }
    }
}
