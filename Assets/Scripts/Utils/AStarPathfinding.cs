using System.Collections.Generic;
using UnityEngine;


namespace Utils
{
    public class PathNode
    {
        public int GCost;
        public int HCost;
        public int FCost => HCost + GCost;
        public Vector2Int Coord;
        public PathNode FromNode;
        public bool Walkable = true;
    }
    public class AStarPathfinding
    {
        private const int Horizontal_Move_Cost = 20;
        private const int Vertical_Move_Cost = 10;

        private FieldModel _fieldModel;
        private List<PathNode> _openPositions;
        private List<PathNode> _closePositions;
        private Dictionary<Vector2Int, PathNode> _positionToNode = new();
        
        public AStarPathfinding(FieldModel field)
        {
            _fieldModel = field;

            for (var x = 0; x < _fieldModel.GetWidth(); x++)
            {
                for (var y = 0; y < _fieldModel.GetHeight(); y++)
                {
                    var pathNode = new PathNode
                    {
                        GCost = int.MaxValue,
                        Coord = new Vector2Int(x, y),
                        FromNode = null
                    };
                    _positionToNode.Add(new Vector2Int(x, y), pathNode);
                }
            }
        }

        public List<PathNode> FindPath(int xStart, int yStart, int xTarget, int yTarget)
        {
            var startNode = _positionToNode[new Vector2Int(xStart, yStart)];
            var targetNode = _positionToNode[new Vector2Int(xTarget, yTarget)];

            _openPositions = new List<PathNode> {_positionToNode[new Vector2Int(xStart, yStart)]};
            _closePositions = new();
            foreach (var pathNode in _positionToNode)
            {
                pathNode.Value.FromNode = null;
                pathNode.Value.GCost = int.MaxValue;
                pathNode.Value.HCost = 0;
            }

            startNode.GCost = 0;
            startNode.HCost = CalculateDirectionCost(startNode, targetNode);

            while (_openPositions.Count > 0)
            {
                var currentNode = GetLowestFCostNode(_openPositions);
                if (currentNode == targetNode && targetNode.Walkable)
                    return CalculatedPath(targetNode);

                _openPositions.Remove(currentNode);
                _closePositions.Add(currentNode);

                foreach (var neighbourNode in GetNeighbours(currentNode))
                {
                    if (_closePositions.Contains(neighbourNode)) continue;

                    var tempGCost = currentNode.GCost + CalculateDirectionCost(currentNode, neighbourNode);
                    if (tempGCost < neighbourNode.GCost)
                    {
                        neighbourNode.FromNode = currentNode;
                        neighbourNode.GCost = tempGCost;
                        neighbourNode.HCost = CalculateDirectionCost(neighbourNode, targetNode);

                        if (!_openPositions.Contains(neighbourNode))
                        {
                            _openPositions.Add(neighbourNode);
                        }
                    }
                }
            }

            return null;
        }

        private List<PathNode> CalculatedPath(PathNode node)
        {
            var path = new List<PathNode>();
            path.Add(node);
            var currentNode = node;
            while (currentNode.FromNode != null)
            {
                path.Add(currentNode.FromNode);
                currentNode = currentNode.FromNode;
            }

            path.Reverse();
            return path;
        }

        public void SetWalkableState(int x, int y, bool state)
        {
            if (_positionToNode.ContainsKey(new Vector2Int(x, y)))
                _positionToNode[new Vector2Int(x, y)].Walkable = state;
        }
        
        public void Reset()
        {
            foreach (var node in _positionToNode)
            {
                node.Value.Walkable = true;
            }
        }

        private List<PathNode> GetNeighbours(PathNode currentNode)
        {
            var neighbourList = new List<PathNode>();

            if (currentNode.Coord.x - 1 >= 0 && currentNode.Walkable)
                neighbourList.Add(_positionToNode[new Vector2Int(currentNode.Coord.x - 1, currentNode.Coord.y)]);

            if (currentNode.Coord.x + 1 < _fieldModel.GetWidth() && currentNode.Walkable)
                neighbourList.Add(_positionToNode[new Vector2Int(currentNode.Coord.x + 1, currentNode.Coord.y)]);

            if (currentNode.Coord.y - 1 >= 0 && currentNode.Walkable)
                neighbourList.Add(_positionToNode[new Vector2Int(currentNode.Coord.x, currentNode.Coord.y - 1)]);

            if (currentNode.Coord.y + 1 < _fieldModel.GetHeight() && currentNode.Walkable)
                neighbourList.Add(_positionToNode[new Vector2Int(currentNode.Coord.x, currentNode.Coord.y + 1)]);

            return neighbourList;
        }

        private int CalculateDirectionCost(PathNode from, PathNode to)
        {
            var direction = (to.Coord - from.Coord);
            return Mathf.Abs(direction.x * Horizontal_Move_Cost + direction.y * Vertical_Move_Cost);
        }

        private PathNode GetLowestFCostNode(List<PathNode> pathNodes)
        {
            var lowestFCostNode = pathNodes[0];
            for (var i = 0; i < pathNodes.Count; i++)
            {
                if (pathNodes[i].FCost < lowestFCostNode.FCost)
                    lowestFCostNode = pathNodes[i];
            }

            return lowestFCostNode;
        }
    }
}