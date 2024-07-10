using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Test : MonoBehaviour
{
    [Header("Component")] 
    public GameObject prefab;
    [Header("Settings")]
    public int count;

    //[Header("Debug")]

    public void BounceTest()
    {
        Vector2 randomVe2 = new Vector2(Random.Range(0, 7), Random.Range(0, 14));
        Tile randomTile = GridManager.Instance.GetTileWithTilePos(randomVe2);
        Instantiate(prefab, randomTile.transform.position + (Vector3.up * 10), Quaternion.Euler(53, -90, 0));
    }
    
}
