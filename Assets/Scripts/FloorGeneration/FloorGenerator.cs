using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
	[SerializeField] private Grid _grid;
	[SerializeField] private RectInt _floorSize;
	[SerializeField] private Vector2Int _startingRoom;


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