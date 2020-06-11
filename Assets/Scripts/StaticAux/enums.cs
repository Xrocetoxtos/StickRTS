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

//public enum CharacterAnimationState
//{
//    Idle,
//    WalkLR,
//    WalkRL,
//    WalkUD,
//    WalkDU,
//    PickUp,
//    Mine,
//    CutTree,
//    Fight
//}

public enum CharacterAnimationState
{
    Idle,
    Walk,
    Pickup,
    Mine,
    CutTree,
    FightStab,
    FightSwing,
    FightShoot
}

public enum CharacterAnimationDirection
{
    Left,
    Right,
    Up,
    Down
}

public enum ColliderValue
{
    Nothing,
    Ground,
    Water,
    Object
}