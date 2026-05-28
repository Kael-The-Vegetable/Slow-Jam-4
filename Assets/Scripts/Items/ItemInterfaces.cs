using System;

public interface IEquipable
{
	public EquipSlot Slot { get; }
	public Junk[] JunkImprovements { get; }
	public int MaxDurability { get; }
	public int CurrentDurability { get; set; }
}

[Flags]
public enum EquipSlot
{
	Head	= 1 << 0,
	Body	= 1 << 1,
	Legs	= 1 << 2,
	Feet	= 1 << 3,
	Hands	= 1 << 4,
	Weapon	= 1 << 5,
	Shield	= 1 << 6
}

public interface IJunk
{
	public Improvement[] Improvements { get; }
}

public interface IStackable
{
	public int MaxSize { get; }
	public int CurrentSize { get; }
}