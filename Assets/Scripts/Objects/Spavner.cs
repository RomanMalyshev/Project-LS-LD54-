using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utils;

public class Spavner : MonoBehaviour
{
    [SerializeField] private List<Level> _levels;
    [SerializeField] float _spavnDelay;
    [SerializeField] private int _waveCount = 0;

    private View _view;
    private int _deadEnemies;
    private AStarPathfinding _pathFinder;
    private Vector3 _targetPosition;
    private int _currentLevel;

    private void Start()
    {
        _view = Globals.Global.View;

        _view.OnEnemyDie.Subscribe(EnemyDie);               

        _view.OnWawesChange.Invoke(_waveCount, _levels[0]._spavnQueue.Count);
    }


    public void StartSpawn(AStarPathfinding pathFinder, Vector3 targetPosition, int currentLevel)
    {
        _pathFinder = pathFinder;
        _targetPosition = targetPosition;
        _currentLevel = currentLevel;
        StartCoroutine(SpavnNewWave(_pathFinder, _targetPosition));
    }


    private IEnumerator SpavnNewWave(AStarPathfinding startPos, Vector3 endPoint)
    {
        var enemyNumber = 0;
        _deadEnemies = 0;

        while (enemyNumber < _levels[_currentLevel]._spavnQueue[_waveCount]._enemies.Count)
        {
            var enemy = Instantiate(_levels[_currentLevel]._spavnQueue[_waveCount]._enemies[enemyNumber], transform.position, Quaternion.identity, transform);
            enemyNumber++;
            enemy.SetPath(startPos, endPoint);
            yield return new WaitForSeconds(_spavnDelay);
        }
    }

    private void EnemyDie(Enemy enemy)
    {
        _deadEnemies++;        

        if (_deadEnemies == _levels[_currentLevel]._spavnQueue[_waveCount]._enemies.Count)
        {
            if (_waveCount + 1 == _levels[_currentLevel]._spavnQueue.Count)
            {
                _view.OnLevelWin.Invoke();
            }
            else
            {
                _waveCount++;
                _view.OnWawesChange.Invoke(_waveCount, _levels[_currentLevel]._spavnQueue.Count);
                StartCoroutine(SpavnNewWave(_pathFinder, _targetPosition));
            }            
        }
    }

    private void OnDestroy()
    {        
        _view.OnEnemyDie.Unsubscribe(EnemyDie);    
    }
}


