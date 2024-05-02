using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CameraPositionControllerSlider : MonoBehaviour
{
    [Header("Component")]
    public Slider slider;
    public CinemachineSmoothPath cameraPath;
    //[Header("Settings")]
    //[Header("Debug")]
    
    private void Awake()
    {
        slider.maxValue = cameraPath.PathLength;
        slider.onValueChanged.AddListener(EventHandler.CallCameraPositionValueChange);
    }
}
