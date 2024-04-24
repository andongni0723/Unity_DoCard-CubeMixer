using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPanel : MonoBehaviour
{
    [Header("Component")]
    public Character character;
    public GameObject chooseOutlineImage;
    //[Header("Settings")]
    //[Header("Debug")]
    private Camera mainCamera;

    private void Awake()
    {
        character ??= transform.parent.parent.GetComponent<Character>();
        mainCamera = Camera.main;
    }

    private void Start()
    { 
        InitialDisplay(); 
    }

    private void InitialDisplay()
    {
        if(character.team != GameManager.Instance.selfTeam) 
            gameObject.SetActive(false);
    }

    // ----------------- Event -----------------
    
    private void OnEnable()
    {
        EventHandler.CharacterCardPress += OnCharacterCardPress;
    }

    private void OnDisable()
    {
        EventHandler.CharacterCardPress -= OnCharacterCardPress;
    }

    private void OnCharacterCardPress(CharacterDetailsSO details, string ID)
    {
        chooseOutlineImage.SetActive(character.ID == ID);
    }

    // ----------------- Game -----------------
    private void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
    }
}
