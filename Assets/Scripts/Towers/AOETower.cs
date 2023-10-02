using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AOETower : MonoBehaviour
{
    [SerializeField] private List<Enemy> _reachableEnemies = new List<Enemy>();
    [SerializeField] private float _attackSpeed = 1f;
    [SerializeField] private int _damage = 5;
    [SerializeField] private SpriteRenderer _atackSprite;

    private Color _color;
    private View _view;
    private Collider2D _shootRadius;

    private void Start()
    {
        _view = Globals.Global.View;
        _shootRadius = GetComponent<Collider2D>();
        _color = _atackSprite.color;

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
            StartCoroutine(AttackAnimation());
            for (int enemyNumber = 0; enemyNumber < _reachableEnemies.Count; enemyNumber++)
            {
                _reachableEnemies[enemyNumber].TakeDamage(_damage);
            }

            yield return new WaitForSeconds(_attackSpeed);
        }
    }

    private IEnumerator AttackAnimation()
    {
        _atackSprite.color = new Color(0.54f, 0f, 1f, 0.3f);
        yield return new WaitForSeconds(0.1f);
        _atackSprite.color = _color;

    }
}
