using System;
using UnityEngine;
using Utils;

public class TestField : MonoBehaviour
{
    private FieldModel _fieldModel;
    private void Start()
    {
        _fieldModel = new FieldModel(10, 10,5,transform.position);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _fieldModel.SetClickValue(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}
