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
    [SerializeField]private GameObject pageImageObj;
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
        defaultScale = transform.localScale;
    }
    
    public void InitialUpdateData()
    {
        characterName.text = characterDetails.characterName;
        characterImage.sprite = characterDetails.characterSprite;
        characterHealthBar.maxValue = characterDetails.health;
        characterHealthBar.value = characterDetails.health;
        
        toggle.group = transform.parent.GetComponent<ToggleGroup>();
        toggle.isOn = true;
        // EventHandler.CallCharacterCardPress(characterDetails, bindingCharacterID);
    }
    
    public void OnToggleValueChanged(bool isOn)
    {
        if (isOn)
            EventHandler.CallCharacterCardPress(characterDetails, bindingCharacterID);
        
        
        GetComponent<RectTransform>().DOScale(isOn ? Vector3.one * 0.8f : defaultScale, 0.2f);
    }
}
