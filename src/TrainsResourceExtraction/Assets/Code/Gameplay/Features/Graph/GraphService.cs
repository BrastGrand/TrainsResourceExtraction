using System.Collections.Generic;
using System.Linq;

namespace Code.Gameplay.Features.Graph
{
    public class GraphService : IGraphService
    {
        private readonly Dictionary<int, GraphNode> _nodes = new();
        private readonly List<GraphEdge> _edges = new();

        public void AddNode(GraphNode node)
        {
            if (node == null)
                throw new System.ArgumentNullException(nameof(node));
                
            _nodes[node.Id] = node;
        }

        public void AddEdge(GraphEdge edge)
        {
            if (edge == null)
                throw new System.ArgumentNullException(nameof(edge));
                
            _edges.Add(edge);
            edge.FromNode.AddConnection(edge);
            edge.ToNode.AddConnection(edge);
        }

        public GraphNode GetNode(int nodeId)
        {
            return _nodes.TryGetValue(nodeId, out var node) ? node : null;
        }

        public List<GraphNode> GetAllNodes()
        {
            return _nodes.Values.ToList();
        }

        public List<GraphNode> GetConnectedNodes(int nodeId)
        {
            var node = GetNode(nodeId);
            if (node == null) return new List<GraphNode>();

            return node.Connections.Select(edge => edge.GetOtherNode(node)).ToList();
        }

        public GraphEdge GetEdge(int fromNodeId, int toNodeId)
        {
            var fromNode = GetNode(fromNodeId);
            var toNode = GetNode(toNodeId);
            
            if (fromNode == null || toNode == null) return null;

            return _edges.FirstOrDefault(edge => 
                (edge.FromNode == fromNode && edge.ToNode == toNode) ||
                (edge.FromNode == toNode && edge.ToNode == fromNode));
        }

        public List<GraphEdge> GetEdgesFromNode(int nodeId)
        {
            var node = GetNode(nodeId);
            if (node == null) return new List<GraphEdge>();

            return node.Connections.ToList();
        }

        public List<GraphNode> FindShortestPath(int fromNodeId, int toNodeId)
        {
            // Dijkstra's algorithm
            var distances = new Dictionary<int, float>();
            var previous = new Dictionary<int, GraphNode>();
            var unvisited = new HashSet<GraphNode>(_nodes.Values);

            foreach (var node in _nodes.Values)
            {
                distances[node.Id] = float.MaxValue;
            }
            distances[fromNodeId] = 0;

            while (unvisited.Count > 0)
            {
                var current = unvisited.OrderBy(node => distances[node.Id]).First();
                unvisited.Remove(current);

                if (current.Id == toNodeId)
                    break;

                foreach (var edge in current.Connections)
                {
                    var neighbor = edge.GetOtherNode(current);
                    if (!unvisited.Contains(neighbor)) continue;

                    var tentativeDistance = distances[current.Id] + edge.Length;
                    if (tentativeDistance < distances[neighbor.Id])
                    {
                        distances[neighbor.Id] = tentativeDistance;
                        previous[neighbor.Id] = current;
                    }
                }
            }

            // Reconstruct path
            var path = new List<GraphNode>();
            var currentNode = GetNode(toNodeId);
            
            while (currentNode != null)
            {
                path.Insert(0, currentNode);
                previous.TryGetValue(currentNode.Id, out currentNode);
            }

            return path.Count > 1 ? path : new List<GraphNode>();
        }

        public List<GraphNode> GetNodesByType(NodeType nodeType)
        {
            return _nodes.Values.Where(node => node.GetNodeType() == nodeType).ToList();
        }
    }
}