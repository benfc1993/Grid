using System;
using UnityEngine;
using Utils;

namespace Grid3D.Grid
{
    public class Grid3D<TGridType>
    {
        public Action<int, int> onGridValueChanged;
        readonly TGridType[,] _grid;
        Vector3 _origin;
        readonly int _width;
        readonly int _height;
        readonly int _cellSize;
        GridDir _gridDir;

        readonly TextMesh[,] _debugTextArray;
        bool showDebug = true;

        public Grid3D(int width, int height, int cellSize, Vector3 origin, GridDir dir, Func<Grid3D<TGridType>, int, int, TGridType> createGridObject)
        {
            _grid = new TGridType[width, height];
            _origin = origin;
            _debugTextArray = new TextMesh[width, height];
            _width = width;
            _height = height;
            _cellSize = cellSize;
            _gridDir = dir;

            CreateGrid(createGridObject);
        }
        
        void CreateGrid(Func<Grid3D<TGridType>, int, int, TGridType> createGridObject)
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    _grid[x, y] = createGridObject(this, x, y);
                }
            }
        }

        void CreateDebug(int x, int y)
        {
            _debugTextArray[x, y] = Utilities.CreateWorldText(_grid[x, y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(_cellSize, 0, _cellSize) * 0.5f, new Vector3(90, 0, 0), 20, Color.white, TextAnchor.MiddleCenter);
            Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, Single.PositiveInfinity);
            Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, Single.PositiveInfinity);
        }

        //TODO: Add in grid rotation to Get World Position
        public Vector3 GetWorldPosition(int x, int y) => new Vector3(x, 0, y) * _cellSize + _origin;
        public Vector3 GetWorldPosition(Vector2Int position) => new Vector3(position.x, 0, position.y) * _cellSize + _origin;


        public void GetXY(Vector3 worldPosition, out int x, out int y, out bool positionInGrid)
        {
            //TODO: Add in grid rotation to GetXY
            x = Mathf.FloorToInt((worldPosition.x - _origin.x) / _cellSize);
            y = Mathf.FloorToInt((worldPosition.z - _origin.z) / _cellSize);
            
            positionInGrid = x >= 0 && y >= 0 && x < _width && y < _height;
        }

        public void SetGridObject(int x, int y, TGridType value)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _height)
            {
                _grid[x, y] = value;
                _debugTextArray[x, y].text = value.ToString();  
            }
        }

        public void SetGridObject(Vector3 worldPosition, TGridType value)
        {
            GetXY(worldPosition, out var x, out var y, out bool positionInGrid);
            if (positionInGrid) 
                SetGridObject(x, y, value);
        }

        public TGridType GetGridObject(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _height)
            {
                return _grid[x, y];
            }
            else
            {
                return default(TGridType);
            }
        }
        
        public TGridType GetGridObject(Vector2Int position)
        {
            if (position.x >= 0 && position.y >= 0 && position.x < _width && position.y < _height)
            {
                return _grid[position.x, position.y];
            }
            else
            {
                return default(TGridType);
            }
        }

        public TGridType GetGridObject(Vector3 worldPosition)
        {
            GetXY(worldPosition, out var x, out var y, out bool positionInGrid);
            return positionInGrid ? GetGridObject(x, y) : default(TGridType);
        }

        public void TriggerGridObjectChanged(int x, int y)
        {
            onGridValueChanged?.Invoke(x, y);
        }

        public int GetCellSize() => _cellSize;

        public void SetOrigin(Vector3 newOrigin)
        {
            _origin = newOrigin;
            DrawDebug();
        }

        public Vector3 GetOrigin() => _origin;
        
        public GridDir GetDir() => _gridDir;

        public void SetDir(GridDir gridDir) => _gridDir = gridDir;

        void DrawDebug()
        {
            if (!showDebug) return;
            
            Debug.DrawLine(GetWorldPosition(0, _height), GetWorldPosition(_width, _height), Color.white, Single.PositiveInfinity);
            Debug.DrawLine(GetWorldPosition(_width, 0), GetWorldPosition(_width, _height), Color.white, Single.PositiveInfinity);
            onGridValueChanged += (changedX, changedY) => _debugTextArray[changedX, changedY].text = _grid[changedX, changedY]?.ToString();
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    CreateDebug(x, y);
                }
            }
        }
    }
}

