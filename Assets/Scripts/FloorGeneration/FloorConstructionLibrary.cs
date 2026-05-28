using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "FloorConstructionLibrary", menuName = "Scriptable Objects/FloorConstructionLibrary")]
public class FloorConstructionLibrary : ScriptableObject
{
	[field: SerializeField] public Room[] RoomTemplates { get; private set; } = new Room[0];

	public Room GetRoomForSpace(RectInt area, Entrance entrance, Room.RoomType type)
	{
		Room[] candidates = FilterRooms(area, type);
		List<Room> validCandidates = new List<Room>();
		for (int i = 0; i < candidates.Length; i++)
		{
			Entrance[] entrances = candidates[i].GetEntrancesForDirection(entrance.Direction);
			for (int j = 0; j < entrances.Length; j++)
			{
				if (area.Enveloping(new RectInt(
					entrance + candidates[i].Area.position - entrances[j],
					candidates[i].Area.size)))
				{
					validCandidates.Add(candidates[i]);
					break;
				}
			}
		}

		return validCandidates.Count > 0 ? validCandidates[Random.Range(0, validCandidates.Count)] : null;
	}
	public Room GetStarterRoom(RectInt floor, Entrance floorStart)
	{
		return GetRoomForSpace(floor, floorStart, Room.RoomType.Starting);
	}
	public Room[] FilterRooms(RectInt area, Room.RoomType type = Room.RoomType.Normal)
	{
		return Array.FindAll(RoomTemplates, 
				r => r.Area.size.x <= area.width && r.Area.size.y <= area.height
				&& r.Type == type);
	}
}
