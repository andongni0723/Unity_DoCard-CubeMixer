using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShake : Singleton<CameraShake>
{
    //[Header("Component")]
    public CameraController selfCamera;
    private CinemachineBasicMultiChannelPerlin noise;
    //[Header("Settings")]
    //[Header("Debug")]
    
    private void OnEnable()
    {
        EventHandler.CharacterObjectGeneratedDone += Initial;
    }

    private void OnDisable()
    {
        EventHandler.CharacterObjectGeneratedDone -= Initial;
    }

    private void Initial()
    {
        selfCamera = GameManager.Instance.GetSelfCameraController();
        noise = selfCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    
    public void Shake(float intensity, float time)
    {
        noise.m_AmplitudeGain = intensity;
        //noise
        StartCoroutine(TimeToCloseShake(time));
    }

    private IEnumerator TimeToCloseShake(float time)
    {
        yield return new WaitForSeconds(time);

        noise.m_AmplitudeGain = 0;
    }
}
