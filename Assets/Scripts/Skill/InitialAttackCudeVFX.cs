using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialAttackCudeVFX : MonoBehaviour
{
    //[Header("Component")]
    //[Header("Settings")]
    //[Header("Debug")]
    
    void Start () {
 
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        GetComponent<Renderer>().GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor("_Color", new Color(1, 0, 0, 1));
        GetComponent<Renderer>().SetPropertyBlock(propertyBlock);
	   
    }
}
