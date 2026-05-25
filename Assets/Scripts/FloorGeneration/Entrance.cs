using System;
using UnityEngine;

[Serializable]
public struct Entrance
{
    [field: SerializeField] public int x { get; set; }
    [field: SerializeField] public int y { get; set; }
    public bool InUse { get; set; }
	public Entrance(int x, int y)
    {
        this.x = x;
        this.y = y;
        InUse = false;
	}

    public static Entrance operator +(Entrance a, Entrance b) => new(a.x + b.x, a.y + b.y);
    public static Vector2Int operator +(Vector2Int a, Entrance b) => new(a.x + b.x, a.y + b.y);
    public static Vector2Int operator +(Entrance a, Vector2Int b) => new(a.x + b.x, a.y + b.y);
	public static Entrance operator -(Entrance a, Entrance b) => new(a.x - b.x, a.y - b.y);
    public static Entrance operator -(Entrance a) => new Entrance(-a.x, -a.y);
    public static Vector2Int operator -(Vector2Int a, Entrance b) => new(a.x - b.x, a.y - b.y);
    public static Vector2Int operator -(Entrance a, Vector2Int b) => new(a.x - b.x, a.y - b.y);
	public static Entrance operator *(Entrance a, int b) => new(a.x * b, a.y * b);
    public static Entrance operator *(int a, Entrance b) => new(a * b.x, a * b.y);
	public static Entrance operator /(Entrance a, int b) => new(a.x / b, a.y / b);
	public static Entrance operator /(int a, Entrance b) => new(a / b.x, a / b.y);
	public static bool operator ==(Entrance a, Entrance b)
    {
		return a.x == b.x && a.y == b.y && a.InUse == b.InUse;
	}
    public static bool operator !=(Entrance a, Entrance b)
    {
        return !(a == b);
	}

	public static implicit operator Vector2Int(Entrance entrance)
    {
        return new Vector2Int(entrance.x, entrance.y);
    }
    public static implicit operator Vector2(Entrance entrance)
    {
        return new Vector2(entrance.x, entrance.y);
	}
	public static implicit operator Entrance(Vector2Int vector)
    {
        return new Entrance(vector.x, vector.y);
    }

	public override readonly bool Equals(object obj)
	{
		return obj is Entrance entrance &&
			   x == entrance.x &&
			   y == entrance.y &&
			   InUse == entrance.InUse;
	}
	public override readonly int GetHashCode()
	{
		return HashCode.Combine(x, y, InUse);
	}
}
