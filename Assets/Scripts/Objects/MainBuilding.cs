using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBuilding : MonoBehaviour
{
    [SerializeField] private int  _health = 100;

    private View _view;

    private void Start()
    {
        _view = Globals.Global.View;
        _view.OnHPChange.Invoke(_health);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            TakeDamage(enemy._damage);
            enemy.EnemyDie();
        }
    }

    private void TakeDamage(int damage)
    {
        _health -= damage;
        _view.OnHPChange.Invoke(_health);

        if (_health <= 0)
        {
            DestroyMainBuilding();
        }
    }

    private void DestroyMainBuilding()
    {
        _view.OnLevelLost.Invoke();
        gameObject.SetActive(false);
    }
}
