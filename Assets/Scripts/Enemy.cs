using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{    
    [SerializeField] float _speed;
    [SerializeField] float _health;
    [SerializeField] float _damage;

    private View _view;

    private void Start()
    {
        _view = Globals.Global.View;

        _view.OnRocketHitEnemy.Subscribe((damage, enemy) =>
        {
            TakeDamage(damage, enemy);
        });
    }

    private void Update()
    {
        transform.position = new Vector2(transform.position.x + _speed*Time.deltaTime, transform.position.y); 
    }

    private void TakeDamage(float damage, Enemy enemy)
    {
        if (enemy == this)
        {
            _health -= damage;           
        }

        if (_health <= 0)
        {
            EnemyDie();
        }
    }

    private void EnemyDie()
    {
        Destroy(gameObject);
    }

}
