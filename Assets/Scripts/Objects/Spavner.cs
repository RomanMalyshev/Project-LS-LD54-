using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Spavner : MonoBehaviour
{
    [SerializeField] List<Enemy> _spavnQueue;
    [SerializeField] int _spavnDelay;

    private void Start()
    {
       // StartCoroutine(SpavnEnemy());
    }

    public void StartSpawn(Vector3 startPos, List<Vector3> path)
    {
        StartCoroutine(SpavnEnemy(startPos, path));
    }


    private IEnumerator SpavnEnemy(Vector3 startPos, List<Vector3> path)
    {
        var enemyNumber = 0;

        while (enemyNumber < _spavnQueue.Count)
        {            
           var enemy =  Instantiate(_spavnQueue[enemyNumber], startPos, Quaternion.identity);
            enemyNumber++;
            enemy.SetPath(path);
            yield return new WaitForSeconds(_spavnDelay);            
        }
    }
}
