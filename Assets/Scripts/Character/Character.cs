using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Timeline;

#pragma warning disable CS4014

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


[RequireComponent(typeof(CharacterHealth), typeof(CharacterPowerManager), typeof(GameDataBackUp))]
[RequireComponent(typeof(CharacterStatusManager), typeof(SignalReceiver), typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Character : CharacterActionEvent, ITileClickHandler
{
    [Header("Component")]
    [HideInInspector] public MeshRenderer body;
    [HideInInspector] public CharacterManager characterManager; // set by CharacterGenerator
    public CharacterHealth characterHealth;
    public CharacterPowerManager CharacterPower;
    public CharacterStatusManager characterStatus;
    protected GameDataBackUp gameDataBackUp;
    public CharacterSkillButtonsGroup characterSkillButtonsGroup; // set by CharacterSkillButtonsGroup
    public GameObject characterObject;

    
    [Space(15)]
    public Material blueBodyMaterial;

    [SerializeField]
    [FoldoutGroup("Setting Debug")] private string id;
    public string ID => id;
    [FoldoutGroup("Setting Debug")]  public CharacterDetailsSO characterDetails; // set by CharacterGenerator
    [Range(0, 20)]
    [FoldoutGroup("Setting Debug")]  public int moveMaxDistance;
    [FoldoutGroup("Setting Debug")]  public float faceRotation = 0; // red = 0, blue = 180
    [FoldoutGroup("Setting Debug")] public bool isSkillPlaying = false;

    
    [FoldoutGroup("Debug")] public Team team;
    [FoldoutGroup("Debug")] public Vector2 characterTilePosition;
    [FoldoutGroup("Debug")] public Vector3 bodyStartPos;
    [FoldoutGroup("Debug")] private Camera mainCamera;
    [FoldoutGroup("Debug")] public bool isTileReturn = false;
    [SerializeField]
    [FoldoutGroup("Debug")] public List<TileReturnData> skillTileReturnDataList = new();

    private void Awake()
    {
        mainCamera = Camera.main;
        characterHealth = GetComponent<CharacterHealth>();
        CharacterPower = GetComponent<CharacterPowerManager>();
        gameDataBackUp = GetComponent<GameDataBackUp>();
        characterStatus = GetComponent<CharacterStatusManager>();
        characterObject = transform.GetChild(0).gameObject;
        // powerManager.characterHealth = characterHealth;
        
        bodyStartPos = body.transform.localPosition;
    }

    private void Start()
    {
        characterHealth.InitialUpdateData(characterDetails.health, characterDetails.power);
    }
    
    public void InitialUpdateData(string id)
    {
        // Setting variable
        this.id = id;
        DetailsManager.Instance.NewCharacterDetails(this);
        gameDataBackUp.InitialUpdate();
    }

    public void SetTeam(Team team)
    {
        this.team = team;
        SetTeamBodyMaterial();
        SetLookAtForward();
    }

    #region Event

    private void OnEnable()
    {
        EventHandler.CharacterChooseTileRangeDone += OnCharacterChooseTileRangeDone; // setting variable
        EventHandler.ChangeStateDone += OnChangeStateDone; // check is Action state to update turn start data
        EventHandler.CharacterActionEnd += OnCharacterActionEnd; // cancel ready power
        EventHandler.CharacterDead += OnCharacterDead;
    }

    private void OnCharacterDead(Character target)
    {
        if(target != this) return;
        //
        // if(isLastPlayAction && characterManager.IsOwner) EventHandler.CallLastPlayActionEnd();
        // EventHandler.CallCharacterActionEnd(characterManager.IsOwner);
    }

    private void OnDisable()
    {
        EventHandler.CharacterChooseTileRangeDone -= OnCharacterChooseTileRangeDone;
        EventHandler.ChangeStateDone -= OnChangeStateDone; 
        EventHandler.CharacterActionEnd -= OnCharacterActionEnd;
    }

    private void OnCharacterActionEnd(bool isOwner)
    {
        if(isOwner) CharacterPower.CancelReadyPower();
    }

    private void OnCharacterChooseTileRangeDone()
    {
        isTileReturn = false;
    }
    
    private void OnChangeStateDone(GameState newState)
    {
        if (newState != GameState.ActionState) return;
        
        characterHealth.PowerBackToStart();
        CharacterPower.powerPanel.InitialDisplay(characterDetails.power);
        characterHealth.healthPanel.InitialDisplay(characterHealth.currentHealth);
    }
    #endregion

    #region Tools
    // --------------- Tools --------------- //

    public void HitAnimation()
    {
        body.transform.DOPunchPosition(Vector3.forward * 0.3f, 0.5f)
            .OnComplete(BodyResetPosition);
    }
    public void BodyResetPosition()
    {
        body.transform.localPosition = bodyStartPos;
    }
    
    public void ButtonCallUseSkill(SkillDetailsSO skillDetails)
    {
        StopAllCoroutines();
        if (CharacterPower.CheckPowerEnough(skillDetails))
        {
            StartCoroutine(SkillExecuteAction(skillDetails));
            FunctionButtonManager.Instance.CallButtonDisableEvent(ButtonCode.CharacterAction);
            EventHandler.CallButtonCallUseSkillEvent(); 
        }
        else
        {
            HintPanelManager.Instance.CallError("Not enough power!");
        }
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
        Debug.Log("skillTargetPosition: " + skillTargetPosition + " characterTilePosition: " + characterTilePosition);
        Vector2 vector = skillTargetPosition - characterTilePosition;
        
        int angle = (int)(Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg) - 90; // get angle
        angle = angle < 0 ? angle + 360 : angle;                                 // set angle to 0 - 360
        angle = vector == Vector2.zero ? 0 : angle;                              // set mouse on character position is dir to forward
        
        // up
        if (angle is >= 0 and <= 45 or >= 315 and <= 360) // 0 ~ 45 or 315 ~ 360 degree
            return 0;
        // left
        if (angle is >= 45 and <= 135)                    // 45 ~ 135 degree
            return -90;
        // right
        if (angle is >= 225 and <= 315)                   // 225 ~ 315 degree
            return 90;
        // down
        return 180;
    }
    
    #endregion


    #region Callback and Child override
    // --------------- Callback and Child override --------------- //

    /// <summary>
    /// Dependency Injection: Call by tile, when mouse click the target tile
    /// </summary>
    /// <param name="tileGameObject">the tile the mouse click</param>
    /// <param name="targetTilePos"></param>
    void ITileClickHandler.TileReturnClickData(GameObject tileGameObject, Vector2 targetTilePos)
    {
        isTileReturn = true;
        skillTileReturnDataList.Add(new TileReturnData(tileGameObject, targetTilePos));
    }
    
    public virtual void SkillActionStart() => isSkillPlaying = true;
    public virtual void SkillActionEnd()
    {
        isSkillPlaying = false;
    }

    public virtual void CallCameraShake()
    {
        CameraShake.Instance.Shake(2, 0.2f); 
    }

    
    
    public virtual IEnumerator AttackAction (string skillID,SkillButtonType skillButtonType, List<Vector2> skillTargetPosDataList, bool isLastPlayAction = false) 
    {
        // Override by child, Write the skill action

        // Wait skill play end
        yield return new WaitUntil(() => !isSkillPlaying); // wait skill play end
        if(isLastPlayAction && characterManager.IsOwner) EventHandler.CallLastPlayActionEnd();
        EventHandler.CallCharacterActionEnd(characterManager.IsOwner);
    }
    
    #endregion


    #region Game
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
                yield return CallTileStandAnimation(
                    skillDetails, 
                    Vector2.zero, 
                    new Vector2(skillDetails.moveRange, skillDetails.moveRange));
                yield return new WaitUntil(() => isTileReturn); // when player choose the tile
                
                EventHandler.CallCharacterChooseTileRangeDone();
                MoveAction(
                    skillTileReturnDataList[0].tileGameObject,
                    skillTileReturnDataList[0].targetTilePos);
                break;
            
            case SkillButtonType.Attack:
                for (int i = 0; i < skillDetails.attackAimTime; i++)
                {
                    yield return CallTileStandAnimation(
                        skillDetails,
                        skillDetails.SkillAimDataList[i].skillAttackRange, 
                        skillDetails.SkillAimDataList[i].skillCastRange,
                        skillDetails.isDirectionAttack,
                        i == 0 ? 0.1f : 0f);
                    yield return new WaitUntil(() => isTileReturn); // when player choose the tile
                    
                    CharacterUseSkill();
                    EventHandler.CallCharacterChooseTileRangeDone();
                }
                
                var skillTargetPosList = 
                    skillTileReturnDataList
                    .Select(data => new Vector2(data.targetTilePos.x, data.targetTilePos.y))
                    .ToList();
                
                StartCoroutine(AttackAction(
                    skillDetails.skillID,
                    skillDetails.skillType, 
                    skillTargetPosList,
                    true));
                break;
            
            case SkillButtonType.Skill:
                yield return CallTileStandAnimation(skillDetails, Vector2.zero, Vector2.zero);
                yield return new WaitUntil(() => isTileReturn); // when player choose the tile
                
                CharacterUseSkill();
                EventHandler.CallCharacterChooseTileRangeDone();
                
                skillDetails.skillEffectList.ForEach(effect => 
                    characterStatus.AddStatusEffect(skillDetails, effect.data, effect.count));
                Debug.Log("SKill type");
                
                StartCoroutine(AttackAction(
                    skillDetails.skillID,
                    skillDetails.skillType, 
                    new List<Vector2>() { Vector2.zero },
                    true));
                break;
        }
        
        UsePowerOrStatus(skillDetails);

        // Record the character action
        characterManager.characterActionRecord.
            AddCharacterActionData(ID, skillDetails, skillTileReturnDataList);
    }

    private void UsePowerOrStatus(SkillDetailsSO skillDetails)
    {
        switch (skillDetails.skillUseCondition)
        {
            case SkillUseCondition.Power:
                CharacterPower.UsePower(skillDetails.skillNeedPower);
                break;
            case SkillUseCondition.Count:
                characterStatus.RemoveStatusEffect(skillDetails, skillDetails.countStatusEffectData, skillDetails.needCount);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void MoveAction(Vector2 targetTilePos, float duration = 0.5f, bool isLastPlayAction = false)
    { 
        MoveAction(GridManager.Instance.GetTileWithTilePos(targetTilePos).gameObject, targetTilePos, duration, isLastPlayAction);
    }

    public void MoveAction(GameObject tileGameObject, Vector2 targetTilePos, float duration = 0.5f, bool isLastPlayAction = false)
    {
        // Move Animation
        var position = tileGameObject.transform.position;
        characterTilePosition = targetTilePos;
        transform.DOMove(new Vector3(position.x, 0.1f, position.z), duration).OnComplete(() =>
        {
            transform.SetParent(tileGameObject.transform);  
            
            if(isLastPlayAction && characterManager.IsOwner) EventHandler.CallLastPlayActionEnd();
            EventHandler.CallCharacterActionEnd(characterManager.IsOwner); 
        });
    }

    private async UniTask CallTileStandAnimation(SkillDetailsSO data, Vector2 skillAttackRange, Vector2 maxStandDistance, bool isStrict = false, float duration = 0.1f)
    {
        Vector2 beforeMoveTilePos = characterTilePosition;
        
        for (int j = isStrict ? (int)maxStandDistance.y : 0 ; j <= maxStandDistance.y; j++)
        {
            EventHandler.CallTileUpAnimation(data, this, skillAttackRange, beforeMoveTilePos, new Vector2(j, j), isStrict);
            await UniTask.Delay(TimeSpan.FromSeconds(duration)); // Let the difference radius tile stand time
                                                                 // to show the animation
        }
    }
    
    #endregion

    #region ActionEvent
    // -------------- ActionEvent --------------- 
    
    /// <summary>
    /// Those method will override by child
    /// </summary>

    private void ApplyStatusEffect(Func<SkillDetailsSO, bool> condition)
    {
        foreach (var skill in characterDetails.characterSkillList.Where(condition))
            characterStatus.AddStatusEffect(skill, skill.countStatusEffectData, 1);
    }


    public override void CharacterUseSkill()
    {
        ApplyStatusEffect(data => data.isUseSkill);
    }

    public override void CharacterSkillHit()
    {
        ApplyStatusEffect(data => data.isSkillHit);
    }

    public override void CharacterUsePower()
    {
        ApplyStatusEffect(data => data.isUsePower);
    }

    public override void CharacterHasBeenDamage()
    {
        ApplyStatusEffect(data => data.hasBeenDamage);
    }

    
    #endregion
}
