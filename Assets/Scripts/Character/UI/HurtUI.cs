using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class HurtUI : MonoBehaviour
{
    [Header("Component")] 

    [Header("Settings")]
    public GameObject hurtTextPrefab;
    //[Header("Debug")]

    // ------------------- Tools -------------------
    public void CallHurtTextAnimation(int damage)
    {
        HurtText newHurtText = Instantiate(hurtTextPrefab, transform.position, transform.rotation, transform).GetComponent<HurtText>();
        newHurtText.Animaiton(damage);
    }
}
