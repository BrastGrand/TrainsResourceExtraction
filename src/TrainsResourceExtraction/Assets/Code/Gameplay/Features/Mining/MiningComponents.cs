using Entitas;

namespace Code.Gameplay.Features.Mining
{
    [Game] public class Mining : IComponent { }
    [Game] public class MiningTime : IComponent { public float Value; }
    [Game] public class MiningTimeMultiplier : IComponent { public float Value; }
    [Game] public class BaseMiningTime : IComponent { public float Value; }
    [Game] public class MiningCompleted : IComponent { }
} 