using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintButton : MonoBehaviour
{
    [Header("Component")]
    private HintRawImage hintRawImage;
    public Button button;
    //[Header("Settings")]
    //[Header("Debug")]
    
    private void Awake()
    {
        hintRawImage = GetComponentInChildren<HintRawImage>();
        button = GetComponentInChildren<Button>();
        
    }
    //
    // private void Start()
    // {
    //     HintPanelManager.Instance.AnyButtonCallback += () => hintRawImage.CloseImageAnimation(); 
    // }
}
