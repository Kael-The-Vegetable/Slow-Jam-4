using System.Collections.Generic;
using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
	[SerializeField] private Grid _grid;
	[SerializeField] private RectInt _floorSize;
	[SerializeField] private Entrance _entranceToFloor;
	[SerializeField] private FloorConstructionLibrary _library;
	private List<Room> _rooms = new();
	private List<RectInt> _occupiedAreas = new();
	private void Awake()
	{
		if (_grid == null) _grid = GetComponent<Grid>();
		if (_library == null)
		{
			Debug.LogError("No FloorConstructionLibrary assigned to " + name);
			return;
		}

		Room s = _library.GetStarterRoom(_floorSize, _entranceToFloor);
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
		Queue<Entrance> entrancesToExpand = new();
		foreach (Entrance entrance in startingPoint.GetFreeEntrances()) entrancesToExpand.Enqueue(entrance);
		int count = 0;
		while (entrancesToExpand.Count > 0 && count < 1000)
		{
			count++;
			Entrance entrance = entrancesToExpand.Dequeue();
			RectInt vArea = GetValidArea(entrance);
			
			Room room = _library.GetRoomForSpace(vArea, entrance, Room.RoomType.Normal);
			Debug.Log(room);

			if (room == null) continue;
			if (!PlaceRoom(room, entrance, vArea)) continue;

			Entrance[] newEntrances = _rooms[^1].GetFreeEntrances();
			for (int i = 0; i < newEntrances.Length; i++) entrancesToExpand.Enqueue(newEntrances[i]);
		}
		
		if (count >= 1000) Debug.LogError("Reached maximum number of iterations while generating " + name);
	}
	private RectInt GetValidArea(Entrance entrance)
	{
		return _floorSize.LargestFixedPointArea(entrance, _occupiedAreas.ToArray());
	}
	private bool PlaceRoom(Room room, Entrance entrance, RectInt validArea)
	{
		bool canPlace = false;
		int entranceIndex;
		for (entranceIndex = 0; !canPlace && entranceIndex < room.Entrances.Length; entranceIndex++)
		{ canPlace = validArea.Enveloping(new RectInt(entrance + room.Entrances[entranceIndex].DirToVector2() + room.Area.position - room.Entrances[entranceIndex], room.Area.size)); }
		if (!canPlace) return false;

		Vector2Int origin = entrance - room.Entrances[--entranceIndex] + room.Entrances[entranceIndex].DirToVector2();

		Room newRoom = Instantiate(room,
			(Vector2)_grid.CellToWorld((Vector3Int)origin)
				+ (room.Area.size * (Vector2)_grid.cellSize * 0.5f),
			Quaternion.identity, transform);


		newRoom.Initialize(_grid.cellSize, origin);
		_rooms.Add(newRoom);
		_occupiedAreas.Add(newRoom.Area);

		entrance.ConnectedTo = newRoom.Entrances[entranceIndex];
		newRoom.Entrances[entranceIndex].ConnectedTo = entrance;
		return true;
	}
	private void OnDrawGizmos()
	{
		if (_grid == null) return;
		Gizmos.color = new Color(0, 1, 0, 0.1f);

		Rect worldSize = new( _grid.CellToWorld((Vector3Int)_floorSize.position), _grid.CellToWorld((Vector3Int)_floorSize.size) );
		Gizmos.DrawCube(worldSize.center, worldSize.size);

		GizmosExtras.DrawEntrance(transform.position, Vector2.one / 2, _grid.cellSize, _entranceToFloor, Color.red);
	}
}