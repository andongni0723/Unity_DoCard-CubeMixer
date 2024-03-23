using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // [Header("Component")]
    private MeshRenderer meshRenderer;
    private GameObject showChild;

    [Header("Settings")] 
    public Material canWalkMaterial;
    public Material mouseTargetMaterial;
    public Material inAttackRangeMaterial;
    private Material defaultMaterial;
    
    [Space(15)]
    public float standY;
    public float standDuration = 0.5f;
    
    //[Header("Debug")]
    [Header("Data")] 
    public Vector2 tilePosition;
    
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
        EventHandler.CharacterMoveEnd += OnCharacterCancelMove; // Tile down animation
        EventHandler.AttackRangeColor += OnAttackRangeColor; // Check and Set Material to Attack Range Color
    }

    private void OnDisable()
    {
        EventHandler.TilePosYStand -= OnTilePosYStand;
        EventHandler.TilePosXStand += OnTilePosXStand;
        
        EventHandler.TilePosAddManagerList += OnTilePosAddManagerList;
        EventHandler.TileUpAnimation -= OnTileUpAnimation;
        EventHandler.CharacterMoveEnd -= OnCharacterCancelMove;
        EventHandler.AttackRangeColor -= OnAttackRangeColor;
    }

    private void OnTileUpAnimation(Character character,Vector2 skillAttackRange,  Vector2 playerPos, Vector2 distance)
    {
        tempCharacterWantToMove = character;
        tempCharacterAttackRangeDistance = skillAttackRange;
        
        if(Vector2.Distance(playerPos, tilePosition) <= distance.y && !CheckNotTempCharacterOnTile())
        {
            isStand = true;
            transform.DOMoveY(standY, standDuration);
        }
        
        if(isMouseHit)
            EventHandler.CallAttackRangeColor(tilePosition, skillAttackRange);
    }

    private void OnCharacterCancelMove()
    {
        if(isStand)
        {
            isStand = false;
            tempCharacterWantToMove = null;
            transform.DOMoveY(0, standDuration);
        }

        inAttackRange = false;
    }

    private void OnAttackRangeColor(Vector2 targetTilePos, Vector2 distance)
    {
        inAttackRange = Mathf.Abs(tilePosition.x - targetTilePos.x) < distance.x &&
                        Mathf.Abs(tilePosition.y - targetTilePos.y) < distance.y;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseHit = true;
        if(isStand)
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
            if(tempCharacterWantToMove != null)
                tempCharacterWantToMove.TileReturnClickData(gameObject, tilePosition);
        }
    } 

    #endregion

    private void Update()
    {
        UpdateTileColor();
    }

    private void UpdateTileColor()
    {
        if (isMouseHit && isStand)
            meshRenderer.material = mouseTargetMaterial;
        else if(inAttackRange)
            meshRenderer.material = inAttackRangeMaterial;
        else if(isStand)
            meshRenderer.material = canWalkMaterial;
        else
            meshRenderer.material = defaultMaterial;
    }

    /// <summary>
    /// Check if there is a character on the tile (If temp character on tile, this method will ignore it)
    /// </summary>
    /// <returns></returns>
    private bool CheckNotTempCharacterOnTile()
    {
        for(int i = 0 ; i < transform.childCount; i++)
        {
            if (!transform.GetChild(i).CompareTag("Character")) continue;
            return transform.GetChild(i) != tempCharacterWantToMove.transform;
        }
        
        return false;
    }
    
    /// <summary>
    /// Check if there is a character on the tile (If temp character on tile, this method will ignore it)
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
