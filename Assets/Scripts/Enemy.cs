using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _health;
    [SerializeField] private float _damage;

    private View _view;

    private void Start()
    {
        _view = Globals.Global.View;

        _view.OnRocketHitEnemy.Subscribe((damage, enemy) =>
        {
            //TakeDamage(damage, enemy);
        });
    }

    private void Update()
    {
        transform.position = new Vector2(transform.position.x + _speed * Time.deltaTime, transform.position.y);
    }

    public void TakeDamage(float damage)
    {

        _health -= damage;


        if (_health <= 0)
        {
            _view.OnEnemyDie.Invoke(this);
            EnemyDie();
        }
    }

    private void EnemyDie()
    {
        /* _view.OnRocketHitEnemy.Unsubscribe((damage, enemy) =>
         {
             TakeDamage(damage, enemy);
         });*/

        Destroy(gameObject);
    }

}
