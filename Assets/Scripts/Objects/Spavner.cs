using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utils;

public class Spavner : MonoBehaviour
{
    [SerializeField] List<SpawnWave> _spavnQueue;
    [SerializeField] float _spavnDelay;
    [SerializeField] private int _waveCount = 0;

    private View _view;
    private int _deadEnemies;
    private AStarPathfinding _pathFinder;
    private Vector3 _targetPosition;

    private void Start()
    {
        _view = Globals.Global.View;

        _view.OnEnemyDie.Subscribe((enemy) =>
        {
            EnemyDie();
        });               

        _view.OnWawesChange.Invoke(_waveCount, _spavnQueue.Count);
    }


    public void StartSpawn(AStarPathfinding pathFinder, Vector3 targetPosition)
    {
        _pathFinder = pathFinder;
        _targetPosition = targetPosition;
        StartCoroutine(SpavnNewWave(_pathFinder, _targetPosition));
    }


    private IEnumerator SpavnNewWave(AStarPathfinding startPos, Vector3 endPoint)
    {
        var enemyNumber = 0;
        _deadEnemies = 0;

        while (enemyNumber < _spavnQueue[_waveCount]._enemies.Count)
        {
            var enemy = Instantiate(_spavnQueue[_waveCount]._enemies[enemyNumber], transform.position, Quaternion.identity, transform);
            enemyNumber++;
            enemy.SetPath(startPos, endPoint);
            yield return new WaitForSeconds(_spavnDelay);
        }
    }

    private void EnemyDie()
    {
        _deadEnemies++;

        if (_deadEnemies == _spavnQueue[_waveCount]._enemies.Count)
        {
            if (_waveCount + 1 == _spavnQueue.Count)
            {
                _view.OnLevelWin.Invoke();
            }
            else
            {
                _waveCount++;
                _view.OnWawesChange.Invoke(_waveCount, _spavnQueue.Count);
                StartCoroutine(SpavnNewWave(_pathFinder, _targetPosition));
            }            
        }
    }
}

[Serializable]
public class SpawnWave
{
    public List<Enemy> _enemies;
}
