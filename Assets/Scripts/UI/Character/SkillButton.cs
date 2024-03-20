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
    private CharacterCardManager parentCharacterCard;
    private Button button;
    private Image backgroundImage;

    [Header("Settings")]
    public SkillDetailsSO skillDetails;
    
    //[Header("Debug")]

    private void Awake()
    {
        handPanelTransform = GameObject.FindWithTag("HandPanel").transform;
        parentCharacterCard = GetComponentInParent<CharacterCardManager>();
        backgroundImage = GetComponentInChildren<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(ClickAction);
    }
    
    private void Start()
    {
        backgroundImage.gameObject.SetActive(false);
    }

    // Remember to add the method to CharacterCard in the inspector 
    public void ParentClickMoveAnimation()
    {
        transform.SetParent(handPanelTransform);
        transform.SetSiblingIndex(parentCharacterCard.transform.GetSiblingIndex());
        backgroundImage.gameObject.SetActive(true);
        
        // Animation
        Sequence sequence = DOTween.Sequence();
        sequence.Append(backgroundImage.DOFade(1, 0.5f).From(0)); // color
        sequence.Join(backgroundImage.transform.DOMove(transform.position, 0.5f)
            .From(parentCharacterCard.transform.position)); // move

    }

    public void ParentClickToClose()
    {
        transform.SetParent(parentCharacterCard.transform);
        backgroundImage.gameObject.SetActive(false);
    }
    
    private void ClickAction()
    {
        // parentCharacterCard.character.ButtonCallUseSkill(skillDetails);
    }
}
