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
		int W = bounds.width;
		int H = bounds.height;
		int px = point.x - bounds.xMin;
		int py = point.y - bounds.yMin;

		bool[,] blocked = new bool[W, H];
		foreach (var b in blockers)
		{
			int x0 = Mathf.Clamp(b.xMin - bounds.xMin, 0, W);
			int x1 = Mathf.Clamp(b.xMax - bounds.xMin, 0, W);
			int y0 = Mathf.Clamp(b.yMin - bounds.yMin, 0, H);
			int y1 = Mathf.Clamp(b.yMax - bounds.yMin, 0, H);
			for (int x = x0; x < x1; x++)
				for (int y = y0; y < y1; y++)
					blocked[x, y] = true;
		}

		int[] heights = new int[W];
		RectInt best = new RectInt(point.x, point.y, 1, 1);
		int bestArea = 1;

		for (int row = 0; row < H; row++)
		{
			for (int x = 0; x < W; x++)
				heights[x] = blocked[x, row] ? 0 : heights[x] + 1;

			var stack = new Stack<int>();
			for (int x = 0; x <= W; x++)
			{
				int h = x == W ? 0 : heights[x];
				while (stack.Count > 0 && h < heights[stack.Peek()])
				{
					int height = heights[stack.Pop()];
					int left = stack.Count > 0 ? stack.Peek() + 1 : 0;
					int right = x - 1;
					int top = row;
					int bottom = row - height + 1;

					if (px >= left && px <= right && py >= bottom && py <= top)
					{
						int area = (right - left + 1) * height;
						if (area > bestArea)
						{
							bestArea = area;
							best = new RectInt(
								bounds.xMin + left,
								bounds.yMin + bottom,
								right - left + 1,
								height
							);
						}
					}
				}
				stack.Push(x);
			}
		}
		return best;
	}
}
