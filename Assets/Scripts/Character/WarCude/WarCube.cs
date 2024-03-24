using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;


public class WarCube : Character
{
    [Header("Skill Object")] 
    public PlayableDirector skillRotate;
    public PlayableDirector skillTrail;
    public PlayableDirector skillLightWorld;
    
    protected override void AttackAction(SkillDetailsSO skillDetailsSO, List<TileReturnData> skillTileReturnDataList)
    {
        base.AttackAction(skillDetailsSO, skillTileReturnDataList);

        Debug.Log("R");
        
        switch (skillDetailsSO.skillID)
        {
            case "001-rotate":
                skillRotate.Play();
                break;
            case "002-trail":
                skillTrail.Play();
                break;
            case "003-light-world":
                break;
            case "FIN-to-dark":
                break;
            
            default:
                Debug.LogError("The skill ID is not found.");
                break;
        }
    }
}
