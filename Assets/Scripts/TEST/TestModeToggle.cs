using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TestMode
{
    InfinityPower, 
    InfinityHealth,
}

public class TestModeToggle : MonoBehaviour
{
    // [Header("Component")]
    private Toggle toggle;
    
    [Header("Settings")] 
    public TestMode modeName;
    //[Header("Debug")]
}
