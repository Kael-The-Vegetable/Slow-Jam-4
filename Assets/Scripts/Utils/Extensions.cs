using System;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public static class Extensions
{
	public static bool Enveloping(this RectInt b, RectInt s)
	{
		return s.xMin >= b.xMin && s.xMax <= b.xMax && s.yMin >= b.yMin && s.yMax <= b.yMax;
	}

	// TODO: needs work
	public static RectInt LargestFixedPointArea(this RectInt bounds, Vector2Int point, RectInt[] blockers)
	{
		bool[,] blocked = BuildGrid(bounds, blockers);
		int px = point.x - bounds.xMin;
		int py = point.y - bounds.yMin;

		// Cast rays from the required point in each direction
		int clearLeft = CastRay(blocked, px, py, -1, 0, bounds.width, bounds.height);
		int clearRight = CastRay(blocked, px, py, 1, 0, bounds.width, bounds.height);
		int clearDown = CastRay(blocked, px, py, 0, -1, bounds.width, bounds.height);
		int clearUp = CastRay(blocked, px, py, 0, 1, bounds.width, bounds.height);

		RectInt best = new RectInt(point.x, point.y, 1, 1);
		int bestArea = 1;
		
		for (int l = 0; l <= clearLeft; l++)
			for (int r = 0; r <= clearRight; r++)
				for (int d = 0; d <= clearDown; d++)
					for (int u = 0; u <= clearUp; u++)
					{
						int xMin = px - l;
						int xMax = px + r;
						int yMin = py - d;
						int yMax = py + u;

						// Verify the full rectangle is clear (rays alone don't guarantee corners are clear)
						if (!IsRectClear(blocked, xMin, yMin, xMax, yMax))
							continue;

						int area = (xMax - xMin + 1) * (yMax - yMin + 1);
						if (area > bestArea)
						{
							bestArea = area;
							best = new RectInt(
								bounds.xMin + xMin,
								bounds.yMin + yMin,
								xMax - xMin + 1,
								yMax - yMin + 1
							);
						}
					}

		return best;
	}
	private static int CastRay(bool[,] blocked, int px, int py, int dx, int dy, int W, int H)
	{
		// Ensure starting position is itself in bounds
		if (px < 0 || px >= W || py < 0 || py >= H) return 0;

		int steps = 0;
		int x = px + dx;
		int y = py + dy;
		while (x >= 0 && x < W && y >= 0 && y < H && !blocked[x, y])
		{
			steps++;
			x += dx;
			y += dy;
		}
		return steps;
	}
	private static bool[,] BuildGrid(RectInt bounds, IList<RectInt> blockers)
	{
		int W = bounds.width, H = bounds.height;
		bool[,] grid = new bool[W, H];
		foreach (var b in blockers)
		{
			int x0 = Mathf.Clamp(b.xMin - bounds.xMin, 0, W);
			int x1 = Mathf.Clamp(b.xMax - bounds.xMin, 0, W);
			int y0 = Mathf.Clamp(b.yMin - bounds.yMin, 0, H);
			int y1 = Mathf.Clamp(b.yMax - bounds.yMin, 0, H);
			for (int x = x0; x < x1; x++)
				for (int y = y0; y < y1; y++)
					grid[x, y] = true;
		}
		return grid;
	}
	private static bool IsRectClear(bool[,] blocked, int xMin, int yMin, int xMax, int yMax)
	{
		int W = blocked.GetLength(0);
		int H = blocked.GetLength(1);

		// Clamp to grid bounds before iterating
		xMin = Mathf.Clamp(xMin, 0, W - 1);
		xMax = Mathf.Clamp(xMax, 0, W - 1);
		yMin = Mathf.Clamp(yMin, 0, H - 1);
		yMax = Mathf.Clamp(yMax, 0, H - 1);

		for (int x = xMin; x <= xMax; x++)
			for (int y = yMin; y <= yMax; y++)
				if (blocked[x, y]) return false;

		return true;
	}
}
