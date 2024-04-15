using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    //[Header("Component")]
    [Header("Settings")]
    public List<EnvironmentVisibleDetails> EnvironmentVisibleDetailsList = new();
    [Space(15)]
    public List<GameObject> InvisibleObjectList = new();
    //[Header("Debug")]

    private void OnEnable()
    {
        EventHandler.ReturnCharacterInitializedDone += OnCharacterInitializedDone; // Set wall active
    }

    private void OnDisable()
    {
        EventHandler.ReturnCharacterInitializedDone -= OnCharacterInitializedDone;
    }

    private void OnCharacterInitializedDone()
    {
        foreach (var data in EnvironmentVisibleDetailsList)
        {
            data.GameObject.SetActive(data.visibleTeam == GameManager.Instance.selfTeam);
        }
        
        foreach (var obj in InvisibleObjectList)
        {
            obj.SetActive(false);
        }
    }
}
