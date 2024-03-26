using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordVFX : MonoBehaviour
{
    [Header("Component")]
    private Animation anim;
    
    
    //[Header("Settings")]
    //[Header("Debug")]
    
    
    private void Awake()
    {
        anim = GetComponent<Animation>();
        transform.GetChild(0).gameObject.SetActive(false);
    }


    public void PlayAnimation()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        anim.Play();
    }

    public void AnimationEnd()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
