using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Utils;

public class Globals : MonoBehaviour
{
    public static Globals Global = null;    
    public View View = new();    
    public SubscribableAction PlayerBlockPath = new();

    public UIController UIController;
    public TestField TestField;
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
        UIController.Init();
        TestField.Init();
    }
}
