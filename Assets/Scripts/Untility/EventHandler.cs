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


    #region Character

    public static Action<Character, Vector2, int> CharacterNearTileAnimation;
    public static void CallCharacterNearTileAnimation(Character character, Vector2 playerPos, int distance)
    {
        CharacterNearTileAnimation?.Invoke(character, playerPos, distance);
    }

    public static Action CharacterCancelMove;
    public static void CallCharacterCancelMove()
    {
        CharacterCancelMove?.Invoke();
    }
    
    // public static Action<GameObject, Vector2> CharacterMove;
    // public static void OnCharacterMove(GameObject tileGameObject, Vector2 tilePos)
    // {
    //     CharacterMove?.Invoke(tileGameObject, tilePos);
    // }

    #endregion
    
}
