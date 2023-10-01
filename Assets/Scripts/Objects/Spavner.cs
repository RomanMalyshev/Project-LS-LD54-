using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Spavner : MonoBehaviour
{
    [SerializeField] List<Enemy> _spavnQueue;
    [SerializeField] int _spavnDelay;
        
    private void Start()
    {
       // StartCoroutine(SpavnEnemy());
    }


    public void StartSpawn(AStarPathfinding pathFinder,Vector3 targetPosition)
    {
        StartCoroutine(SpavnEnemy(pathFinder, targetPosition));
    }


    private IEnumerator SpavnEnemy(AStarPathfinding startPos, Vector3 endPoint)
    {
        var enemyNumber = 0;

        while (enemyNumber < _spavnQueue.Count)
        {            
           var enemy =  Instantiate(_spavnQueue[enemyNumber], transform.position, Quaternion.identity,transform);
            enemyNumber++;
            enemy.SetPath(startPos,endPoint);
            yield return new WaitForSeconds(_spavnDelay);            
        }
    }

}
