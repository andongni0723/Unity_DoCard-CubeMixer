using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    public string ID;
    public CharacterDetailsSO characterDetails;
    //[Header("Debug")]

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }
    
    public void InitialUpdateData()
    {
        characterName.text = characterDetails.characterName;
        characterImage.sprite = characterDetails.characterSprite;
        characterHealthBar.maxValue = characterDetails.health;
        characterHealthBar.value = characterDetails.health;
        
        toggle.group = transform.parent.GetComponent<ToggleGroup>();
    }
    
    public void OnToggleValueChanged(bool isOn)
    {
        if (isOn)
            EventHandler.CallCharacterCardPress(characterDetails);
        
        pageImageObj.SetActive(isOn);
    }
}
