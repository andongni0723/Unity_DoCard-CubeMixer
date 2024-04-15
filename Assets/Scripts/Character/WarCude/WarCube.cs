using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine.Playables;
using UnityEngine.Timeline;


public class WarCube : Character
{
    [Header("Skill Object")] 
    public List<GameObject> redSwordVFXList;
    public List<GameObject> blueSwordVFXList;
    public List<MeshRenderer> skillCubeList;
    public List<TimelineClip> swordVFXClipList;
    public PlayableDirector skillRotate;
    public PlayableDirector skillTrail;
    public PlayableDirector skillLightWorld;

    protected override void SetTeamBodyMaterial()
    {
        base.SetTeamBodyMaterial();
        
        if (team == Team.Blue)
        {
            foreach (var meshRenderer in skillCubeList)
                meshRenderer.material = blueBodyMaterial;
        }
        
        // set different team sword vfx don't show
        foreach (var swordVFX in team == Team.Red ? blueSwordVFXList : redSwordVFXList)
            swordVFX.SetActive(false);

        foreach (var clip in swordVFXClipList)
        {
            // clip.animationClip.
        }
    }

    protected override void AttackAction(SkillDetailsSO skillDetailsSO, List<TileReturnData> skillTileReturnDataList)
    {
        base.AttackAction(skillDetailsSO, skillTileReturnDataList);

        switch (skillDetailsSO.skillID)
        {
            case "001-rotate":
                skillRotate.Play();
                break;
            case "002-trail":
                var dir = UseSkillTargetPosToRotateCharacter(skillTileReturnDataList[0].targetTilePos);
                Debug.Log(dir);
                skillTrail.transform.rotation = Quaternion.Euler(0, dir, 0);
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
        
        // skillRotate.transform.rotation = quaternion.identity;
        // skillTrail.transform.rotation = Quaternion.identity;
    }
}
