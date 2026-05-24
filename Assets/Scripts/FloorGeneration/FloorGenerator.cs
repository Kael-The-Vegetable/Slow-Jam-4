using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorGenerator : MonoBehaviour
{
	[SerializeField] private Grid _grid;
	[SerializeField] private RectInt _floorSize;
	[SerializeField] private Vector2Int _startingRoom;
	[SerializeField] private FloorConstructionLibrary _library;

	private void Awake()
	{
		if (_grid == null) _grid = GetComponent<Grid>();
		if (_map == null) _map = GetComponentInChildren<Tilemap>();
		if (_library == null)
		{
			Debug.LogError("No FloorConstructionLibrary assigned to " + name);
			return;
		}

		Room s = _library.GetStarterRoom();
		if (s == null)
		{
			Debug.LogError("No starter room found in " + _library.name);
			return;
		}
		
		Instantiate(s, _grid.CellToWorld((Vector3Int)_startingRoom) + _grid.cellSize * 0.5f, Quaternion.identity, transform);
		s.Initialize(_grid.cellSize);
	}
	private void Generate()
	{

	}

	private void OnDrawGizmos()
	{
		if (_grid == null) return;
		Gizmos.color = new Color(0, 1, 0, 0.1f);

		Rect worldSize = new( _grid.CellToWorld((Vector3Int)_floorSize.position), _grid.CellToWorld((Vector3Int)_floorSize.size) );
		Gizmos.DrawCube(worldSize.center, worldSize.size);

		Gizmos.color = Color.red;
		Gizmos.DrawSphere(_grid.CellToWorld((Vector3Int)_startingRoom) + _grid.cellSize * 0.5f, 0.5f);
	}
}