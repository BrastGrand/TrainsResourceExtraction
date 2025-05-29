using Entitas;

namespace Code.Gameplay.Features.TrainAI
{
    [Game] public class CurrentNode : IComponent { public int Value; }
    [Game] public class TargetNode : IComponent { public int Value; }
    [Game] public class NextNodeInPath : IComponent { public int Value; }
    [Game] public class NeedsNewTarget : IComponent { }
    [Game] public class IdleState : IComponent { }
}