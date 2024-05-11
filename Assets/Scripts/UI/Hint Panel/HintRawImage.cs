using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// ReSharper disable CompareOfFloatsByEqualityOperator

public class HintRawImage : MonoBehaviour
{
    //[Header("Component")]
    [HideInInspector] public RawImage hintImage;
    [HideInInspector] public TextMeshProUGUI hintText;
    
    [Header("Settings")]
    public float hintImageMoveSpeed = 1;
    //[Header("Debug")]

    private void Awake()
    {
        hintImage = GetComponentInChildren<RawImage>();
        hintText = GetComponentInChildren<TextMeshProUGUI>();
        
        hintImage.transform.localScale = Vector3.zero;
        // hintImage.gameObject.SetActive(false);
    }
    // ----------------- Tools -----------------
    public void UpdateDetail(string text)
    {
        hintText.text = text;
        hintText.text = text;
        hintImage.uvRect = new Rect(Vector2.zero, hintImage.uvRect.size);
    }
    
    public void HintImageAnimation(float outScreenSizeTime = 0.3f, float inScreenSizeTime = 0.2f, float waitToDisappearTime = -1)
    {
        hintImage.transform.localScale = new Vector3(1, 0, 1); 
        hintImage.gameObject.SetActive(true);
        
        Sequence sequence = DOTween.Sequence();
        // sequence.Append(hintImage.transform.DOScale(Vector3.one * 1.2f, outScreenSizeTime));
        sequence.Append(hintImage.transform.DOScale(new Vector3(1, 1.2f, 1), outScreenSizeTime)); 
        sequence.Append(hintImage.transform.DOScale(Vector3.one, inScreenSizeTime));

        if(waitToDisappearTime == -1) return; // if waitToDisappearTime == -1, then don't disappear
        sequence.AppendInterval(waitToDisappearTime);
        sequence.Append(hintImage.transform.DOScale( new Vector3(1, 0, 1), outScreenSizeTime));
    }

    /// <summary>
    /// Call by HintButton.cs, when button is clicked
    /// </summary>
    /// <param name="outScreenSizeTime"></param>
    public void CloseImageAnimation(float outScreenSizeTime = 0.1f)
    {
        hintImage.transform.DOScale( new Vector3(1, 0, 1), outScreenSizeTime);
    }
    
    // ----------------- Animation -----------------
    
    private void Update()
    {
        hintImage.uvRect = new Rect(
            new Vector2(hintImage.uvRect.x + hintImageMoveSpeed * Time.deltaTime, 0),
            hintImage.uvRect.size);
    }
}
