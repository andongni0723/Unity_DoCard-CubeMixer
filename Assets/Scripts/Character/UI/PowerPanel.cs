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

    private void Awake()
    {
        character ??= transform.parent.parent.GetComponent<Character>();
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
}
