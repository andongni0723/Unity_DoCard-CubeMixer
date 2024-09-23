using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Playables;
using Object = UnityEngine.Object;


public class WarCube : Character
{
    [Header("Skill Object")] 
    [ReadOnly]public Animator cameraAnimator;
    
    [Space(10)]
    public PlayableDirector skillRotate;
    public PlayableDirector skillCeaselessCycle;
    public PlayableDirector skillTrail;
    public PlayableDirector skillCircularCarrier;
    public PlayableDirector skillLightWorld;
    public PlayableDirector darknessRevive;

    public List<GameObject> redSwordVFXList;
    public List<GameObject> blueSwordVFXList;
    public List<MeshRenderer> skillCubeList;

    // --------------Game-----------------

    protected override void SetTeamBodyMaterial()
    {
        base.SetTeamBodyMaterial();
        
        // set skill light world binding "camera" to cameraAnimator
        cameraAnimator = GameManager.Instance.GetSelfCameraController().GetComponentInChildren<Animator>();
        using var enumerator = skillLightWorld.playableAsset.outputs.GetEnumerator();
        int i = 0;
        while (enumerator.MoveNext())
        {
            Object currentKey = enumerator.Current.sourceObject;
            if (currentKey.name == "camera")
                skillLightWorld.SetGenericBinding(currentKey, cameraAnimator);
        }
        
        // set different team body material
        if (team == Team.Blue)
        {
            foreach (var meshRenderer in skillCubeList)
                meshRenderer.material = blueBodyMaterial;
        }
        
        // set different team sword vfx don't show
        foreach (var swordVFX in team == Team.Red ? blueSwordVFXList : redSwordVFXList)
            swordVFX.SetActive(false);
    }

    public override void CallCameraShake()
    {
        // light world skill camera shake
        CameraShake.Instance.Shake(8, 5, 3.5f);
    }

    public override IEnumerator AttackAction(string skillID,SkillButtonType skillButtonType, List<Vector2> skillTargetPosDataList, bool isLastPlayAction = false)
    {
        if (characterHealth.isDead)
        {
            SkillActionEnd();
            Debug.Log("Break");
            yield return base.AttackAction(skillID, skillButtonType, skillTargetPosDataList, isLastPlayAction);
            yield break;
        }
        
        SkillActionStart();
        ResetLookAt();

        int dir;
        switch (skillID)
        {
            case "001-rotate":
                skillRotate.Play();
                break;
            
            case "011-circular-carrier":
                skillCircularCarrier.Play();
                break;
            
            case "002-trail":
                dir = UseSkillTargetPosToRotateCharacter(skillTargetPosDataList[0]);
                skillTrail.transform.rotation = Quaternion.Euler(0, dir, 0);
                skillTrail.Play();
                break;
            
            case "012-ceaseless-cycle":
                dir = UseSkillTargetPosToRotateCharacter(skillTargetPosDataList[0]);
                skillCeaselessCycle.transform.rotation = Quaternion.Euler(0, dir, 0);
                skillCeaselessCycle.Play();
                break;
            
            case "003-light-world":
                skillLightWorld.transform.position = 
                    GridManager.Instance.GetTileWithTilePos(skillTargetPosDataList[0]).transform.position;
                skillLightWorld.Play();
                break;
            
            case "FIN-to-dark":
                darknessRevive.Play();
                break;
            
            default:
                Debug.LogError("The skill ID is not found.");
                break;
        }

        yield return base.AttackAction(skillID, skillButtonType, skillTargetPosDataList, isLastPlayAction);
    }
}
