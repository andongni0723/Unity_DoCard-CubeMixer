using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestModePanel : Singleton<TestModePanel>
{
    //[Header("Component")]
    [Header("Settings")] 
    public bool infinityPower;
    public bool infinityHealth;


    //[Header("Debug")]
    public void SetInfinityPower(bool value) => infinityPower = value;
    public void SetInfinityHealth(bool value) => infinityHealth= value;

}
