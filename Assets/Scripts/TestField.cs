using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class TestField : MonoBehaviour
{
    public List<Level> Levels;
    public Button Restart;
    public Button StartLevel;
    private FieldModel _fieldModel;
    private AStarPathfinding _pathfind;

    private Spavner _spavner;
    private GameObject _levelContainer;
    private View _view;
    private int _levelCount = 0;
    private int _wallsAvalible;   


    private void Start()
    {
        _view = Globals.Global.View;
        Restart.onClick.AddListener(() =>
        {
            Destroy(_levelContainer);
            CreateLevel();
        });

        StartLevel.onClick.AddListener(NextLevel);

        _view.OnLevelWin.Subscribe(() =>
        {
            Destroy(_levelContainer);
        });

        _view.OnLevelLost.Subscribe(() =>
        {
            Destroy(_levelContainer);
        });        

        CreateLevel();
    }  

    private void NextLevel()
    {
        if (_levelCount + 1 < Levels.Count)
        {
            _levelCount++;
        }
        else
        {
            _levelCount = 0;
        }
        CreateLevel();
    }

    private void CreateLevel()
    {
        _view.OnLevelStart.Invoke();

        _wallsAvalible = Levels[_levelCount].WallsCount;
        Debug.Log(_wallsAvalible);
        _view.OnWallsCountChange.Invoke(_wallsAvalible);

        _levelContainer = new GameObject("LevelContainer");

        _fieldModel = new FieldModel(Levels[_levelCount].FieldWidth, Levels[_levelCount].FieldHeight, Levels[_levelCount].CellSize, Levels[_levelCount].OriginPosition,
            Levels[_levelCount].CellSprite, _levelContainer.transform);
        _pathfind = new AStarPathfinding(_fieldModel);
        _spavner = Instantiate(Levels[_levelCount].Spavner, _levelContainer.transform);
        var spawnerPosition = _fieldModel.GetWorldCenterPosition(Levels[_levelCount].SpawnerPosition.x, Levels[_levelCount].SpawnerPosition.y, -1);
        _spavner.transform.position = spawnerPosition;

        var mainBuilding = Instantiate(Levels[_levelCount].MainBuilding,
            _fieldModel.GetWorldCenterPosition(Levels[_levelCount].MainBuildingPosition.x, Levels[_levelCount].MainBuildingPosition.y, -1),
            Quaternion.identity, _levelContainer.transform);

        foreach (var fieldObject in Levels[_levelCount].FieldObjects)
        {
            Instantiate(fieldObject.Object,
                _fieldModel.GetWorldCenterPosition(fieldObject.Position.x, fieldObject.Position.y, -1),
                Quaternion.identity, _levelContainer.transform);
            _pathfind.SetWalkableState(fieldObject.Position, false);
        }

        _spavner.StartSpawn(_pathfind, mainBuilding.transform.position, _levelCount);
    }   

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _wallsAvalible > 0)
        {
            var targetPosition = _fieldModel.GetCellPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (targetPosition.x <= Levels[_levelCount].FieldWidth - 1 && targetPosition.y <= Levels[_levelCount].FieldHeight - 1)
            {
                if (targetPosition.x >= 0 && targetPosition.y >= 0)
                {
                    _wallsAvalible--;
                    _view.OnWallsCountChange.Invoke(_wallsAvalible);                    
                }              
            }

            _pathfind.SetWalkableState(targetPosition.x, targetPosition.y, false);
            _fieldModel.SetSprite(targetPosition.x, targetPosition.y, Levels[_levelCount].WallSelf);
        }

        if (Input.GetMouseButtonDown(1))
        {           
            _fieldModel.Reset();
            _pathfind.Reset();
        }
    }
}