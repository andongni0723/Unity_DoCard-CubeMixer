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
    private Material defaultMaterial;
    
    [Space(15)]
    public float standY;
    public float standDuration = 0.5f;
    
    //[Header("Debug")]
    [Header("Data")] 
    public Vector2 tilePosition;

    private bool isStand = false;
    private bool isMouseHit = false;
    private Character tempCharacterWantToMove;

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
        EventHandler.TilePosYStand += OnTilePosYStand;
        EventHandler.TilePosXStand += OnTilePosXStand;
        EventHandler.TilePosAddManagerList += OnTilePosAddManagerList;
        
        EventHandler.CharacterNearTileAnimation += OnCharacterNearTileAnimation;
        EventHandler.CharacterCancelMove += OnCharacterCancelMove;
    }

    private void OnDisable()
    {
        EventHandler.TilePosYStand -= OnTilePosYStand;
        EventHandler.TilePosXStand += OnTilePosXStand;
        
        EventHandler.TilePosAddManagerList += OnTilePosAddManagerList;
        EventHandler.CharacterNearTileAnimation -= OnCharacterNearTileAnimation;
        EventHandler.CharacterCancelMove -= OnCharacterCancelMove;
    }

    private void OnCharacterNearTileAnimation(Character character, Vector2 playerPos, int distance)
    {
        if(Vector2.Distance(playerPos, tilePosition) <= distance)
        {
            isStand = true;
            tempCharacterWantToMove = character;
            transform.DOMoveY(standY, standDuration);
        }
    }
    
    private void OnCharacterCancelMove()
    {
        if(isStand)
        {
            isStand = false;
            tempCharacterWantToMove = null;
            transform.DOMoveY(0, standDuration);
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseHit = true;
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
                tempCharacterWantToMove.TileCallClickAction(gameObject, tilePosition);
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
        else if(isStand)
            meshRenderer.material = canWalkMaterial;
        else
            meshRenderer.material = defaultMaterial;
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
