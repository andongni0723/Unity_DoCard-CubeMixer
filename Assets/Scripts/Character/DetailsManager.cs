using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailsManager : Singleton<DetailsManager>
{
    //[Header("Component")]
    [Header("Settings")]
    [SerializeField] private List<CharacterDetailsSO> allCharacterDetailsList = new();
    
    //[Header("Debug")]
    
    public CharacterDetailsSO UseIndexSearchCharacterDetailsSO(int index)
    {
        try
        {
            return allCharacterDetailsList[index];
        }
        catch (Exception e)
        {
            Debug.LogError("SearchCharacterDetailsError: Index Error\n" + e);
            return null;
        }
    }
}
