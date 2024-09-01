using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterActionRecord : MonoBehaviour
{
    //[Header("Component")]
    //[Header("Settings")]
    [Header("Debug")]
    public List<CharacterActionData> characterActionDataList = new();

    // ----------------- Event -----------------
    private void OnEnable()
    {
        EventHandler.CharacterActionClear += ClearCharacterActionData;
    }
    
    private void OnDisable()
    {
        EventHandler.CharacterActionClear -= ClearCharacterActionData;
    }

    private void ClearCharacterActionData()
    {
        characterActionDataList.Clear();
    }

    // ----------------- Tools -----------------
    public void AddCharacterActionData(string id, SkillDetailsSO skillData, List<TileReturnData> tileReturnDataList)
    {
        List<Vector2> tilePosList = new();
        foreach (var data in tileReturnDataList)
        {
            tilePosList.Add(data.targetTilePos);
        }
        
        characterActionDataList.Add(new CharacterActionData()
        {
            actionCharacterID = id,
            actionSkillName = skillData.skillID,
            actionType = skillData.skillType,
            actionTilePosList = tilePosList
        });
        
        EventHandler.CallCharacterUseSkill(
            DetailsManager.Instance.UseCharacterIDSearchCharacter(id).characterDetails.characterName, skillData);
    }

    
    // - List Data
    // [0]:
    // characterID = "B01"
    // skillID = "S01"
    // type = SkillButtonType.Attack
    // tileReturnDataList = { (1, 1), (1, 2), (1, 3) }
    // [1]:
    // ...
    //
    // - String Data
    // "{B01,S01,0,(1_1|1_2|1_3)}{...}"
    public string ListDataToStringData()
    {
        var result = string.Empty;
        
        foreach (var data in characterActionDataList)
        {
            // (1, 1), (1, 2), (1, 3) -> 1_1|1_2|1_3
            var tilePosList = data.actionTilePosList
                .Aggregate(string.Empty, (current, tilePos) => current + $"{tilePos.x}_{tilePos.y}|")
                .TrimEnd('|');
            
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
            if (data.Length < 3) continue;
            
            var clearData = data.Substring(1, data.Length - 1); // xxx, xxx, xxx, (xxx|xxx|xxx)
            var clearDataList = clearData.Split(',');
            var tilePosStringList = 
                clearDataList[3]
                    .Substring(1, clearDataList[3].Length - 2)
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
