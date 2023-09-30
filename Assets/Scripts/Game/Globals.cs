using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{
    public static Globals Global = null;    
    public View View = new();    

    public void Awake()
    {
        if (!ReferenceEquals(Global, null))
        {
            Debug.LogWarning("It's another Globals!");
            return;
        }

        Global = this;
    }

    private void Start()
    {
        View.Init();        
    }
}
