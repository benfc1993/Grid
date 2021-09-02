using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grid3D.Grid
{
    public class PlacedObject : MonoBehaviour
    {
        [SerializeField] PlaceableObjectSO placeableObjectSO;
        Vector2Int _gridOrigin;
        Vector3 _worldPosition;
        GridDir _gridDir;
        public Action onPositionChanged; 

        public List<Vector2Int> GetGridPositionList() => placeableObjectSO.GetGridPositionList(_gridOrigin, _gridDir);

        public static PlacedObject Create(Vector3 worldPosition, Vector2Int gridOrigin, GridDir gridDir,
            PlaceableObjectSO placeableObjectSO, Transform parent = null)
        {
            Transform placedObjectTransform = Instantiate(placeableObjectSO.prefab, worldPosition,
                Quaternion.Euler(0, placeableObjectSO.GetRotationAngle(gridDir), 0), parent ? parent : null);

            PlacedObject placedObject = placedObjectTransform.gameObject.AddComponent<PlacedObject>();
            placedObject._gridDir = gridDir;
            placedObject._gridOrigin = gridOrigin;
            placedObject.placeableObjectSO = placeableObjectSO;
            placedObject._worldPosition = worldPosition;

            return placedObject;
        }

        public void SetNewPosition(Vector3 worldPosition, Vector2Int gridOrigin)
        {
            _gridOrigin = gridOrigin;
            _worldPosition = worldPosition;
            onPositionChanged?.Invoke();
        }

        public Vector2Int GetGridOrigin() => _gridOrigin;

        public Vector3 GetWorldPosition() => _worldPosition;
        
        public void ResetPosition(Vector3 newPosition)
        {
            transform.position = newPosition;
        }

        public PlaceableObjectSO GetPlaceableObjectSO() => placeableObjectSO;

        public GridDir GetDir() => _gridDir;
        
        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}