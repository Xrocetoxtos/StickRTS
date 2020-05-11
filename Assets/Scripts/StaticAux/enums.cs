public enum ObjectType
{
    Character,
    Resource,
    Building
}

public enum ResourceType
{
    None,
    Food,
    Wood,
    Gold,
    Stone
}

public enum ActionType
{
    Science,
    Recruit,
    Build
}

public enum CharacterAIState
{
    Idle,
    MovingToPosition,
    MovingToResource,
    GatheringResource,
    MovingToStorage,
    DroppingResource,
    MovingToBuildingProject,
    Building
}

public enum CharacterAITask
{
    None,
    Gather,
    Build
}

public enum CharacterAnimationState
{
    Idle,
    WalkLR,
    WalkRL,
    WalkUD,
    WalkDU,
    PickUp,
    Mine,
    CutTree
}

public enum ColliderValue
{
    Nothing,
    Ground,
    Water,
    Object
}