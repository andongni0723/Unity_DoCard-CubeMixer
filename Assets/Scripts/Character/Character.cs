using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class TileReturnData
{
    public GameObject tileGameObject;
    public Vector2 targetTilePos;
}

public class Character : MonoBehaviour
{
    //[Header("Component")]
    [Header("Settings")] 
    [Range(0, 20)]public int moveMaxDistance;
    
    [Header("Debug")]
    public Vector2 characterTilePosition;
    private Camera mainCamera;
    private bool isTileReturn = false;
    private TileReturnData skillTileReturnData;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void Start()
    {
        // StartCoroutine(Test());
        transform.parent = GridManager.Instance
            .GetTileWithTilePos((int)characterTilePosition.x, (int)characterTilePosition.y).transform;
    }

    #region Event

    private void OnEnable()
    {
        EventHandler.CharacterCancelMove += OnCharacterCancelMove;
    }

    private void OnDisable()
    {
        EventHandler.CharacterCancelMove -= OnCharacterCancelMove;
    }

    private void OnCharacterCancelMove()
    {
        isTileReturn = false;
    }

    #endregion
    
    /// <summary>
    /// Call by skill button, when skill button click
    /// </summary>
    /// <param name="skillDetails"></param>
    public async void ButtonCallUseSkill(SkillDetailsSO skillDetails)
    {
        await CallTileStandAnimation(skillDetails.range);
        switch (skillDetails.skillType)
        {
            case SkillButtonType.Empty:
                Debug.LogError("The skill button is not bind with any skill");
                break;
            
            case SkillButtonType.Move:
                await UniTask.WaitUntil(() => isTileReturn); // back the skill tile return data
                await MoveAction(skillTileReturnData.tileGameObject, skillTileReturnData.targetTilePos);
                EventHandler.CallCharacterCancelMove();
                break;
                
        }
    }

    /// <summary>
    /// Call by tile, when mouse click the target tile
    /// </summary>
    /// <param name="tileGameObject">the tile the mouse click</param>
    /// <param name="targetTilePos"></param>
    public void TileCallClickAction(GameObject tileGameObject, Vector2 targetTilePos)
    {
        isTileReturn = true;
        
        skillTileReturnData = new TileReturnData
        {
            tileGameObject = tileGameObject,
            targetTilePos = targetTilePos
        };
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

    private async UniTask CallTileStandAnimation(float maxStandDistance)
    {
        Vector2 beforeMoveTilePos = characterTilePosition;
            
        for (int i = 0; i <= maxStandDistance; i++)
        {
            EventHandler.CallCharacterNearTileAnimation(this, beforeMoveTilePos, i);
            await UniTask.Delay(TimeSpan.FromSeconds(0.1));
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
