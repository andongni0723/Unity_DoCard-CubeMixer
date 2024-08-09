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

    #region Tools

    public static Action<Vector2> ReturnMouseHitTilePosition;
    public static void CallReturnMouseHitTilePosition(Vector2 pos)
    {
        ReturnMouseHitTilePosition?.Invoke(pos);
    }

    #endregion

    #region Game

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
    
    public static Action UIObjectGenerate;
    public static void CallUIObjectGenerate()
    {
        UIObjectGenerate?.Invoke();
    }
    
    
    public static Action<string, SkillDetailsSO> CharacterUseSkill;
    public static void CallCharacterUseSkill(string characterName, SkillDetailsSO skillData)
    {
        CharacterUseSkill?.Invoke(characterName, skillData);
    }

    public static Action TurnCharacterStartAction;
    public static void CallTurnCharacterStartAction()
    {
        TurnCharacterStartAction?.Invoke();
    }

    public static Action<GameState> StateCallback;
    public static void CallStateCallback(GameState state)
    {
        StateCallback?.Invoke(state);
    }
    
    public static Action<GameState> ChangeState;
    public static void CallChangeState(GameState state)
    {
        ChangeState?.Invoke(state);
    }
    
    public static Action<GameState> ChangeStateDone;
    public static void CallChangeStateDone(GameState state)
    {
        ChangeStateDone?.Invoke(state);
    }
    
    public static Action HitPanelAnyButtonPress;
    public static void CallHitPanelAnyButtonPress()
    {
        HitPanelAnyButtonPress?.Invoke();
    }
    #endregion


    #region Character

    public static Action<SkillDetailsSO, Character, Vector2, Vector2, Vector2, bool> TileUpAnimation;
    public static void CallTileUpAnimation(SkillDetailsSO data, Character character, Vector2 skillAttackRange, Vector2 playerPos, Vector2 distance, bool isStrict = false)
    {
        TileUpAnimation?.Invoke(data, character, skillAttackRange, playerPos, distance, isStrict);
    }
    
    public static Action<Vector2, Vector2> AttackRangeColor;
    public static void CallAttackRangeColor(Vector2 tilePos, Vector2 distance)
    {
        AttackRangeColor?.Invoke(tilePos, distance);
    }

    public static Action ButtonCallUseSkillEvent;
    public static void CallButtonCallUseSkillEvent()
    {
        ButtonCallUseSkillEvent?.Invoke();
    }

    public static Action CharacterChooseTileRangeDone;
    public static void CallCharacterChooseTileRangeDone()
    {
        CharacterChooseTileRangeDone?.Invoke();
    }

    public static Action<bool> CharacterActionEnd;
    public static void CallCharacterActionEnd(bool isOwner)
    {
        CharacterActionEnd?.Invoke(isOwner);
    }

    public static Action UpdateCharacterActionData;
    public static void CallUpdateCharacterActionData()
    {
        UpdateCharacterActionData?.Invoke();
    }
    
    public static Action CharacterBackToTurnStartPoint;
    public static void CallCharacterBackToTurnStartPoint()
    {
        CharacterBackToTurnStartPoint?.Invoke();
    }

    public static Action LastPlayActionEnd;
    public static void CallLastPlayActionEnd()
    {
        LastPlayActionEnd?.Invoke();
    }
    
    public static Action SelfTeamCharacterActionEnd;
    public static void CallSelfTeamCharacterActionEnd()
    {
        SelfTeamCharacterActionEnd?.Invoke();
    }

    public static Action<Character, int, int> HealthChange;
    public static void CallHealthChange(Character character, int newValue, int maxHealth)
    {
        HealthChange?.Invoke(character, newValue, maxHealth);
    }
    public static Action<Character, int, int> PowerChange;
    public static void CallPowerChange(Character character, int newValue, int maxPower)
    {
        PowerChange?.Invoke(character, newValue, maxPower);
    }
    #endregion


    #region UI

    public static Action<CharacterDetailsSO, string> CharacterCardPress;
    public static void CallCharacterCardPress(CharacterDetailsSO data, string ID)
    {
        CharacterCardPress?.Invoke(data, ID);
    }

    public static Action CharacterActionClear;
    public static void CallCharacterActionClear()
    {
        CharacterActionClear?.Invoke();
    }
    
    public static Action<float> CameraPositionValueChange;
    public static void CallCameraPositionValueChange(float value)
    {
        CameraPositionValueChange?.Invoke(value);
    }

    #endregion
    
}
