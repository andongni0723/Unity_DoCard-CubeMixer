using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class TileReturnData
{
    public GameObject tileGameObject;
    public Vector2 targetTilePos;

    public TileReturnData(GameObject tileGameObject, Vector2 targetTilePos)
    {
        this.tileGameObject = tileGameObject;
        this.targetTilePos = targetTilePos;
    }
}

public class Character : MonoBehaviour
{
    [Header("Component")]
    public MeshRenderer body;
    public CharacterManager characterManager;
    public PowerPanel powerPanel;
    
    [Space(15)]
    public Material blueBodyMaterial;
    
    
    [Header("Settings")] 
    [SerializeField]private string id;
    public string ID => id;
    public CharacterDetailsSO characterDetails;

    [Range(0, 20)]public int moveMaxDistance;

    [Header("Debug")] 
    public Team team;
    public Vector2 characterTilePosition;
    private Camera mainCamera;
    private bool isTileReturn = false;
    [SerializeField]protected List<TileReturnData> skillTileReturnDataList = new();

    private void Awake()
    {
        mainCamera = Camera.main;
        powerPanel ??= transform.GetChild(0).GetChild(0).GetComponent<PowerPanel>();
    }

    #region Event

    private void OnEnable()
    {
        EventHandler.CharacterActionEnd += OnCharacterCancelMove;
    }

    private void OnDisable()
    {
        EventHandler.CharacterActionEnd -= OnCharacterCancelMove;
    }

    private void OnCharacterCancelMove()
    {
        isTileReturn = false;
    }

    #endregion

    public void InitialUpdateData(string id)
    {
        this.id = id;
        DetailsManager.Instance.NewCharacterDetails(this);
    }

    public void SetTeam(Team team)
    {
        this.team = team;
        SetTeamBodyMaterial();
    }
    
    // --------------- Tools --------------- //
    
    public void ButtonCallUseSkill(SkillDetailsSO skillDetailsSo)
    {
        StartCoroutine(SkillExecuteAction(skillDetailsSo));
    }

    protected virtual void SetTeamBodyMaterial()
    {
        body.material = team == Team.Blue? blueBodyMaterial : body.material;
    }
    
    /// <summary>
    /// Use Skill Target Position To Rotate Character 
    /// </summary>
    /// <param name="skillTargetPosition"></param>
    /// <returns>the object rotate value</returns>
    protected int UseSkillTargetPosToRotateCharacter(Vector2 skillTargetPosition)
    {
        Vector2 vector = skillTargetPosition - characterTilePosition;
        
        int angle = (int)(Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg) - 90; // get angle
        angle = angle < 0 ? angle + 360 : angle;                                 // set angle to 0 - 360
        angle = vector == Vector2.zero ? 0 : angle;                              // set mouse hit the character position is dir to up
        
        // up
        if (angle is >= 0 and <= 45 or >= 315 and <= 360) // 0 ~ 45 or 315 ~ 360
            return 0;
        // left
        if (angle is >= 45 and <= 135)
            return -90;
        // right
        if (angle is >= 225 and <= 315)
            return 90;
        // down
        return 180;
    }
    
    // --------------- Game --------------- // 

    
    /// <summary>
    /// Call by skill button, when skill button click
    /// </summary>
    /// <param name="skillDetails"></param>
    private IEnumerator SkillExecuteAction(SkillDetailsSO skillDetails)
    {
        skillTileReturnDataList.Clear();
        
        switch (skillDetails.skillType)
        {
            case SkillButtonType.Empty:
                Debug.LogError("The skill button is not bind with any skill");
                break;
            
            case SkillButtonType.Move:
                yield return CallTileStandAnimation(Vector2.zero, 
                    new Vector2(skillDetails.moveRange, skillDetails.moveRange));
                
                yield return new WaitUntil(() => isTileReturn); // back the skill tile return data
                yield return MoveAction(skillTileReturnDataList[0].tileGameObject, skillTileReturnDataList[0].targetTilePos);
                EventHandler.CallCharacterActionEnd();
                break;
            
            case SkillButtonType.Attack:
                for (int i = 0; i < skillDetails.attackAimTime; i++)
                {
                    yield return CallTileStandAnimation(skillDetails.SkillAimDataList[i].skillAttackRange,
                                  skillDetails.SkillAimDataList[i].skillCastRange, i == 0 ? 0.1f : 0f);
                    
                    yield return new WaitUntil(() => isTileReturn); 
                    EventHandler.CallCharacterActionEnd();
                }
                
                AttackAction(skillDetails, skillTileReturnDataList);
                break;
                
        }
    }

    /// <summary>
    /// Call by tile, when mouse click the target tile
    /// </summary>
    /// <param name="tileGameObject">the tile the mouse click</param>
    /// <param name="targetTilePos"></param>
    public void TileReturnClickData(GameObject tileGameObject, Vector2 targetTilePos)
    {
        isTileReturn = true;
        skillTileReturnDataList.Add(new TileReturnData(tileGameObject, targetTilePos));
    }

    private async UniTask MoveAction(GameObject tileGameObject, Vector2 targetTilePos)
    {
        // Move Animation
        var position = tileGameObject.transform.position;
        transform.DOMove(new Vector3(position.x, 0.1f, position.z), 0.5f).OnComplete(() =>
        {
            // Update Data
            characterTilePosition = targetTilePos;
            transform.SetParent(tileGameObject.transform);  
        });

        await UniTask.Yield(0); 
    }
    
    protected virtual void AttackAction(SkillDetailsSO skillDetails, List<TileReturnData> skillTileReturnDataList)
    {
        // Write on child class
    }
    

    private async UniTask CallTileStandAnimation(Vector2 skillAttackRange, Vector2 maxStandDistance, float duration = 0.1f)
    {
        Vector2 beforeMoveTilePos = characterTilePosition;
        
        for (int j = 0; j <= maxStandDistance.y; j++)
        {
            EventHandler.CallTileUpAnimation(this, skillAttackRange, beforeMoveTilePos, new Vector2(j, j));
            await UniTask.Delay(TimeSpan.FromSeconds(duration)); // Let the difference radius tile stand time
                                                                 // to show the animation
        }
    }
    
    // IEnumerator Test()
    // {
    //     while (true)
    //     {
    //         yield return new WaitForSeconds(1f);
    //         Vector2 beforeMoveTilePos = characterTilePosition;
    //         
    //         for (int i = 0; i <= moveMaxDistance; i++)
    //         {
    //             EventHandler.CallCharacterNearTileAnimation(this, beforeMoveTilePos, i);
    //             yield return new WaitForSeconds(0.1f);
    //         }
    //
    //         yield return new WaitForSeconds(1f);
    //         EventHandler.CallCharacterCancelMove();
    //     }
    // }
}
