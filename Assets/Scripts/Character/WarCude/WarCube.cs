using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;


public class WarCube : Character
{
    protected override  void AttackAction(SkillDetailsSO skillDetailsSO)
    {
        base.AttackAction(skillDetailsSO);

        Debug.Log("R");
        
        switch (skillDetailsSO.skillID)
        {
            case "001-rotate":
                break;
            case "002-trail":
                break;
            case "003-light-world":
                break;
            case "FIN-to-dark":
                break;
        }
    }

}
