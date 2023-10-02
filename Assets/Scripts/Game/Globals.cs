using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Globals : MonoBehaviour
{
    public static Globals Global = null;    
    public View View = new();    
    public SubscribableAction PlayerBlockPath = new();     
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
