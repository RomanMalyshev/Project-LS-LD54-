using System;
using UnityEngine;
using Utils;

public class TestField : MonoBehaviour
{
    public Sprite CellSprite;
    private FieldModel _fieldModel;
    private AStarPathfinding _pathfind;
    private int _cellSize = 5;
    private Vector3 _originPosition;

    private void Start()
    {
        _originPosition = transform.position;
        _fieldModel = new FieldModel(10, 10, _cellSize, _originPosition, CellSprite);
        _pathfind = new AStarPathfinding(_fieldModel);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var targetPosition = _fieldModel.GetCellPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            var path = _pathfind.FindPath(3, 5, targetPosition.x, targetPosition.y);
            if (path != null)
                for (var i = 0; i < path.Count; i++)
                {
                    _fieldModel.SetColor(path[i].Coord.x,path[i].Coord.y,Color.green);
                }

            else
            {
                Debug.Log("No Path");
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            var targetPosition = _fieldModel.GetCellPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            _pathfind.SetWalkableState(targetPosition.x,targetPosition.y,false);
            _fieldModel.SetColor(targetPosition.x,targetPosition.y,Color.red);
        }
        
        if (Input.GetMouseButtonDown(2))
        {
            _fieldModel.Reset();
            _pathfind.Reset();
        }
    }
}