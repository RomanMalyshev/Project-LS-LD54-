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
            for (int i = 0; i < _reachableEnemies.Count; i++)
            {                
                StartCoroutine(AttackAnimation(i));
                _reachableEnemies[i].TakeDamage(_damage);
            }

            yield return new WaitForSeconds(_attackSpeed);
        }
    }

    private IEnumerator AttackAnimation(int i)
    {
        var laser = Instantiate(_lazer, transform.position, Quaternion.identity);
        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, _reachableEnemies[i].transform.position);
        yield return new WaitForSeconds(0.1f);
        Destroy(laser);
    }
}
