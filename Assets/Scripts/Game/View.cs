using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class View 
{    
    public SubscribableAction<Enemy> OnEnemyDie = new();   
    public SubscribableAction OnLevelLost = new();   
    public SubscribableAction OnLevelWin = new();      

    public void Init()
    {

    }
}
