using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct FloatRange
{
	public float Min;
	public float Max;
	public FloatRange(float min, float max)
	{
		Min = min;
		Max = max;
	}
	public readonly float GetRand()
	{
		return Random.Range(Min, Max);
	}
}

[Serializable]
public struct IntRange
{
	public int Min;
	public int Max;
	public IntRange(int min, int max)
	{
		Min = min;
		Max = max;
	}
	public readonly int GetRand()
	{
		return Random.Range(Min, Max);
	}
}
