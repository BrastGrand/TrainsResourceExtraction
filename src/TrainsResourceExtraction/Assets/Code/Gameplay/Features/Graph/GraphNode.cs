using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Gameplay.Features.Graph
{
    public abstract class GraphNode
    {
        public int Id { get; private set; }
        public Vector3 Position { get; private set; }
        public List<GraphEdge> Connections { get; private set; }

        protected GraphNode(int id, Vector3 position)
        {
            Id = id;
            Position = position;
            Connections = new List<GraphEdge>();
        }

        public void AddConnection(GraphEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));
                
            Connections.Add(edge);
        }

        public abstract NodeType GetNodeType();
        public abstract float GetMultiplier();
    }

    public enum NodeType
    {
        Base,
        Mine,
        Empty
    }
} 