    using UnityEngine;
using UnityEngine.UI;
using Utils;

public class TestField : MonoBehaviour
{
    public Level Level;
    public Button Restart;
    public Button StartLevel;
    private FieldModel _fieldModel;
    private AStarPathfinding _pathfind;

    private Spavner _spavner;
    private GameObject _levelContainer;

    private void Start()
    {
        Restart.onClick.AddListener(() => Destroy(_levelContainer));
        StartLevel.onClick.AddListener(CreateLevel);

        CreateLevel();
    }

    private void CreateLevel()
    {
        _levelContainer = new GameObject("LevelContainer");

        _fieldModel = new FieldModel(Level.FieldWidth, Level.FieldHeight, Level.CellSize, Level.OriginPosition,
            Level.CellSprite, _levelContainer.transform);
        _pathfind = new AStarPathfinding(_fieldModel);
        _spavner = Instantiate(Level.Spavner, _levelContainer.transform);
        var spawnerPosition = _fieldModel.GetWorldCenterPosition(Level.SpawnerPosition.x, Level.SpawnerPosition.y, -1);
        _spavner.transform.position = spawnerPosition;

        var mainBuilding = Instantiate(Level.MainBuilding,
            _fieldModel.GetWorldCenterPosition(Level.MainBuildingPosition.x, Level.MainBuildingPosition.y, -1),
            Quaternion.identity, _levelContainer.transform);

        foreach (var fieldObject in Level.FieldObjects)
        {
            Instantiate(fieldObject.Object,
                _fieldModel.GetWorldCenterPosition(fieldObject.Position.x, fieldObject.Position.y, -1),
                Quaternion.identity, _levelContainer.transform);
            _pathfind.SetWalkableState(fieldObject.Position, false);
        }

        _spavner.StartSpawn(_pathfind, mainBuilding.transform.position);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var targetPosition = _fieldModel.GetCellPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            _pathfind.SetWalkableState(targetPosition.x, targetPosition.y, false);
            _fieldModel.SetSprite(targetPosition.x, targetPosition.y, Level.WallSelf);
        }

        if (Input.GetMouseButtonDown(1))
        {
            _fieldModel.Reset();
            _pathfind.Reset();
        }
    }
}