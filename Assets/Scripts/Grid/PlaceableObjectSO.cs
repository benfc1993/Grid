using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grid3D.Grid
{
    [CreateAssetMenu(fileName = "New Placeable Object", menuName = "Grid/Placeable Object")]
    public class PlaceableObjectSO : ScriptableObject
    {
        [SerializeField] public Transform prefab;
        [SerializeField] int width;
        [SerializeField] int height;
        [SerializeField] bool canRotate = true;
        List<Vector2Int> _gridPositionList = new List<Vector2Int>();

        
        public List<Vector2Int> GetGridPositionList(Vector2Int offset = default, GridDir gridDir = default)
        {
            _gridPositionList = new List<Vector2Int>(width * height);
            switch (gridDir)
            {
                case GridDir.Down:
                case GridDir.Up:
                    for (var x = 0; x < width; x++)
                    for (var y = 0; y < height; y++)
                        _gridPositionList.Add(offset + new Vector2Int(x, y));

                    break;
                case GridDir.Left:
                case GridDir.Right:
                    for (var x = 0; x < height; x++)
                    for (var y = 0; y < width; y++)
                        _gridPositionList.Add(offset + new Vector2Int(x, y));

                    break;
            }

            return _gridPositionList;
        }

        public GridDir GetNextDirection(GridDir gridDir) => (GridDir) (((int) gridDir + 1) % Enum.GetNames(typeof(GridDir)).Length);

        public int GetRotationAngle(GridDir gridDir) => (int) gridDir * 90;

        public Vector2Int GetRotationOffset(GridDir gridDir)
        {
            switch (gridDir)
            {
                default:
                    return Vector2Int.zero;
                case GridDir.Right:
                    return new Vector2Int(0, width);
                case GridDir.Down:
                    return new Vector2Int(width, height);
                case GridDir.Left:
                    return new Vector2Int(height, 0);
            }
        }

        public bool CanRotate() => canRotate;
    }
}