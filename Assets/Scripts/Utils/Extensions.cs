using UnityEngine;

public static class Extensions
{
	public static bool Enveloping(this RectInt b, RectInt s)
	{
		return s.xMin >= b.xMin && s.xMax <= b.xMax && s.yMin >= b.yMin && s.yMax <= b.yMax;
	}
}
