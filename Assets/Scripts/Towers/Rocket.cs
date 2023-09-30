using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float _speed = 0.005f;
    [SerializeField] private int _damage;
    [SerializeField] private Enemy _target;    

    public void SetTarget(Enemy enemy)
    {
        _target = enemy;
    }

    private void Update()
    {
        if (_target != null)
        {
            Vector3 moveDirection = _target.transform.position - transform.position;
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            transform.position = Vector2.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {            
            enemy.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
