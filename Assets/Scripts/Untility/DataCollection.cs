using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnvironmentVisibleDetails
{
    public GameObject GameObject;
    public Team visibleTeam;
}

[System.Serializable]
public class CharacterActionData
{
    public string actionCharacterID = "L";
    public string actionSkillName = "L";
    public SkillButtonType actionType = SkillButtonType.Empty;
    public List<Vector2> actionTilePosList = new();
}