using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
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
            // HintPanelManager.Instance.CallHint("Turn Start");
            HintPanelManager.Instance.CallChooseBox("Are you sure you want to clear all actions?",
                EventHandler.CallCharacterActionClear);
        });
    }
}
