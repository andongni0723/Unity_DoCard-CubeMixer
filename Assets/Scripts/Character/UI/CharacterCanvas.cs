using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCanvas : MonoBehaviour
{
    //[Header("Component")]
    private Camera mainCamera;

    //[Header("Settings")]
    //[Header("Debug")]
    
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
    }
}
