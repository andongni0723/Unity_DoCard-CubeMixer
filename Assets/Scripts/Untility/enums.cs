public enum SkillButtonType
{
    Empty,
    Move,
    Attack,
    Skill
}

public enum SkillUseCondition
{
    Power,
    Count
}

public enum EffectMaxHaveCountType
{
    Infinite,
    Limit,
}

public enum EffectType
{
    None,
    Buff,
    Debuff
}

public enum StatusType
{
    None,
    SkillChange
}

public enum StatusEffectDisappearType
{
    None,
    Event,
}

public enum Team
{
    Red, Blue
}

public enum Direction
{
    Up, Down, Left, Right
}

public enum GameState
{
    LoadPlayerInGame,
    ActionState,
    FightState,
    FightEndState,
}

[System.Flags]
public enum MultipleGameState
{
    LoadPlayerInGame = 1 << 0,
    ActionState = 1 << 1,
    FightState = 1 << 2,
    FightEndState = 1 << 3,
    All = LoadPlayerInGame | ActionState | FightState | FightEndState
}