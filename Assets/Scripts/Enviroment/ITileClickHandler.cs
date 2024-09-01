using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITileClickHandler
{
    void TileReturnClickData(GameObject tileGameObject, Vector2 targetTilePos);
}

