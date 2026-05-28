using System;
using UnityEngine;

public enum Direction
{
    Up = 1,
    Down = -1,
    Left = -2,
    Right = 2
}
[Serializable]
public class Entrance
{
    [field: SerializeField] public int x { get; set; }
    [field: SerializeField] public int y { get; set; }
    [field: SerializeField] public Direction Direction { get; set; }
	public bool InUse { get; set; }

    public Entrance(int x, int y)
    {
        this.x = x;
        this.y = y;
        Direction = default;
        InUse = false;
	}
	public Entrance(int x, int y, Direction direction)
    {
        this.x = x;
        this.y = y;
        Direction = direction;
        InUse = false;
	}

    public Vector2Int DirToVector2()
    {
        return Direction switch
        {
            Direction.Up => Vector2Int.up,
            Direction.Down => Vector2Int.down,
            Direction.Left => Vector2Int.left,
            Direction.Right => Vector2Int.right,
            _ => Vector2Int.zero
        };
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
		return a.x == b.x && a.y == b.y && a.InUse == b.InUse && a.Direction == b.Direction;
	}
    public static bool operator !=(Entrance a, Entrance b)
    {
        return !(a == b);
	}

    public static implicit operator Vector3Int(Entrance entrance)
    {
        return new Vector3Int(entrance.x, entrance.y, 0);
    }
	public static implicit operator Vector3(Entrance entrance)
    {
        return new Vector3(entrance.x, entrance.y, 0);
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

	public override bool Equals(object obj)
	{
		return obj is Entrance entrance &&
			   x == entrance.x &&
			   y == entrance.y &&
               Direction == entrance.Direction &&
			   InUse == entrance.InUse;
	}
	public override int GetHashCode()
	{
		return HashCode.Combine(x, y, Direction, InUse);
	}
}
