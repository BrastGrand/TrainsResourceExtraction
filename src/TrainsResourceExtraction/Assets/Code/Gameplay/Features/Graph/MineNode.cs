using UnityEngine;

namespace Code.Gameplay.Features.Graph
{
    public class MineNode : GraphNode
    {
        public float MiningTimeMultiplier { get; private set; }

        public MineNode(int id, Vector3 position, float miningTimeMultiplier = 1.0f) : base(id, position)
        {
            MiningTimeMultiplier = miningTimeMultiplier;
        }

        public void UpdateMiningTimeMultiplier(float newMultiplier)
        {
            if (newMultiplier <= 0)
                throw new System.ArgumentException("Mining time multiplier must be positive");
                
            MiningTimeMultiplier = newMultiplier;
        }

        public float CalculateMiningTime(float baseMiningTime)
        {
            return baseMiningTime * MiningTimeMultiplier;
        }

        public override NodeType GetNodeType() => NodeType.Mine;
        public override float GetMultiplier() => MiningTimeMultiplier;
    }
} 