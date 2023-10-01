using System.Linq;
using UnityEngine;
using Utils;

public class TestField : MonoBehaviour
{
    public Spavner Spavner;
    public Sprite CellSprite;
    public Vector2Int SpawnPosition;
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
            var path = _pathfind.FindPath(SpawnPosition.x, SpawnPosition.y, targetPosition.x, targetPosition.y);
            if (path != null)
            {
                for (var i = 0; i < path.Count; i++)
                    _fieldModel.SetColor(path[i].Coord.x, path[i].Coord.y, Color.green);

                var worldPath = path.Select(it => _fieldModel.GetWorldCenterPosition(it.Coord.x, it.Coord.y, -1))
                    .ToList();
                var spawnPos = _fieldModel.GetWorldCenterPosition(SpawnPosition.x, SpawnPosition.y, -1);
                Spavner.StartSpawn(spawnPos, worldPath);
            }
            else
                Debug.Log("No Path");
        }

        if (Input.GetMouseButtonDown(1))
        {
            var targetPosition = _fieldModel.GetCellPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            _pathfind.SetWalkableState(targetPosition.x, targetPosition.y, false);
            _fieldModel.SetColor(targetPosition.x, targetPosition.y, Color.red);
        }

        if (Input.GetMouseButtonDown(2))
        {
            _fieldModel.Reset();
            _pathfind.Reset();
        }
    }
}