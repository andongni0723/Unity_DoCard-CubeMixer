using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoneButton : MonoBehaviour
{
    //[Header("Component")]
    private Button button;
    //[Header("Settings")]
    //[Header("Debug")]
    
    private void Awake()
    {
        button = GetComponent<Button>();
        
        button.onClick.AddListener(() =>
        {
            EventHandler.CallStateCallback(GameState.ActionState);
            button.interactable = false;
        });
    }
}
