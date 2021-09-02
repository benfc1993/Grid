using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Grid3D.Grid
{
    public class Grid3DObjectManager : MonoBehaviour
    {
        [SerializeField] PlaceableObjectSO placeableObjectSO;
        GridDir _gridDir = GridDir.Down;
        Grid3D<GridObject> _activeGrid;
        Transform _gridTransform;
        
        Camera _worldCamera;

        GridObject _selectedObject;
        Transform _hasSelection;

        List<Vector2Int> _movedFromPositions;
            
        void Awake()
        {
            _worldCamera = Camera.main;
        }

        void Update()
        {
            if (_activeGrid == null) return; 
            HandleInput();
        }


        void LateUpdate()
        {
            if (_activeGrid == null || !_hasSelection) return;
            
            MoveSelection();
            RotateSelection();
        }

        public void SetActiveGrid(Transform gridTransform, Grid3D<GridObject> grid)
        {
            _gridTransform = gridTransform;
            _activeGrid = grid;
        }

        void ClearActiveGrid()
        {
            _gridTransform = null;
            _activeGrid = null;
        }

        void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
                HandleMouseClick();

            if (Input.GetMouseButtonDown(1))
            {
                _activeGrid.GetXY(Utilities.GetMouseWorldPosition(_worldCamera), out var x, out var y, out bool positionInGrid);
                if (positionInGrid) DeleteObject(_activeGrid.GetGridObject(x, y)?.GetPlacedObject());
            }

            if (Input.GetKeyDown(KeyCode.R))
                _gridDir = placeableObjectSO.GetNextDirection(_gridDir);
                
            if (Input.GetKeyDown(KeyCode.Escape) && _hasSelection)
                ResetSelectedObject();
        }

        void HandleMouseClick()
        {
            _activeGrid.GetXY(Utilities.GetMouseWorldPosition(_worldCamera), out var x, out var y, out bool positionInGrid);
            if (!positionInGrid) return;
            
            GridObject gridObject = _activeGrid.GetGridObject(x, y);

            if (gridObject.GetPlacedObject() && !_hasSelection)
                SelectObject(gridObject);
            else if (_hasSelection)
                PlaceSelectedObject(x, y);
            else if (TryPlaceObject(x, y))
                PlaceObject(x, y);
        }
        
        void RotateSelection()
        {
            if (!_hasSelection || !_selectedObject.GetPlaceableObjectSO().CanRotate()) return;
            
            Quaternion targetAngle = Quaternion.Euler(0, placeableObjectSO.GetRotationAngle(_gridDir), 0);
            _hasSelection.rotation = Quaternion.Lerp(_hasSelection.rotation, targetAngle, Time.deltaTime * 15f);
        }

        void MoveSelection()
        {
            _activeGrid.GetXY(Utilities.GetMouseWorldPosition(_worldCamera), out var x, out var y, out bool positionInGrid);
            if (!positionInGrid) return;
            Vector2Int offset = _selectedObject.GetPlaceableObjectSO().GetRotationOffset(_selectedObject.GetDir());
            Vector3 startPosition = _hasSelection.position;
            Vector3 targetPosition =
                _activeGrid.GetWorldPosition(x, y) + new Vector3(offset.x, 0, offset.y) * _activeGrid.GetCellSize();
            _hasSelection.position = Vector3.Lerp(startPosition,
                targetPosition, Time.deltaTime * 15f);
        }

        bool TryPlaceObject(int x, int y, PlaceableObjectSO movedPlaceableObjectSO = null)
        {
            PlaceableObjectSO placeableObjectToUse = movedPlaceableObjectSO ? movedPlaceableObjectSO : placeableObjectSO;
        
            var gridPositions = placeableObjectToUse.GetGridPositionList(new Vector2Int(x, y), _gridDir);
            
            foreach (var gridPosition in gridPositions)
            {
                var obj = _activeGrid.GetGridObject(gridPosition.x, gridPosition.y);
                if (obj == null || !obj.CanBuild())
                {
                    return false;
                }
            }
        
            return true;
        }

        void PlaceObject(int x, int y, PlaceableObjectSO movedPlaceableObjectSO = null)
        {
            PlaceableObjectSO placeableObjectToUse = movedPlaceableObjectSO ? movedPlaceableObjectSO : placeableObjectSO;
            Vector2Int rotationOffset = placeableObjectToUse.GetRotationOffset(_gridDir);

            var gridPositions = placeableObjectToUse.GetGridPositionList(new Vector2Int(x, y), _gridDir);

            Vector3 buildPosition = _activeGrid.GetWorldPosition(x, y) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * _activeGrid.GetCellSize();

            PlacedObject placedObject = PlacedObject.Create(buildPosition, new Vector2Int(x, y), _gridDir, placeableObjectToUse, _gridTransform);

            foreach (var gridPosition in gridPositions)
                _activeGrid.GetGridObject(gridPosition).SetPlacedObject(placedObject, _gridDir);
        }

        void SelectObject(GridObject gridObject)
        {
            _hasSelection = gridObject.GetPlacedObject().transform;
            _selectedObject = gridObject;
            _gridDir = _selectedObject.GetDir();
            _movedFromPositions = gridObject.GetPlacedObject().GetGridPositionList();
            
            foreach (var position in _selectedObject.GetPlacedObject().GetGridPositionList())
                _activeGrid.GetGridObject(position).SetIsTransitioning(true);
        }
        
        void PlaceSelectedObject(int x, int y)
        {
            PlaceableObjectSO toPlaceSO = _selectedObject.GetPlaceableObjectSO();
            var toPlace = _selectedObject.GetPlacedObject();
            if (TryPlaceObject(x, y, toPlaceSO))
            {
                var positions = toPlaceSO.GetGridPositionList(new Vector2Int(x, y), _gridDir);

                foreach (var fromPosition in _movedFromPositions)
                    _activeGrid.GetGridObject(fromPosition).ClearPLacedObject();


                foreach (var position in positions)
                {
                    _activeGrid.GetGridObject(position).SetPlacedObject(toPlace, _gridDir);
                    _activeGrid.GetGridObject(position).SetNewPosition(new Vector2Int(x, y));
                }
            }
            else
            {
                ResetSelectedObject();
            }
            
            ClearSelection();
        }

        void DeleteObject(PlacedObject toDelete)
        {
            toDelete.DestroySelf();
            foreach (var position in toDelete.GetGridPositionList())
                _activeGrid.GetGridObject(position).ClearPLacedObject();
        }

        void ResetSelectedObject()
        {
            _selectedObject.ResetPosition();
            _selectedObject.ResetRotation();

            foreach (Vector2Int position in _selectedObject.GetPlacedObject().GetGridPositionList())
                _activeGrid.GetGridObject(position).SetIsTransitioning(false);

            ClearSelection();
        }

        void ClearSelection()
        {
            _selectedObject = null;
            _hasSelection = null;
            _movedFromPositions = null;
        }
    }    
}
