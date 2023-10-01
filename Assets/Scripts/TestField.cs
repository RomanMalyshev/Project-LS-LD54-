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
        var spawnerPosition = _fieldModel.GetWorldCenterPosition(Level.SpawnerPosition.x,Level.SpawnerPosition.y,-1);
        _spavner.transform.position = spawnerPosition;
      
        var mainBuilding = Instantiate(Level.MainBuilding,_fieldModel.GetWorldCenterPosition(Level.MainBuildingPosition.x,Level.MainBuildingPosition.y,-1),Quaternion.identity);
        
        foreach (var fieldObject in Level.FieldObjects)
        {
            Instantiate(fieldObject.Object,_fieldModel.GetWorldCenterPosition(fieldObject.Position.x,fieldObject.Position.y,-1),Quaternion.identity);
            _pathfind.SetWalkableState(fieldObject.Position,false);
        }
        _spavner.StartSpawn(_pathfind,mainBuilding.transform.position);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var targetPosition = _fieldModel.GetCellPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            _pathfind.SetWalkableState(targetPosition.x, targetPosition.y, false);
            _fieldModel.SetColor(targetPosition.x, targetPosition.y, Color.red);
        }

        if (Input.GetMouseButtonDown(1))
        {
            _fieldModel.Reset();
            _pathfind.Reset();
        }
    }
}