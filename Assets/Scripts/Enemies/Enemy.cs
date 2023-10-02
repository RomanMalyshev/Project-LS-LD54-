using System.Collections;
using Unity.VisualScripting;
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

    public void SetPath(AStarPathfinding startPos, Vector3 endPoint)
    {
        if (_moveRoutine != null)
            StopCoroutine(_moveRoutine);

        _moveRoutine = StartCoroutine(MoveRoutine(startPos, endPoint));
    }

    private IEnumerator MoveRoutine(AStarPathfinding pathFinder, Vector3 endPoint)
    {
        var path = pathFinder.FindWorldPath(transform.position, endPoint);

        while (path != null && path.Count > 0)
        {
            var currentTarget = path[1];
            Vector3? nextCurrentTarget = path.Count > 2 ? path[2] : null;
            while (currentTarget != transform.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentTarget, _speed * Time.deltaTime);
                yield return null;
            }

            path.Remove(currentTarget);
            path = pathFinder.FindWorldPath(transform.position, endPoint);
            if (path == null)
            {
                if (nextCurrentTarget != null)
                    path = pathFinder.FindWorldPath(nextCurrentTarget.Value, endPoint);
                
                if (path == null)
                {
                    Globals.Global.PlayerBlockPath.Invoke();
                    Debug.Log("No path");
                    path = pathFinder.FindWorldPath(transform.position, endPoint);
                }
                else
                {
                    while (nextCurrentTarget.Value != transform.position)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, nextCurrentTarget.Value, _speed * Time.deltaTime);
                        yield return null;
                    } 
                }
            }

            yield return null;
        }
    }
}