using System.Linq;
using UnityEngine;
using Utils;

public class TestField : MonoBehaviour
{
    public Level Level;
    private FieldModel _fieldModel;
    private AStarPathfinding _pathfind;

    private Spavner _spavner;
    
    private void Start()
    {
        _fieldModel = new FieldModel(Level.FieldWidth, Level.FieldHeight, Level.CellSize, Level.OriginPosition, Level.CellSprite);
        _pathfind = new AStarPathfinding(_fieldModel);
        _spavner = Instantiate(Level.Spavner);
        _spavner.transform.position =_fieldModel.GetWorldCenterPosition(Level.SpawnerPosition.x,Level.SpawnerPosition.y,-1);
        Instantiate(Level.MainBuilding,_fieldModel.GetWorldCenterPosition(Level.MainBuildingPosition.x,Level.MainBuildingPosition.y,-1),Quaternion.identity);

        foreach (var fieldObject in Level.FieldObjects)
        {
            Instantiate(fieldObject.Object,_fieldModel.GetWorldCenterPosition(fieldObject.Position.x,fieldObject.Position.y,-1),Quaternion.identity);
 
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var targetPosition = _fieldModel.GetCellPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            var path = _pathfind.FindPath(Level.SpawnerPosition.x, Level.SpawnerPosition.y, Level.MainBuildingPosition.x, Level.MainBuildingPosition.y);
            if (path != null)
            {
                for (var i = 0; i < path.Count; i++)
                    _fieldModel.SetColor(path[i].Coord.x, path[i].Coord.y, Color.green);

                var worldPath = path.Select(it => _fieldModel.GetWorldCenterPosition(it.Coord.x, it.Coord.y, -1))
                    .ToList();
                var spawnPos = _fieldModel.GetWorldCenterPosition(Level.SpawnerPosition.x, Level.SpawnerPosition.y, -1);
                _spavner.StartSpawn(spawnPos, worldPath);
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