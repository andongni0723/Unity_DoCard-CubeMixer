using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionGroupController : MonoBehaviour
{
    [Header("Component")]
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    private VerticalLayoutGroup layoutGroup;
    public RectTransform actionUIPrefab;
    
    
    //[Header("Settings")]
    //[Header("Debug")]
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        layoutGroup = GetComponent<VerticalLayoutGroup>();
        parentRectTransform = transform.parent.GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        EventHandler.CharacterUseSkill += SetRectTransformHeight; // setting transform height
    }

    private void OnDisable()
    {
        EventHandler.CharacterUseSkill -= SetRectTransformHeight;
    }

    private void SetRectTransformHeight(string arg1, SkillDetailsSO arg2)
    {
        float newHeight = Mathf.Max(
            (actionUIPrefab.sizeDelta.y + layoutGroup.spacing) * rectTransform.childCount + layoutGroup.spacing, 
            parentRectTransform.sizeDelta.y);
        
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newHeight);
    }

    
    
    
}
