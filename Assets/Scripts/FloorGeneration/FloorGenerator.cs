using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorGenerator : MonoBehaviour
{
	[SerializeField] private Grid _grid;
	[SerializeField] private RectInt _floorSize;
	[SerializeField] private Vector2Int _entranceToFloor;
	[SerializeField] private FloorConstructionLibrary _library;
	private List<Room> _rooms = new();
	private void Awake()
	{
		if (_grid == null) _grid = GetComponent<Grid>();
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

		if (!PlaceRoom(s, _entranceToFloor, _floorSize))
		{
			Debug.LogError("Couldn't place starter room in " + name);
			return;
		}
		Generate(_rooms[^1]);
	}
	private void Generate(Room startingPoint)
	{

	}
	private bool PlaceRoom(Room room, Vector2Int entrance, RectInt validArea)
	{
		bool canPlace = false;
		int entranceIndex;
		for (entranceIndex = 0; !canPlace && entranceIndex < room.Entrances.Length; entranceIndex++)
		{ canPlace = validArea.Enveloping(new RectInt(room.Area.position - room.Entrances[entranceIndex], room.Area.size)); }
		if (!canPlace) return false;

		Vector2Int origin = entrance - room.Entrances[--entranceIndex];
		Room newRoom = Instantiate(room,
			_grid.CellToWorld((Vector3Int)origin)
				+ (Vector3)(room.Area.size * (Vector2)_grid.cellSize * 0.5f),
			Quaternion.identity, transform);

		_rooms.Add(newRoom);
		newRoom.Initialize(_grid.cellSize, origin);
		return true;
	}
	private void OnDrawGizmos()
	{
		if (_grid == null) return;
		Gizmos.color = new Color(0, 1, 0, 0.1f);

		Rect worldSize = new( _grid.CellToWorld((Vector3Int)_floorSize.position), _grid.CellToWorld((Vector3Int)_floorSize.size) );
		Gizmos.DrawCube(worldSize.center, worldSize.size);

		Gizmos.color = Color.red;
		Gizmos.DrawSphere(_grid.CellToWorld((Vector3Int)_entranceToFloor) + _grid.cellSize * 0.5f, 0.5f);
	}
}