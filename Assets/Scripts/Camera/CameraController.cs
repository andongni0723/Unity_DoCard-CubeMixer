using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    //[Header("Component")]
    private new CinemachineVirtualCamera camera;
    private CinemachineTrackedDolly cameraTrack;

    [Header("Settings")] 
    public float speed = 1;
    
    private void Awake()
    {
        camera = GetComponent<CinemachineVirtualCamera>();
        cameraTrack = camera.GetCinemachineComponent<CinemachineTrackedDolly>();
    }
    
    private void OnEnable()
    {
        EventHandler.CameraPositionValueChange += CameraPositionMove; // According to the new value change the camera position on the track
    }

    private void OnDisable()
    {
        EventHandler.CameraPositionValueChange -= CameraPositionMove;
    }

    private void CameraPositionMove(float newValue)
    {
        cameraTrack.m_PathPosition = newValue;
    }
}
