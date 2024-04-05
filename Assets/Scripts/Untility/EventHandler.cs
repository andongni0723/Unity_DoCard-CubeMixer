using System;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler
{
    public static Action<int> TilePosXStand;
    public static void CallTilePosXStand(int x)
    {
        TilePosXStand?.Invoke(x);
    }
    public static Action<int> TilePosYStand;
    public static void CallTilePosYStand(int y)
    {
        TilePosYStand?.Invoke(y);
    }
    public static Action<List<Grid>, Vector2> TilePosAddManagerList;
    public static void OnTilePosAddManagerList(List<Grid> grids,  Vector2 pos)
    {
        TilePosAddManagerList?.Invoke(grids, pos);
    }

    #region Game

    public static Action<List<CharacterDetailsSO>> PlayerCharactersInitialized;
    public static void CallPlayerCharactersInitialized(List<CharacterDetailsSO> data)
    {
        PlayerCharactersInitialized?.Invoke(data);
    }
    
    public static Action<List<CharacterDetailsSO>> EnemyCharactersInitialized;
    public static void CallEnemyCharactersInitialized(List<CharacterDetailsSO> data)
    {
        EnemyCharactersInitialized?.Invoke(data);
    }

    public static Action ReturnCharacterInitializedDone;
    public static void CallReturnCharacterInitializedDone()
    {
        ReturnCharacterInitializedDone?.Invoke();
    }
    
    public static Action CharacterObjectGenerate;
    public static void CallCharacterObjectGenerate()
    {
        CharacterObjectGenerate?.Invoke();
    }
    
    public static Action CharacterObjectGeneratedDone;
    public static void CallCharacterObjectGeneratedDone()
    {
        CharacterObjectGeneratedDone?.Invoke();
    } 
    #endregion


    #region Character

    public static Action<Character, Vector2, Vector2, Vector2> TileUpAnimation;
    public static void CallTileUpAnimation(Character character, Vector2 skillAttackRange, Vector2 playerPos, Vector2 distance)
    {
        TileUpAnimation?.Invoke(character, skillAttackRange, playerPos, distance);
    }
    
    public static Action<Vector2, Vector2> AttackRangeColor;
    public static void CallAttackRangeColor(Vector2 tilePos, Vector2 distance)
    {
        AttackRangeColor?.Invoke(tilePos, distance);
    }

    public static Action CharacterMoveEnd;
    public static void CallCharacterMoveEnd()
    {
        CharacterMoveEnd?.Invoke();
    }

    #endregion


    #region UI

    public static Action<CharacterDetailsSO> CharacterCardPress;
    public static void CallCharacterCardPress(CharacterDetailsSO data)
    {
        CharacterCardPress?.Invoke(data);
    } 

    #endregion
    
}
