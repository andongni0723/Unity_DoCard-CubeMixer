using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CharacterCard : MonoBehaviour
{
    [Header("Component")]
    public Character character;
    private Button selfButton;
    
    // [Header("Settings")]
    [Space(30)]
    public UnityEvent OnClickToOpenEvent;
    public UnityEvent OnClickToCloseEvent;
    
    //[Header("Debug")]
    private bool isOpen;

    private void Awake()
    {
        selfButton = GetComponent<Button>();
        selfButton.onClick.AddListener(() =>
        {
            isOpen = !isOpen;
            if(isOpen)
                OnClickToOpenEvent?.Invoke();
            else
                OnClickToCloseEvent?.Invoke();
        });
    }
    
    
}
