using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitPlayPrepareDone : MonoBehaviour
{
    //[Header("Component")]
    [Header("Settings")]
    public float waitTime = 3f;
    //[Header("Debug")]


    private void OnEnable()
    {
        EventHandler.UIObjectGenerate += WaitPlayer;
    }
    private void OnDisable()
    {
        EventHandler.UIObjectGenerate -= WaitPlayer;
    }

    private void WaitPlayer()
    {
        StartCoroutine(WaitAndStartGame());
    }

    private IEnumerator WaitAndStartGame()
    {
        yield return new WaitForSeconds(waitTime);
        EventHandler.CallStateCallback(GameState.LoadPlayerInGame);
    }
}
