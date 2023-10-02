using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class View 
{    
    public SubscribableAction<Enemy> OnEnemyDie = new();   
    public SubscribableAction OnLevelLost = new();   
    public SubscribableAction OnLevelWin = new();     
    public SubscribableAction OnLevelStart = new();     
    
    public SubscribableAction<int> OnHPChange = new();      
    public SubscribableAction<int> OnWallsCountChange = new();      
    public SubscribableAction<int, int> OnWawesChange = new();      

    public void Init()
    {

    }
}
