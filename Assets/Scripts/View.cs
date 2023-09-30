using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class View 
{
    public SubscribableAction<int, Enemy> OnRocketHitEnemy = new();
    public SubscribableAction<Enemy> OnEnemyDie = new();

    public void Init()

    {

    }
}
