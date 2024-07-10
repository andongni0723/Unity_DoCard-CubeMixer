using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorBulletHitVFX : MonoBehaviour
{
    //[Header("Component")]
    //[Header("Settings")]
    //[Header("Debug")]


    private void Awake()
    {
        Invoke(nameof(Destroy), 1.5f);
    }
    
    private void Destroy()
    {
        Destroy(gameObject);
    }
}
