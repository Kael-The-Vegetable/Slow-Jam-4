using UnityEngine;
using System;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "FloorConstructionLibrary", menuName = "Scriptable Objects/FloorConstructionLibrary")]
public class FloorConstructionLibrary : ScriptableObject
{
	[field: SerializeField] public Room[] RoomTemplates { get; private set; } = new Room[0];

	public Room GetRoomForSpace(RectInt area, Room.RoomType type, Vector2Int entrance)
	{
		Room[] candidates = FilterRooms(area, type, entrance);
		return candidates.Length > 0 ? candidates[Random.Range(0, candidates.Length)] : null;
	}
	public Room GetStarterRoom()
	{
		Room[] candidates = Array.FindAll(RoomTemplates, r => r.Type == Room.RoomType.Starting);
		return candidates.Length > 0 ? candidates[Random.Range(0, candidates.Length)] : null;
	}
	public Room[] FilterRooms(RectInt area, Room.RoomType type = Room.RoomType.Normal, Vector2Int entrance = default)
	{
		if (entrance == default)
		{
			return Array.FindAll(RoomTemplates, 
				r => r.Area.size.x <= area.width && r.Area.size.y <= area.height
				&& r.Type == type);
		}
		else
		{
			return Array.FindAll(RoomTemplates, 
				r => r.Area.size.x <= area.width && r.Area.size.y <= area.height 
				&& r.Type == type
				&& Array.Exists(r.Entrances, e => e == entrance));
		}
	}
}
