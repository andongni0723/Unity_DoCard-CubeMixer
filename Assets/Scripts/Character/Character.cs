using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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


[RequireComponent(typeof(CharacterHealth))]
public class Character : MonoBehaviour
{
    [Header("Component")]
    public MeshRenderer body;
    public CharacterManager characterManager;
    public CharacterHealth characterHealth;
    public PowerPanel powerPanel;
    
    [Space(15)]
    public Material blueBodyMaterial;
    
    
    [Header("Settings")] 
    [SerializeField]private string id;
    public string ID => id;
    
    public CharacterDetailsSO characterDetails;
    [Range(0, 20)]
    public int moveMaxDistance;
    public float faceRotation = 0; // red = 0, blue = 180
    public bool isSkillPlaying = false;

    [Header("Debug")] 
    public Team team;
    public Vector2 characterTilePosition;
    private CharacterGameData turnStartGameData;
    private Camera mainCamera;
    private bool isTileReturn = false;
    [SerializeField]protected List<TileReturnData> skillTileReturnDataList = new();

    private void Awake()
    {
        mainCamera = Camera.main;
        characterHealth = GetComponent<CharacterHealth>();
        powerPanel ??= transform.GetChild(0).GetChild(0).GetComponent<PowerPanel>();
    }

    #region Event

    private void OnEnable()
    {
        EventHandler.CharacterActionClear += BackToTurnStartPoint; // character data back to turn start data
        EventHandler.CharacterBackToTurnStartPoint += BackToTurnStartPoint; // setting variable
        EventHandler.CharacterActionEnd += OnCharacterActionEnd; // setting variable
    }

    private void OnDisable()
    {
        EventHandler.CharacterActionClear -= BackToTurnStartPoint;
        EventHandler.CharacterActionEnd -= OnCharacterActionEnd;
    }

    protected virtual void BackToTurnStartPoint()
    {
        MoveAction(turnStartGameData.tilePosition, 0);
        characterHealth.currentHealth = turnStartGameData.currentHealth;
        characterHealth.currentPower = turnStartGameData.currentHealth;
    }

    private void OnCharacterActionEnd()
    {
        isTileReturn = false;
    }

    #endregion

    public void InitialUpdateData(string id)
    {
        // Setting variable
        this.id = id;
        characterHealth.InitialUpdateDate(characterDetails.health, characterDetails.power);
        turnStartGameData = new CharacterGameData(
            characterDetails.health, 
            characterDetails.power, 
            characterHealth.currentHealth,
            characterHealth.currentPower, 
            tilePosition: characterTilePosition);
        
        DetailsManager.Instance.NewCharacterDetails(this);
    }

    public void SetTeam(Team team)
    {
        this.team = team;
        SetTeamBodyMaterial();
        SetLookAtForward();
    }
    
    // --------------- Tools --------------- //
    
    public void ButtonCallUseSkill(SkillDetailsSO skillDetailsSo)
    {
        StopAllCoroutines();
        StartCoroutine(SkillExecuteAction(skillDetailsSo));
    }

    protected virtual void SetTeamBodyMaterial()
    {
        body.material = team == Team.Blue? blueBodyMaterial : body.material;
    }
    
    protected void SetLookAtForward()
    {
        faceRotation = team == Team.Red ? 0 : 180;
        transform.rotation = Quaternion.Euler(0, faceRotation, 0);
    }
    protected void ResetLookAt()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
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
    
  
    // --------------- Callback and Child override --------------- //

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
    
    public virtual void SkillActionStart() => isSkillPlaying = true;
    public virtual void SkillActionEnd()
    {
        isSkillPlaying = false;
        Debug.Log("Skill Done");
    }

    public virtual IEnumerator AttackAction
        (string skillID,SkillButtonType skillButtonType, List<Vector2> skillTargetPosDataList) {
        Debug.Log("Parent");yield return null;}
    
    
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
                Debug.LogError("SkillButtonType is Empty :The skill button is not bind with any skill");
                HintPanelManager.Instance.CallError("SkillButtonType is Empty :The skill button is not bind with any skill");
                break;
            
            case SkillButtonType.Move:
                yield return CallTileStandAnimation(skillDetails, Vector2.zero, 
                    new Vector2(skillDetails.moveRange, skillDetails.moveRange));
                
                yield return new WaitUntil(() => isTileReturn); // back the skill tile return data
                yield return MoveAction(skillTileReturnDataList[0].tileGameObject, skillTileReturnDataList[0].targetTilePos);
                EventHandler.CallCharacterActionEnd();
                break;
            
            case SkillButtonType.Attack:
                for (int i = 0; i < skillDetails.attackAimTime; i++)
                {
                    yield return CallTileStandAnimation(skillDetails, skillDetails.SkillAimDataList[i].skillAttackRange,
                                  skillDetails.SkillAimDataList[i].skillCastRange, i == 0 ? 0.1f : 0f);
                    
                    yield return new WaitUntil(() => isTileReturn); 
                    EventHandler.CallCharacterActionEnd();
                }
                
                var skillTargetPosList = 
                    skillTileReturnDataList
                    .Select(data => new Vector2(data.targetTilePos.x, data.targetTilePos.y))
                    .ToList();
                
                StartCoroutine(AttackAction(skillDetails.skillID,skillDetails.skillType, skillTargetPosList));
                break;
        }
        
        // Record the character action
        characterManager.characterActionRecord.
            AddCharacterActionData(ID, skillDetails, skillTileReturnDataList);
    }

    public async UniTask MoveAction(Vector2 targetTilePos, float duration = 0.5f)
    {
        await MoveAction(GridManager.Instance.GetTileWithTilePos(targetTilePos).gameObject, targetTilePos, duration);
    }
    private async UniTask MoveAction(GameObject tileGameObject, Vector2 targetTilePos, float duration = 0.5f)
    {
        // Move Animation
        var position = tileGameObject.transform.position;
        transform.DOMove(new Vector3(position.x, 0.1f, position.z), duration)
            .OnComplete(() =>
            {
                // Update Data
                characterTilePosition = targetTilePos;
                transform.SetParent(tileGameObject.transform);  
            });

        await UniTask.Yield(0); 
    }

    private async UniTask CallTileStandAnimation(SkillDetailsSO data, Vector2 skillAttackRange, Vector2 maxStandDistance, float duration = 0.1f)
    {
        Vector2 beforeMoveTilePos = characterTilePosition;
        
        for (int j = 0; j <= maxStandDistance.y; j++)
        {
            EventHandler.CallTileUpAnimation(data, this, skillAttackRange, beforeMoveTilePos, new Vector2(j, j));
            await UniTask.Delay(TimeSpan.FromSeconds(duration)); // Let the difference radius tile stand time
                                                                 // to show the animation
        }
    }
}
