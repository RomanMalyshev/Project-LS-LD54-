using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class Tower : MonoBehaviour
{
    [SerializeField] List<Enemy> _reachableEnemies = new List<Enemy>();
    [SerializeField] Collider2D _shootRadius;
    [SerializeField] bool _isActive;
    [SerializeField] Rocket _rocket;


    private void Start()
    {
        _shootRadius = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            _reachableEnemies.Add(enemy);
            _isActive = true;
        }

        InvokeRepeating("LaunchRocet", 0, 1);
    }

    private void Update()
    {
       // if (_isActive)
            //LaunchRocet();
    }

    private void LaunchRocet()
    {
        var rocket = Instantiate(_rocket, transform.position, Quaternion.identity);
        rocket.SetTarget(_reachableEnemies[0]);
    }
}
