using Grid3D.Grid;
using UnityEngine;

public class GridHolder : MonoBehaviour
{
    [SerializeField] Grid3DObjectManager grid3DObjectManager;
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] int cellSize;
    Grid3D<GridObject> _grid;
    PlacedObject _placedObject;
    
    void Start()
    {
        grid3DObjectManager = FindObjectOfType<Grid3DObjectManager>();
        _placedObject = GetComponent<PlacedObject>();
        if (_placedObject) _placedObject.onPositionChanged += OnGridPositionChanged;
        CreateGrid();
    }

    void CreateGrid()
    {
        int rotatedWidth = width;
        int rotatedHeight = height;
        if (_placedObject)
        {
            rotatedWidth = _placedObject.GetDir() == GridDir.Left || _placedObject.GetDir() == GridDir.Right ? height : width;
            rotatedHeight = _placedObject.GetDir() == GridDir.Left || _placedObject.GetDir() == GridDir.Right ? width : height;    
        }
        
        _grid = new Grid3D<GridObject>(rotatedWidth, rotatedHeight, cellSize, GetOriginWithRotation(), GetRotation(), (g, x, y) => new GridObject(g, x, y));
    }

    GridDir GetRotation()
    {
        if (_placedObject == null) return GridDir.Up;
        return _placedObject.GetDir();
    }

    Vector3 GetOriginWithRotation()
    {
        if (_placedObject == null) return transform.position;
        
        var bottomLeft = _placedObject.GetWorldPosition();

        switch (_placedObject.GetDir())
        {
            case GridDir.Up:
                return bottomLeft;
            case GridDir.Right:
                bottomLeft += new Vector3(0, 0, -width) * cellSize;
                break;
            case GridDir.Down:
                bottomLeft += new Vector3(-width, 0, -height) * cellSize;
                break;
            case GridDir.Left:
                bottomLeft += new Vector3(-height, 0, 0) * cellSize;
                break;
        }

        return bottomLeft;
    }

    void OnGridPositionChanged()
    {
        _grid.SetOrigin(_placedObject.GetWorldPosition());
        _grid.SetDir(_placedObject.GetDir());
    }

    [ContextMenu("Set as active grid")]
    public void SetAsActiveGrid() => grid3DObjectManager.SetActiveGrid(transform, _grid);
}
