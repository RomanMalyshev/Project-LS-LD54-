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
        StartCoroutine(SpavnEnemy());
    }

    private IEnumerator SpavnEnemy()
    {
        var enemyNumber = 0;

        while (enemyNumber < _spavnQueue.Count)
        {            
            Instantiate(_spavnQueue[enemyNumber], transform.position, Quaternion.identity);
            enemyNumber++;

            yield return new WaitForSeconds(_spavnDelay);            
        }
    }
}
