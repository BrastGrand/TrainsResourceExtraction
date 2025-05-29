namespace Code.Gameplay.Features.Graph
{
    public class GraphEdge
    {
        public GraphNode FromNode { get; private set; }
        public GraphNode ToNode { get; private set; }
        public float Length { get; private set; }

        public GraphEdge(GraphNode fromNode, GraphNode toNode, float length)
        {
            FromNode = fromNode ?? throw new System.ArgumentNullException(nameof(fromNode));
            ToNode = toNode ?? throw new System.ArgumentNullException(nameof(toNode));
            
            if (length <= 0)
                throw new System.ArgumentException("Edge length must be positive");
                
            Length = length;
        }

        public void UpdateLength(float newLength)
        {
            if (newLength <= 0)
                throw new System.ArgumentException("Edge length must be positive");
                
            Length = newLength;
        }

        public GraphNode GetOtherNode(GraphNode currentNode)
        {
            if (currentNode == FromNode)
                return ToNode;
            if (currentNode == ToNode)
                return FromNode;
                
            throw new System.ArgumentException("Node is not part of this edge");
        }

        public float CalculateTravelTime(float speed)
        {
            return Length / speed;
        }
    }
} 