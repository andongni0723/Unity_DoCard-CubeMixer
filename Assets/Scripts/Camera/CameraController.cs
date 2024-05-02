using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    //[Header("Component")]
    private PlayerInputSystem playerControls;
    private CinemachineTrackedDolly cameraTrack;

    [Header("Settings")] 
    public float speed = 1;
    
    //[Header("Debug")]
    private Vector2 mouseDelta;
    [SerializeField]private bool isDragging;
    
    private void Awake()
    {
        playerControls = new PlayerInputSystem();

        playerControls.GamePlay.MouseHold.performed += _ => StartCoroutine(Drag());
        playerControls.GamePlay.MouseHold.canceled += _ => isDragging = false;
        playerControls.GamePlay.MouseDelta.performed += _ =>
            mouseDelta = playerControls.GamePlay.MouseDelta.ReadValue<Vector2>();
       
        // playerControls.GamePlay.Enable();
        
        var camera = GetComponent<CinemachineVirtualCamera>();
        cameraTrack = camera.GetCinemachineComponent<CinemachineTrackedDolly>();
    }
    
    private void OnEnable()
    {
        EventHandler.CameraPositionValueChange += CameraPositionMove; // According to the new value change the camera position on the track
    }

    private void OnDisable()
    {
        EventHandler.CameraPositionValueChange -= CameraPositionMove;
        playerControls.GamePlay.Disable();
    }

    private void CameraPositionMove(float newValue)
    {
        cameraTrack.m_PathPosition = newValue;
    }

    IEnumerator Drag()
    {
        isDragging = true;

        while (isDragging)
        {
            CameraMove();
            yield return null;
        }
    }

    private void CameraMove()
    {
        cameraTrack.m_PathPosition += -mouseDelta.y * speed;
        // set camera track range(0, 30)
        cameraTrack.m_PathPosition = Mathf.Clamp(cameraTrack.m_PathPosition, 0, 30);
        
    }

}
