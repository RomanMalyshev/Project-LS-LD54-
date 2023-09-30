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

    private View _view;


    private void Start()
    {
        _view = Globals.Global.View;
        _shootRadius = GetComponent<Collider2D>();

        _view.OnEnemyDie.Subscribe((enemy) =>
        {
            EnemyDie(enemy);
        });

        StartCoroutine(LaunchRocet());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            _reachableEnemies.Add(enemy);  
        }
    }

    private void Update()
    {
       
    }

    private void EnemyDie(Enemy enemy)
    {
        _reachableEnemies.Remove(enemy);
    }    

    private IEnumerator LaunchRocet()
    {
        while (_reachableEnemies.Count > 0)
        {
            var rocket = Instantiate(_rocket, transform.position, Quaternion.identity);
            rocket.SetTarget(_reachableEnemies[0]);

            yield return new WaitForSeconds(1);
        }        
    }
}
