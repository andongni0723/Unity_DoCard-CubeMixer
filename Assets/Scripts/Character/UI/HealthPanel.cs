using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthPanel : MonoBehaviour
{
    //[Header("Component")]
    //[Header("Settings")]
    //[Header("Debug")]
    
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
    // Sequence textAndInsideImageSequence;
    
    private void Awake()
    {
        mainCamera = Camera.main;
        character ??= GetComponentInParent<Character>();
    }
    
    public void InitialDisplay(int startHealth)
    {
        gameObject.SetActive(character.team != GameManager.Instance.selfTeam);
        
        powerSlider.maxValue = startHealth;
        powerInsideSlider.maxValue = startHealth;
        powerInsideSlider.value = startHealth;
        powerSlider.value = startHealth; 
        SetHealthUI(startHealth);
    }

    public void SetHealthUI(int startHealth)
    {
        powerSlider.value = startHealth;
        powerInsideSlider.value = startHealth;
        powerText.text = startHealth.ToString();
    }
}
