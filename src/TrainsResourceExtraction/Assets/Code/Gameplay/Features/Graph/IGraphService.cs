using System.Collections.Generic;

namespace Code.Gameplay.Features.Graph
{
    public interface IGraphService
    {
        void AddNode(GraphNode node);
        void AddEdge(GraphEdge edge);
        GraphNode GetNode(int nodeId);
        List<GraphNode> GetAllNodes();
        List<GraphNode> GetConnectedNodes(int nodeId);
        GraphEdge GetEdge(int fromNodeId, int toNodeId);
        List<GraphEdge> GetEdgesFromNode(int nodeId);
        List<GraphNode> FindShortestPath(int fromNodeId, int toNodeId);
        List<GraphNode> GetNodesByType(NodeType nodeType);
    }
}