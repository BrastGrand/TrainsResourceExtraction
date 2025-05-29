using UnityEngine;

namespace Code.Gameplay.Features.Graph
{
    public class EmptyNode : GraphNode
    {
        public EmptyNode(int id, Vector3 position) : base(id, position)
        {
        }

        public override NodeType GetNodeType() => NodeType.Empty;
        public override float GetMultiplier() => 1.0f; // Промежуточные узлы не имеют множителя
    }
} 