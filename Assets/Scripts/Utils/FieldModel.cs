using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class FieldModel
    {
        private readonly int _width;
        private readonly int _height;
        private readonly int _cellSize;
        private readonly Vector3 _originPosition;

        private readonly int[,] _fieldArray;
        private readonly Dictionary<(int x, int y), TextMesh> _clickTestInfo =  new ();
        public FieldModel(int width, int height, int cellSize,Vector3 originPosition)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;
            _originPosition = originPosition;
            _fieldArray = new int[_width, _height];

            DrawDebugField();
        }

        private void DrawDebugField()
        {
            var debugCells = new GameObject("DebugCells");
            for (var x = 0; x < _fieldArray.GetLength(0); x++)
            {
                for (var y = 0; y < _fieldArray.GetLength(1); y++)
                {
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.red, 1000f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.red, 1000f);

                    var cellIndexText = new GameObject("FieldTestCell", typeof(TextMesh));
                    cellIndexText.transform.SetParent(debugCells.transform);
                    cellIndexText.transform.localPosition = GetWorldPosition(x,y) + new Vector3(_cellSize,_cellSize)*0.5f;
                    var cellIndexTextMesh = cellIndexText.GetComponent<TextMesh>();
                    cellIndexTextMesh.text = $"{x}:{y}";
                    cellIndexTextMesh.anchor = TextAnchor.MiddleCenter;
                    cellIndexTextMesh.alignment = TextAlignment.Center;
                    
                    var clickInfo = new GameObject("FieldTestCell", typeof(TextMesh));
                    clickInfo.transform.SetParent(debugCells.transform);
                    clickInfo.transform.localPosition = GetWorldPosition(x,y);
                    var clickInfoTextMesh = clickInfo.GetComponent<TextMesh>();
                    clickInfoTextMesh.text = $"0";
                    clickInfoTextMesh.anchor = TextAnchor.LowerLeft;
                    clickInfoTextMesh.alignment = TextAlignment.Center;
                    
                    _clickTestInfo.Add((x,y),clickInfoTextMesh);
                }
            }

            Debug.DrawLine(GetWorldPosition(0, _height), GetWorldPosition(_width, _height), Color.red, 100f);
            Debug.DrawLine(GetWorldPosition(_width, 0), GetWorldPosition(_width, _height), Color.red, 100f);
        }


        private Vector2Int GetCellPosition(Vector3 worldPosition)
        {
            return new Vector2Int(Mathf.FloorToInt((worldPosition - _originPosition).x/_cellSize),Mathf.FloorToInt((worldPosition - _originPosition).y/_cellSize));
        }

        private Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * _cellSize + _originPosition;
        }

        public void SetClickValue(Vector3 worldPosition)
        {
            var cellPosition = GetCellPosition(worldPosition);
            if(cellPosition.x >= _width || cellPosition.y >= _height || cellPosition.x < 0 || cellPosition.y < 0)
                return;
            
            _clickTestInfo[(cellPosition.x, cellPosition.y)].text = "1";
        }
    }
}