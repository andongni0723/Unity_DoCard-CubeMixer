using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
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

    public void Shake(float intensity) => noise.m_AmplitudeGain = intensity;
    public void Shake(float intensity, float duration)
    {
        Shake(intensity);
        StartCoroutine(TimeToCloseShake(duration));
    }
    /// <param name="startTimeToFade">when the time, the shake intensity will fade to zero</param>
    public void Shake(float intensity, float duration, float startTimeToFade)
    {
        Shake(intensity, duration);
        StartCoroutine(TimeToFadeShake(startTimeToFade, duration - startTimeToFade));
    }

    private IEnumerator TimeToCloseShake(float time)
    {
        yield return new WaitForSeconds(time);

        noise.m_AmplitudeGain = 0;
    }
    
    private IEnumerator TimeToFadeShake(float waitTime, float fadeDuration)
    {
        yield return new WaitForSeconds(waitTime);
        DOTween.To(() => noise.m_AmplitudeGain, x => noise.m_AmplitudeGain = x, 0, fadeDuration);
    }
}
