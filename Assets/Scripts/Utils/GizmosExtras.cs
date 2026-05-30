using UnityEngine;

public static class GizmosExtras
{
	public static void DrawEntrance(Vector2 localSpace, Vector2 offset, Vector2 scale, Entrance entrance, Color color)
	{
		Vector2 pos = localSpace + (entrance + offset) * scale;
		Gizmos.color = color;
		if (entrance.InUse)
			DrawX(pos, scale * 0.3f);
		else
			DrawArrow(pos, entrance.DirToVector2(), scale * 0.3f);
	}

	private static void DrawX(Vector3 pos, Vector2 size)
	{
		Vector3 a = new Vector3(size.x, size.y, 0);
		Vector3 b = new Vector3(-size.x, size.y, 0);
		Gizmos.DrawLine(pos - a, pos + a);
		Gizmos.DrawLine(pos - b, pos + b);
	}

	private static void DrawArrow(Vector2 pos, Vector2 dir, Vector2 size)
	{
		Vector2 perp = new(-dir.y, dir.x); // 90 degrees

		float len = (dir.y != 0) ? size.y : size.x;
		float head = len * 0.5f;

		Vector2 tail = pos - dir * len;
		Vector2 tip = pos + dir * len;

		// Shaft
		Gizmos.DrawLine(tail, tip);

		// Arrowhead
		Gizmos.DrawLine(tip, tip - dir * head + perp * head);
		Gizmos.DrawLine(tip, tip - dir * head - perp * head);
	}
}
