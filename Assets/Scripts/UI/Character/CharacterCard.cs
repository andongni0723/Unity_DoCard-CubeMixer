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
    [SerializeField] private GameObject cardObj;
    [SerializeField] private TMP_Text characterName;
    [SerializeField] private Image characterImage;
    [SerializeField] private Slider characterHealthBar;
    [SerializeField] private Slider characterPowerBar;
    
    [Space(15)]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI powerText;


    // [SerializeField]private GameObject pageImageObj;
    private Toggle toggle;
    
    [Header("Settings")] 
    public string bindingCharacterID;
    public CharacterDetailsSO characterDetails;

    //[Header("Debug")]
    public Character character; // set by CharacterCardManager
    private Vector3 defaultScale;
    private Vector3 startPos;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.group = transform.parent.GetComponent<ToggleGroup>();
        defaultScale = transform.localScale;
        startPos = cardObj.transform.localPosition;
    }
    
    // ------------------- Event -------------------
    private void OnEnable()
    {
        EventHandler.TurnCharacterStartAction += CloseActive; // close
        EventHandler.LastPlayActionEnd += OnLastPlayActionEnd; // open if is  Action state
        EventHandler.ChangeStateDone += OnChangeStateDone; // open if is  Action state
        EventHandler.HealthChange += OnHealthChange;
        EventHandler.PowerChange += OnPowerChange;
        EventHandler.CharacterDead += OnCharacterDead; // close
    }

    private void OnDisable()
    {
        EventHandler.TurnCharacterStartAction -= CloseActive;
        EventHandler.LastPlayActionEnd -= OnLastPlayActionEnd;
        EventHandler.ChangeStateDone -= OnChangeStateDone;
        EventHandler.HealthChange -= OnHealthChange;
        EventHandler.PowerChange -= OnPowerChange;
        EventHandler.CharacterDead += OnCharacterDead;

    }

    private void OnPowerChange(Character target, int newValue, int maxPower)
    {
        if(target != character) return; // target is not this character
        
        characterPowerBar.maxValue = maxPower;
        powerText.text = $"{newValue}/{maxPower}";
        
        characterPowerBar.DOValue(newValue, 0.5f);
    }

    private void OnHealthChange(Character target, int newValue, int maxHealth)
    {
        if(target != character) return; // target is not this character

        characterHealthBar.maxValue = maxHealth; 
        healthText.text = $"{newValue}/{maxHealth}";
        
        characterHealthBar.DOValue(newValue, 0.5f);
        cardObj.transform.DOPunchPosition(Vector3.right * 40, 0.5f)
            .OnComplete(() => cardObj.transform.localPosition = startPos);
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
        // cardObj.SetActive(false);
        toggle.interactable = false;
    }

    private void OpenActive()
    {
        // cardObj.SetActive(true);
        toggle.interactable = true;
    }
    
    private void OnCharacterDead(Character character)
    {
        if(this.character == character)
            gameObject.SetActive(false);
    }
    
    // ------------- Game ----------------

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
