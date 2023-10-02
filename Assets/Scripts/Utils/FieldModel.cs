using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class FieldModel
    {
        public enum CellState
        {
            selectable,
            notSelectable
        }

        private readonly int _width;
        private readonly int _height;
        private readonly int _cellSize;
        private readonly Vector3 _originPosition;

        private readonly Sprite _sprite;

        private readonly Dictionary<(int x, int y), SpriteRenderer> _posToSprite = new();
        private Dictionary<(int x, int y), CellState> _posToSelectable = new();
        private List<Vector2Int> _cellsPos = new();
        private Transform _fieldParent;
        public int GetWidth() => _width;
        public int GetHeight() => _height;
        public List<Vector2Int> GetCells() => _cellsPos;

        public FieldModel(int width, int height, int cellSize, Vector3 originPosition, Sprite cellSprite,
            Transform fieldParent = null)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;
            _originPosition = originPosition;
            _sprite = cellSprite;
            _fieldParent = fieldParent;

            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    _cellsPos.Add(new Vector2Int(x, y));
                    _posToSelectable.Add((x, y), CellState.selectable);
                }
            }

            DrawDebugField();
        }

        private void DrawDebugField()
        {
            var debugCells = new GameObject("DebugCells");
            debugCells.transform.SetParent(_fieldParent);
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    // Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.red, 1000f);
                    // Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.red, 1000f);

                    var sprite = new GameObject("FieldTestSprite", typeof(SpriteRenderer));
                    sprite.transform.SetParent(debugCells.transform);
                    sprite.transform.localPosition = GetWorldPosition(x, y) + new Vector3(_cellSize, _cellSize) * 0.5f;
                    sprite.transform.localScale *= _cellSize;
                    var spriteRenderer = sprite.GetComponent<SpriteRenderer>();
                    spriteRenderer.color = Color.white;
                    spriteRenderer.sprite = _sprite;

                    var cellIndexText = new GameObject("FieldTestCell", typeof(TextMesh));
                    cellIndexText.transform.SetParent(debugCells.transform);
                    cellIndexText.transform.localPosition = GetWorldPosition(x, y) + new Vector3(_cellSize, _cellSize) * 0.5f;
                    var cellIndexTextMesh = cellIndexText.GetComponent<TextMesh>();
                    cellIndexTextMesh.text = $"{x}:{y}";
                    cellIndexTextMesh.anchor = TextAnchor.MiddleCenter;
                    cellIndexTextMesh.alignment = TextAlignment.Center;
                    cellIndexTextMesh.fontStyle = FontStyle.Bold;
                    
                    _posToSprite.Add((x, y), spriteRenderer);
                }
            }

            // Debug.DrawLine(GetWorldPosition(0, _height), GetWorldPosition(_width, _height), Color.red, 100f);
            //Debug.DrawLine(GetWorldPosition(_width, 0), GetWorldPosition(_width, _height), Color.red, 100f);
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

        public Vector3 GetWorldPosition(int x, int y, float z)
        {
            var worldPosition = new Vector3(x, y) * _cellSize + _originPosition;
            return new Vector3(worldPosition.x, worldPosition.y, z);
        }

        public Vector3 GetWorldCenterPosition(int x, int y, float z)
        {
            var worldPosition = new Vector3(x, y) * _cellSize + _originPosition;
            return new Vector3(worldPosition.x + _cellSize / 2f, worldPosition.y + _cellSize / 2f, z);
        }

        public void Reset()
        {
            foreach (var cMesh in _posToSprite)
            {
                cMesh.Value.color = Color.white;
                cMesh.Value.sprite = _sprite;
            }
        }

        public void SetColor(int x, int y, Color color)
        {
            if (_posToSprite.ContainsKey((x, y)))
                _posToSprite[(x, y)].color = color;
        }

        public void SetSprite(int x, int y, Sprite sprite)
        {
            if (_posToSprite.ContainsKey((x, y)))
                _posToSprite[(x, y)].sprite = sprite;
        }

        public void SetSelectable(int x, int y, CellState state)
        {
            if (_posToSelectable.ContainsKey((x, y)))
                _posToSelectable[(x, y)] = state;
        }

        public void SetSelectable(Vector2Int position, CellState state)
        {
            if (_posToSelectable.ContainsKey((position.x, position.y)))
                _posToSelectable[(position.x, position.y)] = state;
        }

        public CellState GetCellState(Vector2Int position)
        {
            return _posToSelectable[(position.x, position.y)];
        }

        public bool CellPositionExist(Vector2Int position)
        {
            return _posToSprite.ContainsKey((position.x, position.y));
        }
    }
}