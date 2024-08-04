using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    //[Header("Component")]
    private Button button;
    //[Header("Settings")]
    //[Header("Debug")]
    
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(EventHandler.CallTurnCharacterStartAction);
        button.onClick.AddListener(() =>
        {
            HintPanelManager.Instance.CallHint("Replay Action");
            FunctionButtonManager.Instance.CallButtonDisableEvent(ButtonCode.PlayButton);
        });
        
    }
}
