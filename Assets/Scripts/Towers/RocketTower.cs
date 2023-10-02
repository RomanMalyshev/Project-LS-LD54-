using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class RocketTower : MonoBehaviour
{
    [SerializeField] private List<Enemy> _reachableEnemies = new List<Enemy>();
    [SerializeField] private float _attackSpeed = 1f;    
    [SerializeField] private bool _isActive;
    [SerializeField] private Rocket _rocket;

    private Collider2D _shootRadius;
    private View _view;

    private void Start()
    {
        _view = Globals.Global.View;
        _shootRadius = GetComponent<Collider2D>();

        _view.OnEnemyDie.Subscribe((enemy) =>
        {
            EnemyDie(enemy);
        });               
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            _reachableEnemies.Add(enemy);

            if (!_isActive)
            {
                StartCoroutine(LaunchRocket());
            }

            _isActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            _reachableEnemies.Remove(enemy);

            if (_reachableEnemies.Count <= 0)
            {
                _isActive = false;
            }
        }
    }

    private void EnemyDie(Enemy enemy)
    {
        _reachableEnemies.Remove(enemy);
    }

    private IEnumerator LaunchRocket()
    {        
        while (_reachableEnemies.Count > 0)
        {
            var rocket = Instantiate(_rocket, new Vector3(transform.position.x,transform.position.y,-1), Quaternion.identity);
            rocket.SetTarget(_reachableEnemies[0]);
            rocket.transform.SetParent(transform);
            //Debug.Log(rocket.transform.position);
            yield return new WaitForSeconds(_attackSpeed);
        }
    }
}
