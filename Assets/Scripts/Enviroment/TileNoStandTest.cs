using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileNoStandTest : MonoBehaviour
{
    //[Header("Component")]
    //[Header("Settings")]
    //[Header("Debug")]

    private void Start()
    {
        StartCoroutine(TestAction());
    }

    IEnumerator TestAction()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 8; i++)
        {
            EventHandler.CallTilePosXStand(i);
            yield return new WaitForSeconds(0.5f);
        }
        
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 22; i++)
        {
            EventHandler.CallTilePosYStand(i);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
