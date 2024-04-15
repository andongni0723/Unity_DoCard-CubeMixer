using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GridManager : Singleton<GridManager>
{
    //[Header("Component")]
    [Header("Settings")]
    private List<List<Tile>> tilesList = new();

    //[Header("Debug")]

    public override void Awake()
    {
        base.Awake();
        
        try
        {
            for (int i = 0; i <= 19; i++)
            {
                List<Tile> tiles = new();
            
                int targetJ = i >= 17 ? 4 : 7;
                for (int j = 0; j <= targetJ; j++)
                {
                    tiles.Add(transform.GetChild(i).transform.GetChild(j).GetComponent<Tile>());
                }
                tilesList.Add(tiles);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            throw;
        }
    }
    
    public Tile GetTileWithTilePos(int x, int y)
    {
        // Debug.Log($"GetTileWithTilePos x: {x}, y: {y}");
        return tilesList[y][x];
    }
}
