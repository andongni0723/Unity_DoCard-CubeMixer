using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[Serializable]
public class HintPanelTheme
{
    public Color hintImageColor;
    public Color hintTextColor;
    public float hintImageMoveSpeed;
    public float fontSize = 160;
}
public class HintPanelManager : Singleton<HintPanelManager>
{
    [Header("Component")] 
    public HintRawImage mainPanel;
    public HintRawImage yesButtonImage;
    public HintRawImage noButtonImage;
    public HintButton yesButton;
    public HintButton noButton;

    [Header("Settings")] 
    public float hintImageMoveSpeed = 1;

    [Header("Theme Setting")] 
    public HintPanelTheme hintTheme;
    public HintPanelTheme warningTheme;
    public HintPanelTheme errorTheme;

    [Header("Animation Setting")] 
    public float outScreenSizeTime = 0.3f;
    public float inScreenSizeTime = 0.2f;
    public float waitToDisappearTime = 1.5f;
    //[Header("Debug")]

    public Action AnyButtonCallback; // call when any button is clicked
    public Action YesButtonCallback; // call when yes button is clicked

    private void Start()
    {
        yesButton.button.onClick.AddListener(() =>
        {
            YesButtonCallback?.Invoke();
            AnyButtonCallback?.Invoke();
            YesButtonCallback = null; // Clear the caller callback 
        });
        
        noButton.button.onClick.AddListener(() =>
        {
            AnyButtonCallback?.Invoke();
        });
        
        AnyButtonCallback += () =>
        {
            EventHandler.CallHitPanelAnyButtonPress();
            mainPanel.CloseImageAnimation();
            yesButtonImage.CloseImageAnimation();
            noButtonImage.CloseImageAnimation();
        };
    }

    // ----------------- Tools -----------------
    
    public void CallHint(string text)
    {
        ChangeTheme(hintTheme);
        mainPanel.UpdateDetail(text);
        mainPanel.HintImageAnimation(outScreenSizeTime, inScreenSizeTime, waitToDisappearTime);
    }
    public void CallWarning(string text)
    {
        ChangeTheme(warningTheme);
        mainPanel.UpdateDetail(text);
        mainPanel.HintImageAnimation(outScreenSizeTime, inScreenSizeTime, waitToDisappearTime);    
    }
    public void CallChooseBox(string text, Action callback)
    {
        ChangeTheme(warningTheme);
        mainPanel.UpdateDetail(text);

        YesButtonCallback += callback;
        // hint box don't disappear
        mainPanel.HintImageAnimation(outScreenSizeTime, inScreenSizeTime);
        yesButtonImage.HintImageAnimation(outScreenSizeTime, inScreenSizeTime);
        noButtonImage.HintImageAnimation(outScreenSizeTime, inScreenSizeTime);
    }
    
    public void CallError(string text)
    {
        ChangeTheme(errorTheme);
        mainPanel.UpdateDetail(text);
        mainPanel.HintImageAnimation(outScreenSizeTime, inScreenSizeTime, waitToDisappearTime);    
    }
    
    // ----------------- Animation -----------------

    private void ChangeTheme(HintPanelTheme newTheme)
    {
        mainPanel.hintImage.color = newTheme.hintImageColor;
        mainPanel.hintText.color = newTheme.hintTextColor;
        mainPanel.hintText.fontSize = newTheme.fontSize;
        hintImageMoveSpeed = newTheme.hintImageMoveSpeed; 
        
    }
}
