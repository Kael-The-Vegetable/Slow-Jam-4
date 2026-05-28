using System;
using UnityEngine;

[Serializable]
public struct Improvement
{
	public enum Modifier
	{
		Damage,
		Defense,
		Durability,
		Speed,
	}
	[field: SerializeField] public EquipSlot Location { get; private set; }
	[field: SerializeField] public Modifier Type { get; private set; }
	[field: SerializeField] public int Value { get; private set; }
}
