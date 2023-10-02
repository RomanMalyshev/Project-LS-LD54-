using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class LazerTower : MonoBehaviour
{
    [SerializeField] private List<Enemy> _reachableEnemies = new List<Enemy>();
    [SerializeField] private float _attackSpeed = 1f;
    [SerializeField] private int _damage = 5;
    [SerializeField] private LineRenderer _lazer;

    private Color _color;
    private View _view;
    private Collider2D _shootRadius;

    private void Start()
    {
        _view = Globals.Global.View;
        _shootRadius = GetComponent<Collider2D>();        

        _view.OnEnemyDie.Subscribe((enemy) =>
        {
            EnemyDie(enemy);
        });

        StartCoroutine(Attack());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            _reachableEnemies.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            _reachableEnemies.Remove(enemy);
        }
    }

    private void EnemyDie(Enemy enemy)
    {
        _reachableEnemies.Remove(enemy);
    }

    private IEnumerator Attack()
    {
        while (true)
        {            
            for (int enemyNumber = 0; enemyNumber < _reachableEnemies.Count; enemyNumber++)
            {                
                StartCoroutine(AttackAnimation(enemyNumber));
                _reachableEnemies[enemyNumber].TakeDamage(_damage);
            }

            yield return new WaitForSeconds(_attackSpeed);
        }
    }

    private IEnumerator AttackAnimation(int enemyNumber)
    {
        var laser = Instantiate(_lazer, new Vector3(transform.position.x, transform.position.y, -1), Quaternion.identity,transform);
        laser.SetPosition(0, new Vector3(transform.position.x, transform.position.y, -2));
        laser.SetPosition(1, new Vector3(_reachableEnemies[enemyNumber].transform.position.x, _reachableEnemies[enemyNumber].transform.position.y, -2));
        yield return new WaitForSeconds(0.1f);
        Destroy(laser.gameObject);
    }
}
