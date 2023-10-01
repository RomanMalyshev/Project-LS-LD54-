using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _health;
    public int _damage;

    private View _view;
    private Coroutine _moveRoutine;

    private void Start()
    {
        _view = Globals.Global.View;
    }

    private void Update()
    {
        //transform.position = new Vector2(transform.position.x + _speed * Time.deltaTime, transform.position.y);
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            EnemyDie();
        }
    }

    public void EnemyDie()
    {
        _view.OnEnemyDie.Invoke(this);
        Destroy(gameObject);
    }

    public void SetPath(List<Vector3> worldPath)
    {
        if (_moveRoutine != null)
            StopCoroutine(_moveRoutine);

        _moveRoutine = StartCoroutine(MoveRoutine( worldPath));
    }

    private IEnumerator MoveRoutine( List<Vector3> worldPath)
    {
        while (worldPath.Count > 0)
        {
            var currentTarget = worldPath[0];
            while (currentTarget != transform.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentTarget, _speed * Time.deltaTime);
                yield return null;
            }

            worldPath.Remove(currentTarget);
            yield return null;
        }
    }
}