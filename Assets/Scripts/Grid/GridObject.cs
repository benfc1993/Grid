using UnityEngine;

namespace Grid3D.Grid
{
    public class GridObject
    {
        readonly Grid3D<GridObject> _grid3D;

        readonly int _x;
        readonly int _y;
        GridDir _gridDir;
        PlacedObject _placedObject;
        bool _isTransitioning;

        int _value;

        public GridObject(Grid3D<GridObject> grid3D, int x, int y)
        {
            _x = x;
            _y = y;
            _grid3D = grid3D;
        }

        public void SetPlacedObject(PlacedObject placedObject, GridDir gridDir = GridDir.Down)
        {
            _placedObject = placedObject;
            _grid3D.TriggerGridObjectChanged(_x, _y);
            _gridDir = gridDir;
            _isTransitioning = false;
        }

        public void SetNewPosition(Vector2Int gridOrigin) => _placedObject.SetNewPosition(_grid3D.GetWorldPosition(gridOrigin), gridOrigin);

        public void ClearPLacedObject()
        {
            _placedObject = null;
            _isTransitioning = false;
            _grid3D.TriggerGridObjectChanged(_x, _y);
        }

        public bool CanBuild() => _placedObject == null || _isTransitioning;

        public override string ToString() => $"{_x}, {_y}";
        
        public PlacedObject GetPlacedObject() => _placedObject;
    
        public void ResetPosition()
        {
            var offset = _placedObject.GetPlaceableObjectSO().GetRotationOffset(_gridDir);

            var newPosition = _grid3D.GetWorldPosition(_placedObject.GetGridOrigin()) +
                              new Vector3(offset.x, 0, offset.y) * _grid3D.GetCellSize();
            _placedObject.ResetPosition(newPosition);
        }

        public void SetIsTransitioning(bool isTransitioning)
        {
            _isTransitioning = isTransitioning;
            _grid3D.TriggerGridObjectChanged(_x, _y);
        }

        public PlaceableObjectSO GetPlaceableObjectSO() => _placedObject.GetPlaceableObjectSO();

        public GridDir GetDir() => _gridDir;

        public void SetDir(GridDir gridDir) => _gridDir = gridDir;

        public void ResetRotation() =>
            _placedObject.transform.rotation = Quaternion.Euler(0, _placedObject.GetPlaceableObjectSO().GetRotationAngle(_gridDir), 0);
    }
}