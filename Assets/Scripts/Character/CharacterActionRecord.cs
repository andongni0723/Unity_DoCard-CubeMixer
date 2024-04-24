using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActionRecord : MonoBehaviour
{
    //[Header("Component")]
    //[Header("Settings")]
    //[Header("Debug")]
    public List<CharacterActionData> characterActionDataList = new();
    
    public void AddCharacterActionData(string id, string skillID, SkillButtonType type, List<TileReturnData> tileReturnDataList)
    {
        List<Vector2> tilePosList = new();
        foreach (var data in tileReturnDataList)
        {
            tilePosList.Add(data.targetTilePos);
        }
        
        characterActionDataList.Add(new CharacterActionData()
        {
            actionCharacterID = id,
            actionSkillName = skillID,
            actionType = type,
            actionTilePosList = tilePosList
        });
    }

    // - List Data
    // [0]:
    // characterID = "B01"
    // skillID = "S01"
    // type = SkillButtonType.Attack
    // tileReturnDataList = { (1, 1), (1, 2), (1, 3) }
    // [1]:
    // ...
    
    // - String Data
    // "{B01,S01,0,(1_1|1_2|1_3)}{...}"
    
    public string ListDataToStringData()
    {
        var result = string.Empty;
        
        foreach (var data in characterActionDataList)
        {
            var tilePosList = string.Empty;
            // var tilePosList = data.actionTilePosList.Aggregate(string.Empty, (current, tilePos) => current + $"{tilePos.x},{tilePos.y}|");

            foreach (var tilePos in data.actionTilePosList)
            {
                tilePosList += $"{tilePos.x}_{tilePos.y}|";
            }
            tilePosList = tilePosList.Remove(tilePosList.Length - 1);
            
            result += $"{{{data.actionCharacterID},{data.actionSkillName},{(int)data.actionType},({tilePosList})}}";
        }

        return result;
    }

    // - String Data
    // "{B01,S01,0,(1_1|1_2|1_3)}{...}"
    public void StringDataToListData(string stringData)
    {
        characterActionDataList.Clear();
        
        var dataList = stringData.Split('}'); 
        foreach (var data in dataList) // data have "{..."
        {
            if (data.Length < 3)
            {
                continue;
            }
            
            var clearData = data.Substring(1, data.Length - 1); // xxx, xxx, xxx, (xxx|xxx|xxx)
            var clearDataList = clearData.Split(',');
            var tilePosStringList = clearDataList[3].Substring(1, clearDataList[3].Length - 2)
                                                    .Split('|'); // (1_2|2_3|3_4) -> 1_2, 2_3, 3_4

            List<Vector2> tilePosVector2List = new();
            foreach (var tilePos in tilePosStringList)
            {
                var pairStringPosList = tilePos.Split('_'); // "2", "3"
                
                tilePosVector2List.Add(new Vector2(int.Parse(pairStringPosList[0]), int.Parse(pairStringPosList[1])));
            }
            
            characterActionDataList.Add(new CharacterActionData()
            {
                actionCharacterID = clearDataList[0],
                actionSkillName = clearDataList[1],
                actionType = (SkillButtonType)int.Parse(clearDataList[2]),
                actionTilePosList = tilePosVector2List
            });
        }
    }
}
