using UnityEngine;

namespace Code.Gameplay.Features.Graph
{
    public class BaseNode : GraphNode
    {
        public float ResourceMultiplier { get; private set; }

        public BaseNode(int id, Vector3 position, float resourceMultiplier = 1.0f) 
            : base(id, position)
        {
            ResourceMultiplier = resourceMultiplier;
        }

        public void UpdateResourceMultiplier(float newMultiplier)
        {
            if (newMultiplier <= 0)
                throw new System.ArgumentException("Resource multiplier must be positive");
                
            ResourceMultiplier = newMultiplier;
        }

        public int ProcessResourceDelivery(int baseResourceAmount)
        {
            var deliveredAmount = Mathf.RoundToInt(baseResourceAmount * ResourceMultiplier);
            return deliveredAmount;
        }

        public override NodeType GetNodeType() => NodeType.Base;
        public override float GetMultiplier() => ResourceMultiplier;
    }
} 