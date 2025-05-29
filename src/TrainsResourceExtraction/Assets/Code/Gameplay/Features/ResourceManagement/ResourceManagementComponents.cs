using Entitas;

namespace Code.Gameplay.Features.ResourceManagement
{
    [Game] public class Resource : IComponent { }
    [Game] public class ResourceMultiplier : IComponent { public float Value; }
    [Game] public class TotalResources : IComponent { public int Value; }
    [Game] public class ResourceDelivered : IComponent { public int Value; }
}