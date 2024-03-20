using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Character/CharacterDetailsSO")]
public class CharacterDetailsSO : ScriptableObject
{
    
    [PreviewField(100, ObjectFieldAlignment.Left), HideLabel]
    public Sprite characterSprite;
    
    public string characterName;
    [TextArea]public string characterDescription;
    public int health;

    [Space(15)] 
    public GameObject skillButtonsGroupPrefab;
}
