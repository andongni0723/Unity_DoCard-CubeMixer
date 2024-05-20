using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// ReSharper disable FieldCanBeMadeReadOnly.Local

public class PowerPanel : MonoBehaviour
{
    [Header("Component")]
    public Character character;
    public GameObject chooseOutlineImage;
    public Slider powerSlider;
    public Slider powerInsideSlider;
    public Image powerInsideFillImage;
    public TextMeshProUGUI powerText;

    [Header("Settings")] 
    public Color powerInsideBarColor1;
    public Color powerInsideBarColor2;
    
    //[Header("Debug")]
    private Camera mainCamera;
    // private Sequence textAndInsideImageSequence = DOTween.Sequence();
    Sequence textAndInsideImageSequence;

    private void Awake()
    {
        character ??= transform.parent.parent.GetComponent<Character>();
        mainCamera = Camera.main;

        TextAndInsideImageColorAnimation();
    }
    
    public void InitialDisplay(int startPower)
    {
        if(character.team != GameManager.Instance.selfTeam) 
            gameObject.SetActive(false);
        
        // powerInsideBarColor1 = powerInsideFillImage.color;
        powerSlider.maxValue = startPower;
        powerInsideSlider.maxValue = startPower;
        powerInsideSlider.value = startPower;
        powerSlider.value = startPower; 
        SetPowerUI(startPower);
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
    
    // ----------------- Tools -----------------
    public void SetPowerUI(int currentPower)
    {
        powerSlider.value = currentPower;
        powerInsideSlider.value = currentPower;
        powerText.text = currentPower.ToString();
        PowerInsideBarColorFadeAnimation(false);
    }

    public void ReadyPowerUI(int currentPower, int readyCost)
    {
        powerSlider.value = currentPower;
        powerSlider.value -= readyCost;
        powerText.text = powerSlider.value.ToString(CultureInfo.InvariantCulture);
        PowerInsideBarColorFadeAnimation(true);
    }

    public void CancelReady(int currentPower)
    {
        SetPowerUI(currentPower);
    }


    // ----------------- Game -----------------
    private void PowerInsideBarColorFadeAnimation(bool isOpen)
    {
        if (isOpen)
            textAndInsideImageSequence.Play();
        else
        {
            textAndInsideImageSequence.Pause();
            powerInsideFillImage.color = powerInsideBarColor1;
            powerText.color = powerInsideBarColor1;
        }
    }

    private void TextAndInsideImageColorAnimation()
    {
        textAndInsideImageSequence = DOTween.Sequence();
        textAndInsideImageSequence.SetLoops(-1);
        textAndInsideImageSequence.Append(powerInsideFillImage.DOColor(powerInsideBarColor1, 0f));
        textAndInsideImageSequence.Join(powerText.DOColor(powerInsideBarColor1, 0.5f));
        textAndInsideImageSequence.Append(powerInsideFillImage.DOColor(powerInsideBarColor2, 0.5f));
        textAndInsideImageSequence.Join(powerText.DOColor(powerInsideBarColor2, 0.5f));
        textAndInsideImageSequence.Append(powerInsideFillImage.DOColor(powerInsideBarColor1, 0.5f));
        textAndInsideImageSequence.Join(powerText.DOColor(powerInsideBarColor1, 0.5f));
        textAndInsideImageSequence.Pause();
    }
    
    private void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);

        //TODO: Fix the color fade animation
        // while (isColorFade)
        // {
        
        // }
    }
}
