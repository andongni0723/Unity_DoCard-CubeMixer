using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class HurtText : MonoBehaviour
{
    [Header("Component")]
    private TextMeshProUGUI text;
    
    [Header("Settings")] 
    public Vector2 endPoint;
    public float duration = 1;
    //[Header("Debug")]


    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    
    // ----------------- Tools -----------------
    public void Animaiton(int damage)
    {
        text.text = '-' + damage.ToString();
        
        Sequence hurtUIAnimationSequence = DOTween.Sequence();
        hurtUIAnimationSequence.Append(text.DOFade(1, 0));
        hurtUIAnimationSequence.Append(text.rectTransform.DOAnchorPos(endPoint, duration).SetEase(Ease.OutQuad));
        hurtUIAnimationSequence.Join(text.DOFade(0, duration).SetEase(Ease.InExpo));
        hurtUIAnimationSequence.OnComplete(() => Destroy(gameObject));
    }
}
