using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;

public class Rocket : MonoBehaviour
{
    [SerializeField] float _speed = 0.005f;
    [SerializeField] int _damage;
    [SerializeField] Enemy _target;

    private View _view;

    private void Start()
    {
        _view = Globals.Global.View;
    }

    public void SetTarget(Enemy enemy)
    {        
        _target = enemy;
    }

    private void Update()
    {
        if (_target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
        }
        else
        {
            Destroy(gameObject);
        }
               
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            _view.OnRocketHitEnemy.Invoke(_damage, enemy);
            Destroy(gameObject);
        }
    }
}
