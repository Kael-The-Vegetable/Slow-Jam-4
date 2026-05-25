using System;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
	public static bool Enveloping(this RectInt b, RectInt s)
	{
		return s.xMin >= b.xMin && s.xMax <= b.xMax && s.yMin >= b.yMin && s.yMax <= b.yMax;
	}
	public static RectInt LargestFixedPointArea(this RectInt area, Vector2Int entrance, RectInt[] blockers)
	{
		var candidateX = new SortedSet<int>() { area.xMin, area.xMax };
		var candidateY = new SortedSet<int>() { area.yMin, area.yMax };

		for (int i = 0; i < blockers.Length; i++)
		{
			RectInt a = blockers[i];

			if (a.xMin > area.xMin) candidateX.Add(a.xMin);
			if (a.xMax < area.xMax) candidateX.Add(a.xMax);
			if (a.yMin > area.yMin) candidateY.Add(a.yMin);
			if (a.yMax < area.yMax) candidateY.Add(a.yMax);
		}
		List<int> xs = new(candidateX);
		List<int> ys = new(candidateY);
		RectInt cur = new RectInt(entrance, Vector2Int.zero);
		int bestArea = 0;

		for (int li = 0; li < xs.Count - 1; li++)
		{ // li = left index
			int lX = xs[li];
			if (lX > entrance.x) break; // no area past this works

			int[] rightBound = new int[ys.Count - 1]; // gaps
			Array.Fill(rightBound, area.xMax);

			// sweep right
			for (int ri = li + 1; ri < xs.Count; ri++)
			{ // ri = right index
				int rX = xs[ri];

				for (int ai = 0; ai < blockers.Length; ai++)
				{
					RectInt a = blockers[ai];
					if (a.xMin >= rX || a.xMax <= lX) continue; // no intersection
					for (int bi = 0; bi < ys.Count - 1; bi++)
					{ // bi = bottom index
						int bY = ys[bi];
						int tY = ys[bi + 1];
						if (a.yMin >= tY || a.yMax <= bY) continue; // no intersection
						rightBound[bi] = Math.Min(rightBound[bi], a.xMin > lX ? a.xMin : lX);
					}
				}

				if (entrance.x < lX || entrance.x >= rX) continue; // no intersection with entrance

				int pointRowBand = -1;
				for (int i = 0; i < ys.Count - 1; i++)
				{
					int bY = ys[i];
					int tY = ys[i + 1];
					if (entrance.y < bY || entrance.y >= tY) continue; // no intersection with entrance
					pointRowBand = i;
					break;
				}

				if (pointRowBand == -1) continue; // no intersection with entrance
				if (rightBound[pointRowBand] < rX) continue; // point's row is blocked

				int b = pointRowBand, t = pointRowBand;
				while (b > 0 && rightBound[b - 1] >= rX) b--;
				while (t < ys.Count - 2 && rightBound[t + 1] >= rX) t++;

				int y1 = ys[b];
				int y2 = ys[t + 1];
				int curArea = (rX - lX) * (y2 - y1);
				if (curArea > bestArea)
				{
					bestArea = curArea;
					cur = new RectInt(lX, y1, rX - lX, y2 - y1);
				}
			}
		}
		return cur;
	}
}
