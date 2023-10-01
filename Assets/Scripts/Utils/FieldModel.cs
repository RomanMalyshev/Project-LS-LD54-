using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
    public class FieldModel
    {
        private readonly int _width;
        private readonly int _height;
        private readonly int _cellSize;
        private readonly Vector3 _originPosition;

        private readonly Sprite _sprite;

        private readonly Vector2Int[] _fieldArray;
        private readonly Dictionary<(int x, int y), TextMesh> _clickTestInfo = new();
        private readonly Dictionary<(int x, int y), SpriteRenderer> _posToSprite = new();

        public int GetWidth() => _width;
        public int GetHeight() => _height;

        public FieldModel(int width, int height, int cellSize, Vector3 originPosition, Sprite cellSprite)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;
            _originPosition = originPosition;
            _fieldArray = new Vector2Int[_width * _height];
            _sprite = cellSprite;
            DrawDebugField();
        }

        private void DrawDebugField()
        {
            var debugCells = new GameObject("DebugCells");
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.red, 1000f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.red, 1000f);

                    var sprite = new GameObject("FieldTestSprite", typeof(SpriteRenderer));
                    sprite.transform.SetParent(debugCells.transform);
                    sprite.transform.localPosition = GetWorldPosition(x, y) + new Vector3(_cellSize, _cellSize) * 0.5f;
                    sprite.transform.localScale *= _cellSize;
                    var spriteRenderer = sprite.GetComponent<SpriteRenderer>();
                    spriteRenderer.color = Color.black;
                    spriteRenderer.sprite = _sprite;

                    _posToSprite.Add((x, y), spriteRenderer);

                    var cellIndexText = new GameObject("FieldTestCell", typeof(TextMesh));
                    cellIndexText.transform.SetParent(debugCells.transform);
                    cellIndexText.transform.localPosition =
                        GetWorldPosition(x, y) + new Vector3(_cellSize, _cellSize) * 0.5f;
                    var cellIndexTextMesh = cellIndexText.GetComponent<TextMesh>();
                    cellIndexTextMesh.text = $"{x}:{y}";
                    cellIndexTextMesh.anchor = TextAnchor.MiddleCenter;
                    cellIndexTextMesh.alignment = TextAlignment.Center;


                    var clickInfo = new GameObject("FieldTestCell", typeof(TextMesh));
                    clickInfo.transform.SetParent(debugCells.transform);
                    clickInfo.transform.localPosition = GetWorldPosition(x, y);
                    var clickInfoTextMesh = clickInfo.GetComponent<TextMesh>();
                    clickInfoTextMesh.text = $"0";
                    clickInfoTextMesh.anchor = TextAnchor.LowerLeft;
                    clickInfoTextMesh.alignment = TextAlignment.Center;

                    _clickTestInfo.Add((x, y), clickInfoTextMesh);
                }
            }

            Debug.DrawLine(GetWorldPosition(0, _height), GetWorldPosition(_width, _height), Color.red, 100f);
            Debug.DrawLine(GetWorldPosition(_width, 0), GetWorldPosition(_width, _height), Color.red, 100f);
        }

        public Vector2Int GetCellPosition(Vector3 worldPosition)
        {
            return new Vector2Int(Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize),
                Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize));
        }

        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * _cellSize + _originPosition;
        }

        public void Reset()
        {
            foreach (var cMesh in _clickTestInfo)
            {
                cMesh.Value.text = "0";
            }

            foreach (var cMesh in _posToSprite)
            {
                cMesh.Value.color = Color.black;
            }
        }

        public void SetColor(int x, int y, Color color)
        {
            if (_posToSprite.ContainsKey((x, y)))
                _posToSprite[(x, y)].color = color;
        }
    }
}