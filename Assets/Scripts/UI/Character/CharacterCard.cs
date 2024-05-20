using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CharacterCard : MonoBehaviour
{
    [Header("Component")]
    [SerializeField]private TMP_Text characterName;
    [SerializeField]private Image characterImage;
    [SerializeField]private Slider characterHealthBar;
    // [SerializeField]private GameObject pageImageObj;
    [SerializeField]private GameObject cardObj;
    private Toggle toggle;
    
    [Header("Settings")] 
    public string bindingCharacterID;
    public CharacterDetailsSO characterDetails;

    //[Header("Debug")]
    public Character character;
    private Vector3 defaultScale;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.group = transform.parent.GetComponent<ToggleGroup>();
        defaultScale = transform.localScale;
    }
    
    // ------------------- Event -------------------
    private void OnEnable()
    {
        EventHandler.TurnCharacterStartAction += CloseActive; // close
        EventHandler.LastPlayActionEnd += OnLastPlayActionEnd; // open if is  Action state
        EventHandler.ChangeStateDone += OnChangeStateDone; // open if is  Action state
    }
    
    private void OnDisable()
    {
        EventHandler.TurnCharacterStartAction -= CloseActive;
        EventHandler.LastPlayActionEnd -= OnLastPlayActionEnd;
        EventHandler.ChangeStateDone -= OnChangeStateDone;
    }

    private void OnChangeStateDone(GameState newState)
    {
        if(newState == GameState.ActionState)
            OpenActive();
    }

    private void OnLastPlayActionEnd()
    {
        if(GameManager.Instance.gameStateManager.currentState == GameState.ActionState)
            OpenActive();
    }

    private void CloseActive()
    {
        cardObj.SetActive(false);
    }

    private void OpenActive()
    {
        cardObj.SetActive(true);
    }

    public void InitialUpdateData(bool toggleOn)
    {
        characterName.text = characterDetails.characterName;
        characterImage.sprite = characterDetails.characterSprite;
        characterHealthBar.maxValue = characterDetails.health;
        characterHealthBar.value = characterDetails.health;
        
        toggle.isOn = toggleOn;
        EventHandler.CallCharacterCardPress(characterDetails, bindingCharacterID);
    }
    
    public void OnToggleValueChanged(bool isOn)
    {
        if (isOn) EventHandler.CallCharacterCardPress(characterDetails, bindingCharacterID);
        GetComponent<RectTransform>().DOScale(isOn ? Vector3.one * 0.8f : defaultScale, 0.2f);
    }
}
