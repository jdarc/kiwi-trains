using System;
using System.Collections.Generic;

namespace KiwiTrains
{
    internal class LinkedNode
    {
        private readonly RouteManager _manager;
        private readonly bool[] _visited;

        private Edge RouteEdge { get; }

        private Node RouteNode { get; }

        public LinkedNode(RouteManager rm, Node fromNode)
        {
            _manager = rm;
            RouteNode = fromNode;
            _visited = new bool[rm.Matrix.Size];
        }

        private LinkedNode ParentNode { get; }

        public IEnumerable<Journey> GenerateRoutes(Node to, int depth, bool depthSearch, bool additive)
        {
            var leafNodes = new List<LinkedNode>();
            if (additive)
            {
                for (var i = 1; i < depth; i++)
                {
                    GenerateRoutesRecursive(leafNodes, to, i, depthSearch);
                }
            }
            else
            {
                GenerateRoutesRecursive(leafNodes, to, depth, depthSearch);
            }

            var journies = new List<Journey>();
            foreach (var tnode in leafNodes)
            {
                var j = new Journey();
                var current = tnode;
                while (current.RouteEdge != null)
                {
                    j.AddRoute(current.RouteEdge);
                    current = current.ParentNode;
                }

                j.Reverse();
                journies.Add(j);
            }

            return journies.ToArray();
        }

        private LinkedNode(RouteManager rm, Edge routeEdge, LinkedNode parent) : this(rm, routeEdge.To)
        {
            RouteEdge = routeEdge;
            ParentNode = parent;
        }

        private void GenerateRoutesRecursive(ICollection<LinkedNode> leafNodes, Node to, int depth, bool depthSearch)
        {
            _visited[RouteNode.Index] = true;
            var matrix = _manager.Matrix;
            for (var i = 0; i < matrix.Size; i++)
            {
                if (matrix.GetElementAt(RouteNode.Index, i) > 0)
                {
                    var edge = _manager.FindEdge(RouteNode.Index, i);
                    var nextNode = new LinkedNode(_manager, edge, this);
                    Array.Copy(_visited, nextNode._visited, _visited.Length);

                    if (depthSearch)
                    {
                        if (depth > 0)
                        {
                            nextNode.GenerateRoutesRecursive(leafNodes, to, depth - 1, true);
                        }
                        else
                        {
                            leafNodes.Add(nextNode);
                        }
                    }
                    else
                    {
                        if (nextNode.RouteNode != to)
                        {
                            if (!_visited[edge.To.Index])
                            {
                                nextNode.GenerateRoutesRecursive(leafNodes, to, depth - 1, false);
                            }
                        }
                        else
                        {
                            leafNodes.Add(nextNode);
                        }
                    }
                }
            }
        }
    }
}