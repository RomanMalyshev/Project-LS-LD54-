using System;
using UnityEngine;
using Utils;

public class TestField : MonoBehaviour
{
    private FieldModel _fieldModel;
    private AStarPathfinding _pathfind;
    private int _cellSize = 5;
    private Vector3 _originPosition;
    private void Start()
    {
        _originPosition = transform.position;
        _fieldModel = new FieldModel(10, 10, _cellSize, _originPosition);
        _pathfind = new AStarPathfinding(_fieldModel);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var targetPosition = _fieldModel.GetCellPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            var path = _pathfind.FindPath(0, 0, targetPosition.x, targetPosition.y);
            if (path != null)
                for (var i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(
                        new Vector3(path[i].Coord.x, path[i].Coord.y) * _cellSize + Vector3.one * _cellSize / 2f + _originPosition,
                        new Vector3(path[i + 1].Coord.x, path[i + 1].Coord.y) * _cellSize+Vector3.one * _cellSize / 2f +_originPosition,
                        Color.green, 1000f);
                }

            else
            {
                Debug.Log("No Path");
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            _fieldModel.Reset();
        }
    }
}