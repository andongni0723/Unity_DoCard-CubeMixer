using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Component")]
    private Camera mainCamera;
    private MeshRenderer meshRenderer;
    private GameObject showChild;

    [Header("Settings")] 
    public Material canWalkMaterial;
    public Material mouseTargetMaterial;
    public Material inAttackRangeMaterial;
    private Material defaultMaterial;

    [Space(15)] public float standY;
    public float standDuration = 0.5f;

    //[Header("Debug")]
    [Header("Data")] public Vector2 tilePosition;
    public float angle;

    private bool isStand = false;
    private bool isMouseHit = false;
    private bool inAttackRange = false;
    private Character tempCharacterWantToMove;
    private Vector2 tempCharacterAttackRangeDistance;

    private void Awake()
    {
        showChild = transform.GetChild(0).gameObject;
        meshRenderer = GetComponent<MeshRenderer>();
        defaultMaterial = meshRenderer.material;
        mainCamera = Camera.main;
    }

    private void OnValidate()
    {
        // tilePosition = new Vector2(transform.GetSiblingIndex(), transform.parent.GetSiblingIndex());
    }

    #region Event

    private void OnEnable()
    {
        EventHandler.TilePosYStand += OnTilePosYStand; // Test Stand
        EventHandler.TilePosXStand += OnTilePosXStand; // Test Stand
        EventHandler.TilePosAddManagerList += OnTilePosAddManagerList; // Initial Setting

        EventHandler.TileUpAnimation += OnTileUpAnimation; // Tile up animation
        EventHandler.CharacterActionEnd += OnCharacterCancelMove; // Tile down animation
        EventHandler.AttackRangeColor += OnAttackRangeColor; // Check and Set Material to Attack Range Color
    }

    private void OnDisable()
    {
        EventHandler.TilePosYStand -= OnTilePosYStand;
        EventHandler.TilePosXStand += OnTilePosXStand;

        EventHandler.TilePosAddManagerList += OnTilePosAddManagerList;
        EventHandler.TileUpAnimation -= OnTileUpAnimation;
        EventHandler.CharacterActionEnd -= OnCharacterCancelMove;
        EventHandler.AttackRangeColor -= OnAttackRangeColor;
    }

    private void OnTileUpAnimation(SkillDetailsSO data, Character character, Vector2 skillAttackRange, Vector2 playerPos, Vector2 distance)
    {
        tempCharacterWantToMove = character;
        tempCharacterAttackRangeDistance = skillAttackRange;

        if (Vector2.Distance(playerPos, tilePosition) <= distance.y)
        {
            // If skill area can't enemy on and have character who not temp one on the tile
            if (!data.isSkillAreaCanEnemyOn && CheckHaveCharacterWhoNotTempCharacterWOnTile()) return;
            
            isStand = true;
            transform.DOMoveY(standY, standDuration); 
        }

        if (isMouseHit)
            EventHandler.CallAttackRangeColor(tilePosition, skillAttackRange);
    }

    private void OnCharacterCancelMove()
    {
        if (isStand)
        {
            isStand = false;
            // tempCharacterWantToMove = null;
            transform.DOMoveY(0, standDuration);
        }

        inAttackRange = false;
    }

    private void OnAttackRangeColor(Vector2 targetTilePos, Vector2 distance)
    {
        // if(isMouseHit)
        //     Debug.Log($"{skillAttackRange}");
        // if(isMouseHit)
        //     Debug.Log($"{skillAttackRange}");
        var newDistance = CheckMouseDirectionToRotateSkillRange(distance);
        //distance = new Vector2(distance.y, distance.x);
        inAttackRange = Mathf.Abs(tilePosition.x - targetTilePos.x) < newDistance.x &&
                        Mathf.Abs(tilePosition.y - targetTilePos.y) < newDistance.y;
    }

    // ------------------- Mouse -------------------

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventHandler.CallReturnMouseHitTilePosition(tilePosition);
     
        isMouseHit = true;
        if (isStand)
            EventHandler.CallAttackRangeColor(tilePosition, tempCharacterAttackRangeDistance);

        // Debug.Log(tempCharacterAttackRangeDistance.x + " " + tempCharacterAttackRangeDistance.y);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseHit = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isStand)
        {
            if (tempCharacterWantToMove != null)
                tempCharacterWantToMove.TileReturnClickData(gameObject, tilePosition);
        }
    }

    #endregion

    private void Update()
    {
        UpdateTileColor();
        // TODO: Fix this , the skill direction is wrong
    }

    private void UpdateTileColor()
    {
        if (isMouseHit && isStand)
            meshRenderer.material = mouseTargetMaterial;
        else if (inAttackRange)
            meshRenderer.material = inAttackRangeMaterial;
        else if (isStand)
            meshRenderer.material = canWalkMaterial;
        else
            meshRenderer.material = defaultMaterial;
    }

    private Vector2 CheckMouseDirectionToRotateSkillRange(Vector2 skillAttackRange)
    { 
        // Character Tile Position
        Vector2 baseTilePos = tempCharacterWantToMove.characterTilePosition;
        Vector2 dir = DetailsManager.Instance.mouseHitTilePos;
        Vector2 reverseSkillAttackRange = new Vector2(skillAttackRange.y, skillAttackRange.x);
        Vector2 vector = dir - baseTilePos;
        
        angle = (int)(Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg) - 90; // get angle
        angle = angle < 0 ? angle + 360 : angle;                             // set angle to 0 - 360
        angle = vector == Vector2.zero ? 0 : angle;                          // set mouse hit the character position is dir to up
        
        // TODO : error
        // up
        if (angle is >= 0 and <= 45 or >= 315 and <= 360) // 0 ~ 45 or 315 ~ 360
            return skillAttackRange;
        // left
        if(angle is >= 45 and <= 135)
            return reverseSkillAttackRange;
        // right
        if(angle is >= 225 and <= 315)
            return reverseSkillAttackRange;
        // down
        return skillAttackRange;
    }

/// <summary>
    /// Check if there is a character ＂who not temp character" on the tile
    /// </summary>
    /// <returns></returns>
    private bool CheckHaveCharacterWhoNotTempCharacterWOnTile()
    {
        for(int i = 0 ; i < transform.childCount; i++)
        {
            if (!transform.GetChild(i).CompareTag("Character")) continue;
            return transform.GetChild(i) != tempCharacterWantToMove.transform;
        }
        
        return false;
    }
    
    /// <summary>
    /// Check if there is a character on the tile
    /// </summary>
    /// <returns></returns>
    private bool CheckHaveCharacterOnTile()
    {
        for(int i = 0 ; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("Character"))
                return true;
        }
        
        return false;
    }

    #region Test
    private void OnTilePosXStand(int no) => showChild.SetActive(no == tilePosition.x);
    private void OnTilePosYStand(int no) => showChild.SetActive(no == tilePosition.y);
    
    private void OnTilePosAddManagerList(List<Grid> grids, Vector2 pos)
    {
        if (pos == tilePosition)
        {
            Debug.Log("d");
            grids.Add(GetComponent<Grid>());
        }
    }
    #endregion


    
}
